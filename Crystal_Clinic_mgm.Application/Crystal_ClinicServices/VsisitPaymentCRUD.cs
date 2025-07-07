using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.CrystalClinic.Visits;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    #region Record Payment

    public class RecordVisitPaymentCommand : IRequest<VisitDto>
    {
        public int VisitId { get; set; }
        public decimal AmountPaid { get; set; }
        public int CurrencyTypeId { get; set; } // Payment currency
        public decimal RefundAmountInAFN { get; set; } = 0;
        public decimal? ExchangeRateToAFN { get; set; }


    }

    public class RecordVisitPaymentCommandValidator : AbstractValidator<RecordVisitPaymentCommand>
    {
        public RecordVisitPaymentCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x.AmountPaid)
                .GreaterThan(0)
                .WithMessage("AmountPaid must be greater than 0.");
            RuleFor(x => x.CurrencyTypeId).GreaterThan(0).WithMessage("CurrencyTypeId must be greater than 0.");

        }
    }

    public class RecordVisitPaymentHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RecordVisitPaymentCommand, VisitDto>
    {
        public async Task<VisitDto> Handle(RecordVisitPaymentCommand request, CancellationToken cancellationToken)
        {
            var visit = await context.Visit
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Medications)
                .ThenInclude(vm => vm.stock).ThenInclude(s => s.item)
                .Include(v => v.Services).ThenInclude(vs => vs.service)
                .Include(v => v.Services).ThenInclude(x => x.CurrencyType)
                .Include(v => v.Payments).ThenInclude(x => x.CurrencyType)
                .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken) ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

            var afnCurrencyId = context.CurrencyType.Where(c => c.Code == "AFN").AsEnumerable().FirstOrDefault()!.ID;
            var exchangeRateToAFN = request.ExchangeRateToAFN ?? await context.GetExchangeRate(request.CurrencyTypeId, afnCurrencyId, cancellationToken);

            var amountInAFN = (request.AmountPaid * exchangeRateToAFN);
            if (request.RefundAmountInAFN > visit.paidAmount + amountInAFN)
            {
                throw new InvalidOperationException($"AmountPaid ({request.RefundAmountInAFN} AFN) exceeds paid balance ({visit.paidAmount} AFN) for visit.");
            }
            if ((amountInAFN - request.RefundAmountInAFN) > visit.remainingAmount)
            {
                throw new InvalidOperationException($"Payment amount ({amountInAFN - request.RefundAmountInAFN} AFN) exceeds remaining balance ({visit.remainingAmount} AFN).");
            }

            var payment = new VisitPayment
            {
                visitId = request.VisitId,
                amountPaid = request.AmountPaid,
                AmountInAFN = amountInAFN,
                paymentStatus = amountInAFN >= visit.remainingAmount ? PaymentStatus.Completed : PaymentStatus.Paid,
                paymentDate = DateTime.UtcNow,
                paymentType = PaymentType.General,
                CurrencyTypeId = request.CurrencyTypeId,
                ExchangeRateToAFN = exchangeRateToAFN,
                RefundAmountInAFN = request.RefundAmountInAFN,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.UtcNow
            };
            visit.paidAmount += amountInAFN - request.RefundAmountInAFN;
            visit.remainingAmount = visit.totalAmount - visit.paidAmount;
            if (visit.remainingAmount == 0)
            {
                visit.status = VisitStatus.COMPLETED;
            }

            context.VisitPayment.Add(payment);
            context.Visit.Update(visit);
            await context.SaveChangesAsync(cancellationToken);

            return CreateVisitDto(visit);
        }
        private VisitDto CreateVisitDto(Visit visit)
        {
            var currencyTotals = new Dictionary<string, decimal>();
            currencyTotals["AFN"] = visit.Medications.Where(m => !m.IsDeleted).Sum(m => m.quantity * m.price);
            var serviceTotals = visit.Services
                .GroupBy(s => s.CurrencyType!.Code)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.totalPrice));

            foreach (var kvp in serviceTotals)
            {
                currencyTotals[kvp.Key] = kvp.Value;
            }

            return new VisitDto
            {
                VisitId = visit.visitId,
                PatientId = visit.patientId,
                PatientName = visit.Patient?.name ?? "",
                DoctorId = visit.doctorId,
                DoctorName = visit.Doctor != null ? $"{visit.Doctor.firstName} {visit.Doctor.lastName}" : null,
                VisitDate = visit.visitDate,
                Status = visit.status,
                TotalAmount = visit.totalAmount,
                PaidAmount = visit.paidAmount,
                RemainingAmount = visit.remainingAmount,
                Medications = visit.Medications.Select(m => new VisitMedicationDto
                {
                    MedicationId = m.medicationId,
                    Name = m.name,
                    Dosage = m.dosage,
                    Quantity = m.quantity,
                    Price = m.price,
                    BatchNumber = m.stock?.batchNumber ?? ""
                }).ToList(),
                Services = visit.Services.Select(s => new VisitServiceDto
                {
                    VisitServiceId = s.visitServiceId,
                    ServiceName = s.service?.Name ?? "",
                    TotalSessions = s.totalSessions,
                    CompletedSessions = s.completedSessions,
                    PricePerSession = s.pricePerSession,
                    TotalPrice = s.totalPrice,
                    CurrencyTypeId = s.CurrencyTypeId,
                    CurrencyCode = s.CurrencyType?.Code ?? ""
                }).ToList(),
                Payments = visit.Payments.Select(p => new VisitPaymentDto
                {
                    visitPaymentId = p.visitPaymentId,
                    visitId = p.visitId,
                    amountPaid = p.amountPaid,
                    paymentStatus = p.paymentStatus,
                    paymentDate = p.paymentDate,
                    paymentType = p.paymentType,
                    CurrencyTypeId = p.CurrencyTypeId ?? 0,
                    CurrencyCode = p.CurrencyType?.Code ?? "",
                    AmountInAFN = p.AmountInAFN,
                    ExchangeRateToAFN = p.ExchangeRateToAFN,
                }).ToList(),
                CurrencyTotals = currencyTotals
            };
        }
    }


    #endregion

    #region Pay for Medication

    public class PayVisitMedicationCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public decimal AmountPaid { get; set; }
        public int CurrencyTypeId { get; set; }
        public decimal RefundAmountInAFN { get; set; } = 0;
        public decimal? ExchangeRateToAFN { get; set; }


    }

    public class PayVisitMedicationCommandValidator : AbstractValidator<PayVisitMedicationCommand>
    {
        public PayVisitMedicationCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");


            RuleFor(x => x.AmountPaid)
                .GreaterThan(0)
                .WithMessage("AmountPaid must be greater than 0.");
            RuleFor(x => x.CurrencyTypeId).GreaterThan(0).WithMessage("CurrencyTypeId must be greater than 0.");

        }
    }

    public class PayVisitMedicationHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<PayVisitMedicationCommand, bool>
    {
        public async Task<bool> Handle(PayVisitMedicationCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var visit = await context.Visit
                    .Include(v => v.Medications)
                    .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

                var currency = await context.CurrencyType
                    .FirstOrDefaultAsync(c => c.ID == request.CurrencyTypeId, cancellationToken)
                    ?? throw new KeyNotFoundException($"CurrencyType with ID {request.CurrencyTypeId} not found.");

                var afnCurrencyId = (await context.CurrencyType.FirstAsync(c => c.Code == "AFN", cancellationToken)).ID;
                var exchangeRateToAFN = request.ExchangeRateToAFN ?? await context.GetExchangeRate(request.CurrencyTypeId, afnCurrencyId, cancellationToken);

                var medicationTotal = visit.Medications
                    .Where(m => !m.IsDeleted).Sum(x => x.quantity * x.price); // In AFN

                var paidForMedication = await context.VisitPayment
                    .Where(p => p.visitId == request.VisitId && p.paymentType == PaymentType.Medication && !p.IsDeleted)
                    .SumAsync(p => p.AmountInAFN, cancellationToken);
                var medicationRemaining = medicationTotal - paidForMedication;

                var amountInAFN = request.AmountPaid * exchangeRateToAFN;
                if (request.RefundAmountInAFN > visit.paidAmount + amountInAFN)
                {
                    throw new InvalidOperationException($"AmountPaid ({request.RefundAmountInAFN} AFN) exceeds paid balance ({visit.paidAmount} AFN) for visit.");
                }
                if ((amountInAFN - request.RefundAmountInAFN) > medicationRemaining)
                {
                    throw new InvalidOperationException($"AmountPaid ({amountInAFN} AFN) exceeds remaining balance ({medicationRemaining} AFN) for medications.");
                }

                var payment = new VisitPayment
                {
                    visitId = request.VisitId,
                    amountPaid = request.AmountPaid,
                    AmountInAFN = amountInAFN,
                    paymentType = PaymentType.Medication,
                    paymentStatus = amountInAFN >= medicationRemaining ? PaymentStatus.Completed : PaymentStatus.Paid,
                    paymentDate = DateTime.UtcNow,
                    CurrencyTypeId = request.CurrencyTypeId,
                    ExchangeRateToAFN = exchangeRateToAFN,
                    RefundAmountInAFN = request.RefundAmountInAFN,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                visit.paidAmount += amountInAFN - request.RefundAmountInAFN;
                visit.remainingAmount = visit.totalAmount - visit.paidAmount;
                if (visit.remainingAmount == 0)
                {
                    visit.status = VisitStatus.COMPLETED;
                }

                context.VisitPayment.Add(payment);
                context.Visit.Update(visit);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Generate Bill
    public class GenerateBillQuery : IRequest<GenerateBillDto>
    {
        public int VisitId { get; set; }
    }

    public class GenerateBillQueryValidator : AbstractValidator<GenerateBillQuery>
    {
        public GenerateBillQueryValidator()
        {
            RuleFor(x => x.VisitId).GreaterThan(0).WithMessage("VisitId must be greater than 0.");
        }
    }

    public class GenerateBillHandler(ERP_DbContext context) : IRequestHandler<GenerateBillQuery, GenerateBillDto>
    {
        public async Task<GenerateBillDto> Handle(GenerateBillQuery request, CancellationToken cancellationToken)
        {
            var visit = await context.Visit
                .Include(v => v.Patient)
                .Include(v => v.Medications).ThenInclude(vm => vm.stock).ThenInclude(s => s.item)
                .Include(v => v.Services).ThenInclude(vs => vs.service)
                .Include(v => v.Services).ThenInclude(vs => vs.CurrencyType)
                .Include(v => v.Services).ThenInclude(vs => vs.sessions)
                .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

            var afnCurrencyId = (await context.CurrencyType.FirstAsync(c => c.Code == "AFN", cancellationToken)).ID;
            var currencyTotals = new Dictionary<string, decimal>();
            currencyTotals["AFN"] = visit.Medications.Where(m => !m.IsDeleted).Sum(m => m.quantity * m.price);

            var serviceBills = await context.VisitServices
                .Where(vs => vs.visitId == visit.visitId)
                .Select(vs => new VisitServiceBillDto
                {
                    VisitServiceId = vs.visitServiceId,
                    ServiceName = vs.service!.Name,
                    ImplementedSessions = vs.sessions.Count(),
                    PricePerSession = vs.pricePerSession,
                    TotalImplementedPrice = vs.sessions.Count() * vs.pricePerSession,
                    TotalImplementedPriceInAFN = vs.sessions.Sum(ss => ss.PriceInAFN),
                    CurrencyCode = vs.CurrencyType!.Code
                })
                .ToListAsync(cancellationToken);

            foreach (var service in serviceBills)
            {
                currencyTotals[service.CurrencyCode] = currencyTotals.GetValueOrDefault(service.CurrencyCode) + service.TotalImplementedPrice;
            }

            return new GenerateBillDto
            {
                VisitId = visit.visitId,
                PatientName = visit.Patient?.name ?? "",
                TotalAmount = visit.totalAmount,
                PaidAmount = visit.paidAmount,
                RemainingAmount = visit.remainingAmount,
                Medications = visit.Medications.Select(m => new VisitMedicationDto
                {
                    MedicationId = m.medicationId,
                    Name = m.stock?.item?.Name ?? "",
                    Dosage = m.dosage,
                    Quantity = m.quantity,
                    Price = m.price,
                    BatchNumber = m.stock?.batchNumber ?? ""
                }).ToList(),
                Services = serviceBills,
                CurrencyTotals = currencyTotals
            };
        }
    }

    #endregion

    #region  DTO
    public class GenerateBillDto
    {
        public int VisitId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } // In AFN
        public decimal PaidAmount { get; set; } // In AFN
        public decimal RemainingAmount { get; set; } // In AFN
        public List<VisitMedicationDto> Medications { get; set; } = new();
        public List<VisitServiceBillDto> Services { get; set; } = new();
        public Dictionary<string, decimal> CurrencyTotals { get; set; } = new();
    }
    public class VisitServiceBillDto
    {
        public int VisitServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int ImplementedSessions { get; set; }
        public decimal PricePerSession { get; set; }
        public decimal TotalImplementedPrice { get; set; } // In service currency
        public decimal TotalImplementedPriceInAFN { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
    }
    #endregion
}
