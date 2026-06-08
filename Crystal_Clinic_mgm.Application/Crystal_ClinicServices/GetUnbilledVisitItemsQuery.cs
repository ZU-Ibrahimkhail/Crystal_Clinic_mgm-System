using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    public class GetUnbilledVisitItemsQuery : IRequest<Result>
    {
        public int VisitId { get; set; }
    }

    public class GetUnbilledVisitItemsQueryHandler(ERP_DbContext context) : IRequestHandler<GetUnbilledVisitItemsQuery, Result>
    {
        public async Task<Result> Handle(GetUnbilledVisitItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var visit = await context.Visit
                    .FirstOrDefaultAsync(v => v.visitId == request.VisitId && !v.IsDeleted, cancellationToken);

                if (visit == null)
                    return Result.Fail("Visit not found.");

                var response = new UnbilledVisitItemsDto { VisitId = request.VisitId };

                // Get all medications for this visit
                var visitMedications = await context.VisitMedication
                    .Where(vm => vm.visitId == request.VisitId && !vm.IsDeleted)
                    .ToListAsync(cancellationToken);

                // Get all kits for this visit
                var visitKits = await context.VisitKits
                    .Where(vk => vk.VisitId == request.VisitId && !vk.IsDeleted)
                    .Include(vk => vk.InventoryKit)
                        .ThenInclude(ik => ik.KitLines)
                            .ThenInclude(kl => kl.Item)
                    .ToListAsync(cancellationToken);

                // Get all services for this visit
                var visitServices = await context.VisitServices
                    .Where(vs => vs.visitId == request.VisitId)
                    .Include(vs => vs.service)
                    .Include(vs => vs.sessions)
                    .ToListAsync(cancellationToken);

                // Get all invoice lines for this visit to subtract billed quantities
                var invoiceLines = await context.SalesInvoiceLines
                    .Where(sil => context.SalesInvoices
                        .Where(si => si.VisitId == request.VisitId && !si.IsDeleted)
                        .Select(si => si.Id)
                        .Contains(sil.SalesInvoiceId))
                    .ToListAsync(cancellationToken);

                // Process Medications
                var medicationGroups = visitMedications.GroupBy(vm => new { vm.stockId, vm.name, vm.dosage });
                foreach (var medGroup in medicationGroups)
                {
                    var totalQty = medGroup.Sum(m => m.quantity);
                    var billedQty = invoiceLines
                        .Where(il => il.InventoryItemId == medGroup.First().stockId)
                        .Sum(il => (int)il.Quantity);

                    var availableQty = totalQty - billedQty;

                    if (availableQty > 0)
                    {
                        var unitPrice = medGroup.First().price;
                        response.Medications.Add(new UnbilledMedicationDto
                        {
                            ItemId = medGroup.First().stockId,
                            MedicationName = medGroup.Key.name,
                            Dosage = medGroup.Key.dosage,
                            AvailableQuantity = availableQty,
                            UnitPrice = unitPrice,
                            TotalPrice = availableQty * unitPrice,
                            StockId = medGroup.First().stockId
                        });
                    }
                }

                // Process Kits
                var kitGroups = visitKits.GroupBy(vk => vk.KitId);
                foreach (var kitGroup in kitGroups)
                {
                    var totalQty = kitGroup.Count();
                    var billedQty = invoiceLines
                        .Where(il => il.KitId == kitGroup.Key)
                        .Sum(il => (int)il.Quantity);

                    var availableQty = totalQty - billedQty;

                    if (availableQty > 0)
                    {
                        var kit = kitGroup.First().InventoryKit;
                        
                        // Calculate kit price by summing the cost of all items in the kit
                        // Formula: Sum(Item.UnitCost * KitLine.Quantity) for all items in the kit
                        decimal kitPrice = 0;
                        if (!kit.IsFreeForPatient && kit?.KitLines != null && kit.KitLines.Any())
                        {
                            kitPrice = kit.KitLines.Sum(kl => (kl.Item?.UnitCost ?? 0) * kl.Quantity);
                        }
                        
                        response.Kits.Add(new UnbilledKitDto
                        {
                            KitId = kitGroup.Key,
                            KitName = kit?.KitName ?? "Unknown Kit",
                            AvailableQuantity = availableQty,
                            UnitPrice = kitPrice,
                            TotalPrice = availableQty * kitPrice
                        });
                    }
                }

                // Process Services
                var serviceGroups = visitServices.GroupBy(vs => new { vs.serviceId, vs.service?.Name });
                foreach (var serviceGroup in serviceGroups)
                {
                    var totalSessions = serviceGroup.Sum(s => s.sessions.Count);
                    var billedSessions = invoiceLines
                        .Where(il => il.ServiceId == serviceGroup.Key.serviceId)
                        .Sum(il => (int)il.Quantity);

                    var availableSessions = totalSessions - billedSessions;

                    if (availableSessions > 0)
                    {
                        var pricePerSession = serviceGroup.First().pricePerSession;
                        response.Services.Add(new UnbilledServiceDto
                        {
                            ServiceId = serviceGroup.Key.serviceId,
                            ServiceName = serviceGroup.Key.Name ?? "Unknown Service",
                            AvailableSessionCount = availableSessions,
                            PricePerSession = pricePerSession,
                            TotalPrice = availableSessions * pricePerSession
                        });
                    }
                }

                // Calculate total unbilled amount
                response.TotalUnbilledAmount = 
                    response.Medications.Sum(m => m.TotalPrice) +
                    response.Kits.Sum(k => k.TotalPrice) +
                    response.Services.Sum(s => s.TotalPrice);

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving unbilled visit items: {ex.Message}");
            }
        }
    }
}
