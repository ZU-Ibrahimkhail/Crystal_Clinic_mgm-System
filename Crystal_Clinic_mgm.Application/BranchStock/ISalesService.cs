using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public interface ISalesService
    {
        /// <summary>
        /// Creates a sales estimate (quote) for a patient
        /// </summary>
        Task<Result> CreateEstimateAsync(CreateEstimateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Converts an estimate to a sales invoice
        /// </summary>
        Task<Result> ConvertEstimateToInvoiceAsync(int estimateId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates tax for a given amount
        /// </summary>
        Task<decimal> CalculateTaxAsync(decimal amount, int taxId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets available tax rates
        /// </summary>
        Task<IEnumerable<TaxRateDto>> GetTaxRatesAsync(CancellationToken cancellationToken = default);
    }

    public class CreateEstimateRequest
    {
        public int PatientId { get; set; }
        public IEnumerable<EstimateItem> Items { get; set; } = new List<EstimateItem>();
        public int? TaxId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class EstimateItem
    {
        public int? ItemId { get; set; }
        public int ServiceId { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class SalesEstimateDto
    {
        public int Id { get; set; }
        public string EstimateNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime EstimateDate { get; set; }
        public DateTime ValidUntil { get; set; }
        public IEnumerable<EstimateItemDto> Items { get; set; } = new List<EstimateItemDto>();
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class EstimateItemDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class SalesInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public IEnumerable<InvoiceItemDto> Items { get; set; } = new List<InvoiceItemDto>();
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class InvoiceItemDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class TaxRateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}