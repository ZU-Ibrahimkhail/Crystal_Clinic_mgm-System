using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public class SalesService : ISalesService
    {
        private readonly ERP_DbContext _context;
        private readonly ISalesInvoiceService _salesInvoiceService;
        private readonly ILogger<SalesService> _logger;
        private readonly ILoggedInUser _loggedInUser;

        public SalesService(
            ERP_DbContext context,
            ISalesInvoiceService salesInvoiceService,
            ILogger<SalesService> logger,
            ILoggedInUser loggedInUser)
        {
            _context = context;
            _salesInvoiceService = salesInvoiceService;
            _logger = logger;
            _loggedInUser = loggedInUser;
        }

        public async Task<Result> CreateEstimateAsync(CreateEstimateRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Validate patient exists
                var patient = await _context.Patient.FindAsync(request.PatientId);
                if (patient == null)
                {
                    return Result.Fail("Patient not found");
                }

                // Generate estimate number
                var estimateNumber = await GenerateEstimateNumberAsync(cancellationToken);

                // Calculate totals
                var (subtotal, lines) = await CalculateEstimateLinesAsync(request.Items, cancellationToken);
                var taxAmount = request.TaxId.HasValue ?
                    await CalculateTaxAsync(subtotal, request.TaxId.Value, cancellationToken) : 0;
                var discountAmount = subtotal * (request.DiscountPercentage / 100);
                var totalAmount = subtotal + taxAmount - discountAmount;

                // Create estimate
                var estimate = new SalesEstimate
                {
                    EstimateNumber = estimateNumber,
                    PatientId = request.PatientId,
                    EstimateDate = DateTime.UtcNow,
                    ValidUntil = DateTime.UtcNow.AddDays(30), // Default 30 days validity
                    Subtotal = subtotal,
                    TaxAmount = taxAmount,
                    DiscountAmount = discountAmount,
                    TotalAmount = totalAmount,
                    Status = EstimateStatus.Draft,
                    Notes = request.Notes,
                    BranchId = _loggedInUser.BranchId,
                    CreatedBy = _loggedInUser.Id
                };

                _context.SalesEstimates.Add(estimate);
                await _context.SaveChangesAsync(cancellationToken);

                // Create estimate lines
                foreach (var line in lines)
                {
                    var estimateLine = new SalesEstimateLine
                    {
                        SalesEstimateId = estimate.Id,
                        ServiceId = line.ServiceId,
                        ItemId = line.ItemId,
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice,
                        TotalPrice = line.TotalPrice,
                        Description = line.Description,
                        CreatedBy = _loggedInUser.Id
                    };

                    _context.SalesEstimateLines.Add(estimateLine);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                var estimateDto = await MapToEstimateDtoAsync(estimate, cancellationToken);
                return Result.Success(estimateDto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error creating sales estimate for patient {PatientId}", request.PatientId);
                return Result.Fail("Failed to create sales estimate");
            }
        }

        public async Task<Result> ConvertEstimateToInvoiceAsync(int estimateId, CancellationToken cancellationToken = default)
        {
            var estimate = await _context.SalesEstimates
                .Include(e => e.Patient)
                .Include(e => e.EstimateLines)
                .ThenInclude(l => l.Service)
                .FirstOrDefaultAsync(e => e.Id == estimateId && !e.IsDeleted, cancellationToken);

            if (estimate == null)
            {
                return Result.Fail("Estimate not found");
            }

            if (estimate.Status == EstimateStatus.Converted)
            {
                return Result.Fail("Estimate has already been converted");
            }

            if (estimate.ValidUntil < DateTime.UtcNow)
            {
                return Result.Fail("Estimate has expired");
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Create sales invoice
                var invoiceDto = new CreateSalesInvoiceDto
                {
                    PatientId = estimate.PatientId,
                    InvoiceDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(30), // Default 30 days payment terms
                    Lines = estimate.EstimateLines.Select(l => new SalesInvoiceLineDto
                    {
                        ServiceId = l.ServiceId,
                        ItemId = l.ItemId,
                        Quantity = l.Quantity,
                        UnitPrice = l.UnitPrice,
                        Description = l.Description
                    }).ToList(),
                    TaxAmount = estimate.TaxAmount,
                    DiscountAmount = estimate.DiscountAmount,
                    Notes = $"Converted from estimate {estimate.EstimateNumber}"
                };

                var invoiceResult = await _salesInvoiceService.CreateSalesInvoiceAsync(invoiceDto, cancellationToken);
                if (!invoiceResult.IsSuccess)
                {
                    return Result.Fail($"Failed to create invoice: {invoiceResult.Error}");
                }

                // Update estimate status
                estimate.Status = EstimateStatus.Converted;
                estimate.ConvertedToInvoiceId = (int?)invoiceResult.Data; // Assuming the result contains the invoice ID
                estimate.ModifiedBy = _loggedInUser.Id;
                estimate.ModifiedOn = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                // Get the created invoice details
                var invoiceQueryResult = await _salesInvoiceService.GetSalesInvoiceByIdAsync((int)invoiceResult.Data, cancellationToken);
                if (invoiceQueryResult.Succeeded && invoiceQueryResult.Data is SalesInvoiceDto invoice)
                {
                    return Result.Success(invoiceQueryResult.Data, "Success");
                }

                return Result.Fail("Invoice created but failed to retrieve details");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error converting estimate {EstimateId} to invoice", estimateId);
                return Result.Fail("Failed to convert estimate to invoice");
            }
        }

        public async Task<decimal> CalculateTaxAsync(decimal amount, int taxId, CancellationToken cancellationToken = default)
        {
            // This is a simplified implementation
            // In a real system, you'd have a TaxRate table with different tax types
            var taxRate = taxId switch
            {
                1 => 0.10m, // 10% VAT
                2 => 0.05m, // 5% Service Tax
                _ => 0.0m   // No tax
            };

            return amount * taxRate;
        }

        public async Task<IEnumerable<TaxRateDto>> GetTaxRatesAsync(CancellationToken cancellationToken = default)
        {
            // This is a simplified implementation
            // In a real system, you'd query a TaxRate table
            return new List<TaxRateDto>
            {
                new TaxRateDto { Id = 1, Name = "VAT", Rate = 0.10m, Description = "Value Added Tax 10%", IsActive = true },
                new TaxRateDto { Id = 2, Name = "Service Tax", Rate = 0.05m, Description = "Service Tax 5%", IsActive = true },
                new TaxRateDto { Id = 3, Name = "No Tax", Rate = 0.0m, Description = "No tax applicable", IsActive = true }
            };
        }

        private async Task<string> GenerateEstimateNumberAsync(CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow;
            var baseNumber = $"EST-{today:yyyyMMdd}";
            var count = await _context.SalesEstimates
                .CountAsync(e => e.EstimateNumber.StartsWith(baseNumber), cancellationToken);
            return $"{baseNumber}-{(count + 1):D3}";
        }

        private async Task<(decimal Subtotal, List<EstimateLineData> Lines)> CalculateEstimateLinesAsync(
            IEnumerable<EstimateItem> items, CancellationToken cancellationToken)
        {
            var lines = new List<EstimateLineData>();
            decimal subtotal = 0;

            foreach (var item in items)
            {
                string description = string.Empty;
                decimal unitPrice = item.UnitPrice;

                // If it's a service, get service details
                if (item.ServiceId > 0)
                {
                    var service = await _context.Services.FindAsync(item.ServiceId);
                    if (service != null)
                    {
                        description = service.Name;
                        unitPrice = service.FeeAmount; // Use service price if not specified
                    }
                }

                // If it's an item, get item details
                if (item.ItemId > 0)
                {
                    var inventoryItem = await _context.Items.FindAsync(item.ItemId);
                    if (inventoryItem != null)
                    {
                        description = inventoryItem.Name;
                        unitPrice = inventoryItem.UnitCost; // Use item cost as default price
                    }
                }

                var totalPrice = item.Quantity * unitPrice;
                subtotal += totalPrice;

                lines.Add(new EstimateLineData
                {
                    ServiceId = item.ServiceId,
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice,
                    Description = string.IsNullOrEmpty(item.Description) ? description : item.Description
                });
            }

            return (subtotal, lines);
        }

        private async Task<SalesEstimateDto> MapToEstimateDtoAsync(SalesEstimate estimate, CancellationToken cancellationToken)
        {
            var lines = await _context.SalesEstimateLines
                .Where(l => l.SalesEstimateId == estimate.Id)
                .Include(l => l.Service)
                .Include(l => l.Item)
                .Select(l => new EstimateItemDto
                {
                    ServiceId = l.ServiceId,
                    ServiceName = l.Service != null ? l.Service.Name : (l.Item != null ? l.Item.Name : "Unknown"),
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice,
                    TotalPrice = l.TotalPrice,
                    Description = l.Description
                })
                .ToListAsync(cancellationToken);

            return new SalesEstimateDto
            {
                Id = estimate.Id,
                EstimateNumber = estimate.EstimateNumber,
                PatientId = estimate.PatientId,
                PatientName = estimate.Patient?.name ?? "Unknown",
                EstimateDate = estimate.EstimateDate,
                ValidUntil = estimate.ValidUntil,
                Items = lines,
                Subtotal = estimate.Subtotal,
                TaxAmount = estimate.TaxAmount,
                DiscountAmount = estimate.DiscountAmount,
                TotalAmount = estimate.TotalAmount,
                Status = estimate.Status.ToString()
            };
        }
    }

    internal class EstimateLineData
    {
        public int ServiceId { get; set; }
        public int? ItemId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}