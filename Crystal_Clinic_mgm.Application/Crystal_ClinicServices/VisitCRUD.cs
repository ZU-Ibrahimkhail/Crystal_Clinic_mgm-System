using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.CrystalClinic.Visits
{
    #region Create Visit

    public class CreateVisitCommand : IRequest<JsonResult>
    {
        public int? PatientId { get; set; } // Nullable to allow new patient creation
        public string? PatientName { get; set; } // Required if PatientId is null
        public string? PatientContactInfo { get; set; } // Required if PatientId is null
        public string? PatientEmail { get; set; } // Optional
        public int? DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitStatus Status { get; set; } = VisitStatus.SCHEDULED;
    }

    public class CreateVisitCommandValidator : AbstractValidator<CreateVisitCommand>
    {
        public CreateVisitCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.PatientName) || string.IsNullOrWhiteSpace(x.PatientContactInfo))
                .WithMessage("PatientId is required if patient details are not provided.");

            RuleFor(x => x.PatientName)
                .NotEmpty()
                .When(x => !x.PatientId.HasValue)
                .WithMessage("PatientName is required when PatientId is not provided.");

            RuleFor(x => x.PatientContactInfo)
                .NotEmpty()
                .When(x => !x.PatientId.HasValue)
                .WithMessage("PatientContactInfo is required when PatientId is not provided.");

            RuleFor(x => x.PatientEmail)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.PatientEmail))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.VisitDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("VisitDate must be in the present or future.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid VisitStatus.");
        }
    }

    public class CreateVisitHandler(ERP_DbContext context, ILoggedInUser loggedInUser, IMessage message) : IRequestHandler<CreateVisitCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(CreateVisitCommand request, CancellationToken cancellationToken)
        {
            int patientId;
            var validator = new CreateVisitCommandValidator().Validate(request).Errors;

            if (validator.Count > 0) return message.CheckCCValidationError(validator);

            if (request.PatientId.HasValue)
            {
                var patient = await context.Patient.FirstOrDefaultAsync(p => !p.IsDeleted && p.patientId == request.PatientId.Value, cancellationToken) ?? throw new KeyNotFoundException($"Patient with ID {request.PatientId} not found.");
                patientId = request.PatientId.Value;
            }
            else
            {
                var newPatient = new Patient
                {
                    name = request.PatientName!,
                    contactInfo = request.PatientContactInfo!,
                    email = request.PatientEmail,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };
                context.Patient.Add(newPatient);
                await context.SaveChangesAsync(cancellationToken);
                patientId = newPatient.patientId;
            }

            if (request.DoctorId.HasValue)
            {
                var doctor = await context.Doctor.FirstOrDefaultAsync(d => !d.IsDeleted && d.doctorId == request.DoctorId.Value, cancellationToken);
                if (doctor == null || !doctor.isAvailable)
                {
                    throw new InvalidOperationException("Doctor not found or unavailable.");
                }

                // Check for scheduling conflicts
                var conflictingVisit = await context.Visit
                    .AnyAsync(v => !v.IsDeleted && v.doctorId == request.DoctorId && v.visitDate == request.VisitDate && v.status != VisitStatus.COMPLETED, cancellationToken);
                if (conflictingVisit)
                {
                    throw new InvalidOperationException("Doctor has a conflicting visit at this time.");
                }
            }

            var visit = new Visit
            {
                patientId = patientId,
                doctorId = request.DoctorId,
                visitDate = request.VisitDate,
                status = request.Status,
                totalAmount = 0,
                paidAmount = 0,
                remainingAmount = 0,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.UtcNow
            };

            context.Visit.Add(visit);
            await context.SaveChangesAsync(cancellationToken);

            return new JsonResult(visit);
        }
    }

    #endregion

    #region Update Visit

    public class UpdateVisitCommand : IRequest<JsonResult>
    {
        public int VisitId { get; set; }
        public int? DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitStatus Status { get; set; }
    }

    public class UpdateVisitCommandValidator : AbstractValidator<UpdateVisitCommand>
    {
        public UpdateVisitCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x.VisitDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("VisitDate must be in the present or future.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid VisitStatus.");
        }
    }

    public class UpdateVisitHandler(ERP_DbContext context, ILoggedInUser loggedInUser, IMessage message) : IRequestHandler<UpdateVisitCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(UpdateVisitCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateVisitCommandValidator().Validate(request).Errors;

            if (validator.Count > 0) return message.CheckCCValidationError(validator);

            var visit = await context.Visit.FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken) ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");
            if (request.DoctorId.HasValue)
            {
                var doctor = await context.Doctor.FirstOrDefaultAsync(d => !d.IsDeleted && d.doctorId == request.DoctorId.Value, cancellationToken);
                if (doctor == null || !doctor.isAvailable)
                {
                    throw new InvalidOperationException("Doctor not found or unavailable.");
                }

                // Check for scheduling conflicts
                var conflictingVisit = await context.Visit
                    .AnyAsync(v => !v.IsDeleted && v.visitId != request.VisitId && v.doctorId == request.DoctorId && v.visitDate == request.VisitDate && v.status != VisitStatus.COMPLETED, cancellationToken);
                if (conflictingVisit)
                {
                    throw new InvalidOperationException("Doctor has a conflicting visit at this time.");
                }
            }

            visit.doctorId = request.DoctorId;
            visit.visitDate = request.VisitDate;
            visit.status = request.Status;
            visit.ModifiedBy = loggedInUser.Id;
            visit.ModifiedOn = DateTime.UtcNow;

            context.Visit.Update(visit);
            await context.SaveChangesAsync(cancellationToken);

            return new JsonResult(visit);
        }
    }

    #endregion

    #region Delete Visit

    public class DeleteVisitCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
    }

    public class DeleteVisitHandler : IRequestHandler<DeleteVisitCommand, bool>
    {
        private readonly ERP_DbContext _context;
        private readonly ILoggedInUser _loggedInUser;

        public DeleteVisitHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        {
            _context = context;
            _loggedInUser = loggedInUser;
        }

        public async Task<bool> Handle(DeleteVisitCommand request, CancellationToken cancellationToken)
        {
            var visit = await _context.Visit.FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken);
            if (visit == null)
            {
                return false;
            }

            visit.IsDeleted = true;
            visit.ModifiedBy = _loggedInUser.Id;
            visit.ModifiedOn = DateTime.UtcNow;

            _context.Visit.Update(visit);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

    #endregion

    #region Get Visit Details

    public class GetVisitDetailsQuery : IRequest<VisitDto>
    {
        public int VisitId { get; set; }
    }

    public class GetVisitDetailsHandler : IRequestHandler<GetVisitDetailsQuery, VisitDto>
    {
        private readonly ERP_DbContext _context;

        public GetVisitDetailsHandler(ERP_DbContext context)
        {
            _context = context;
        }

        public async Task<VisitDto> Handle(GetVisitDetailsQuery request, CancellationToken cancellationToken)
        {
            var visit = await _context.Visit
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Payments).Include(x => x.Services)
                .Include(v => v.Medications)
                .ThenInclude(vm => vm.stock).ThenInclude(s => s.item)
                .Include(v => v.Services)
                .ThenInclude(vs => vs.service)
                .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken);

            if (visit == null)
            {
                throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");
            }

            return new VisitDto
            {
                VisitId = visit.visitId,
                PatientId = visit.patientId,
                PatientName = visit.Patient.name,
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
                    Name = m.stock.item.Name,
                    Dosage = m.dosage,
                    Quantity = m.quantity,
                    Price = m.price,
                    BatchNumber = m.stock.batchNumber
                }).ToList(),
                Services = visit.Services.Select(s => new VisitServiceDto
                {
                    VisitServiceId = s.visitServiceId,
                    ServiceName = s.service.Name,
                    TotalSessions = s.totalSessions,
                    CompletedSessions = s.completedSessions,
                    PricePerSession = s.pricePerSession,
                    TotalPrice = s.totalPrice,
                    PaidAmount = s.paidAmount,
                    RemainAmount = s.remainAmount
                }).ToList(),

                Payments = visit.Payments.Where(x=>!x.IsDeleted).Select(s => new VisitPaymentDto
                {
                    visitPaymentId = s.visitPaymentId,
                    visitId = s.visitId,
                    serviceId = s.serviceId,
                    serviceName = s.service?.Name,
                    sessionNumber = s.sessionNumber,
                    amountPaid = s.amountPaid,
                    paymentStatus = s.paymentStatus,
                    paymentType = s.paymentType,
                    paymentDate = s.paymentDate,

                }).ToList()
            };
        }
    }

    #endregion

    #region Get Visit List

    public class GetVisitListQuery : IRequest<VisitListDto>
    {
        public string? Search { get; set; }
        public int? PatientId { get; set; }
        public int? LastVisitId { get; set; }
        public int PageSize { get; set; } = 20;
    }

    public class GetVisitListQueryValidator : AbstractValidator<GetVisitListQuery>
    {
        public GetVisitListQueryValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.LastVisitId)
                .GreaterThan(0)
                .When(x => x.LastVisitId.HasValue)
                .WithMessage("LastVisitId must be greater than 0.");

            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .When(x => x.PatientId.HasValue)
                .WithMessage("PatientId must be greater than 0.");
        }
    }

    public class GetVisitListHandler : IRequestHandler<GetVisitListQuery, VisitListDto>
    {
        private readonly ERP_DbContext _context;

        public GetVisitListHandler(ERP_DbContext context)
        {
            _context = context;
        }

        public async Task<VisitListDto> Handle(GetVisitListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Visit
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Where(v => !v.IsDeleted)
                .AsQueryable();

            if (request.PatientId.HasValue)
            {
                query = query.Where(v => v.patientId == request.PatientId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(v => v.Patient.name.Contains(request.Search) ||
                                         (v.Doctor != null && (v.Doctor.firstName.Contains(request.Search) || v.Doctor.lastName.Contains(request.Search))));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            if (request.LastVisitId.HasValue)
            {
                query = query.Where(v => v.visitId > request.LastVisitId.Value);
            }

            query = query.OrderBy(v => v.visitId).Take(request.PageSize);

            var visits = await query
                .Select(v => new VisitDto
                {
                    VisitId = v.visitId,
                    PatientId = v.patientId,
                    PatientName = v.Patient.name,
                    DoctorId = v.doctorId,
                    DoctorName = v.Doctor != null ? $"{v.Doctor.firstName} {v.Doctor.lastName}" : null,
                    VisitDate = v.visitDate,
                    Status = v.status,
                    TotalAmount = v.totalAmount,
                    PaidAmount = v.paidAmount,
                    RemainingAmount = v.remainingAmount
                })
                .ToListAsync(cancellationToken);

            return new VisitListDto
            {
                Data = visits,
                TotalCount = totalCount
            };
        }
    }

    #endregion

    #region Add Medications to Visit

    public class AddVisitMedicationCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public List<MedicationDetails> Medications { get; set; } = new();
    }

    public class MedicationDetails
    {
        public int StockId { get; set; }
        public string Dosage { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
    }

    public class AddVisitMedicationCommandValidator : AbstractValidator<AddVisitMedicationCommand>
    {
        public AddVisitMedicationCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x.Medications)
                .NotEmpty()
                .WithMessage("At least one medication must be provided.");

            RuleForEach(x => x.Medications).SetValidator(new MedicationDetailsValidator());
        }
    }

    public class MedicationDetailsValidator : AbstractValidator<MedicationDetails>
    {
        public MedicationDetailsValidator()
        {
            RuleFor(x => x.StockId)
                .GreaterThan(0)
                .WithMessage("StockId must be greater than 0.");

            RuleFor(x => x.Dosage)
                .NotEmpty()
                .WithMessage("Dosage is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be non-negative.");

            RuleFor(x => x.BatchNumber)
                .NotEmpty()
                .WithMessage("BatchNumber is required.");
        }
    }

    public class AddVisitMedicationHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<AddVisitMedicationCommand, bool>
    {
        public async Task<bool> Handle(AddVisitMedicationCommand request, CancellationToken cancellationToken)
        {
            var visit = await context.Visit.FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken);
            if (visit == null)
            {
                throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");
            }

            var stockIds = request.Medications.Select(m => m.StockId).ToList();
            var stocks = await context.Stocks
                .Include(s => s.item)
                .Where(s => stockIds.Contains(s.stockId))
                .ToDictionaryAsync(s => s.stockId, s => s, cancellationToken);

            foreach (var med in request.Medications)
            {
                if (!stocks.TryGetValue(med.StockId, out var stock) || stock.quantity < med.Quantity)
                {
                    throw new InvalidOperationException($"Stock with ID {med.StockId} not found or insufficient quantity.");
                }

                if (stock.batchNumber != med.BatchNumber)
                {
                    throw new InvalidOperationException($"Batch number does not match for stock ID {med.StockId}.");
                }

                var medication = new VisitMedication
                {
                    visitId = request.VisitId,
                    stockId = med.StockId,
                    name = stock.item.Name, // Fixed: Use stock.item.Name
                    dosage = med.Dosage,
                    quantity = med.Quantity,
                    price = med.Price,
                    stock = stock,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                stock.quantity -= med.Quantity;
                if (stock.quantity < stock.item.ReorderLevel)
                {
                    // TODO: Trigger reorder alert
                }

                visit.totalAmount += med.Price * med.Quantity;

                context.VisitMedication.Add(medication);
                context.Stocks.Update(stock);
            }

            visit.remainingAmount = visit.totalAmount - visit.paidAmount;
            context.Visit.Update(visit);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

    #endregion

    #region Add Services to Visit

    public class AddVisitServiceCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public List<ServiceDetails> Services { get; set; } = new();
    }

    public class ServiceDetails
    {
        public int ServiceId { get; set; }
        public int TotalSessions { get; set; }
        public decimal PricePerSession { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class AddVisitServiceCommandValidator : AbstractValidator<AddVisitServiceCommand>
    {
        public AddVisitServiceCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x.Services)
                .NotEmpty()
                .WithMessage("At least one service must be provided.");

            RuleForEach(x => x.Services).SetValidator(new ServiceDetailsValidator());
        }
    }

    public class ServiceDetailsValidator : AbstractValidator<ServiceDetails>
    {
        public ServiceDetailsValidator()
        {
            RuleFor(x => x.ServiceId)
                .GreaterThan(0)
                .WithMessage("ServiceId must be greater than 0.");

            RuleFor(x => x.TotalSessions)
                .GreaterThan(0)
                .WithMessage("TotalSessions must be greater than 0.");

            RuleFor(x => x.PricePerSession)
                .GreaterThanOrEqualTo(0)
                .WithMessage("PricePerSession must be non-negative.");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("StartDate must be in the present or future.");
        }
    }

    public class AddVisitServiceHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<AddVisitServiceCommand, bool>
    {
        public async Task<bool> Handle(AddVisitServiceCommand request, CancellationToken cancellationToken)
        {
            var visit = await context.Visit.FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken) ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");
            var serviceIds = request.Services.Select(s => s.ServiceId).ToList();
            var services = await context.Services
                .Where(s => serviceIds.Contains(s.ServiceId))
                .ToDictionaryAsync(s => s.ServiceId, s => s, cancellationToken);

            foreach (var svc in request.Services)
            {
                if (!services.TryGetValue(svc.ServiceId, out var service))
                {
                    throw new KeyNotFoundException($"Service with ID {svc.ServiceId} not found.");
                }

                var visitService = new VisitServices
                {
                    visitId = request.VisitId,
                    serviceId = svc.ServiceId,
                    service = service,
                    startDate = svc.StartDate,
                    totalSessions = svc.TotalSessions,
                    completedSessions = 0,
                    pricePerSession = svc.PricePerSession,
                    totalPrice = svc.PricePerSession * svc.TotalSessions,
                    paidAmount = 0,
                    remainAmount = svc.PricePerSession * svc.TotalSessions,
                    paymentStatus = PaymentStatus.Pending,
                    //CreatedBy = _loggedInUser.Id,
                    //CreatedOn = DateTime.UtcNow
                };

                visit.totalAmount += visitService.totalPrice;
                context.VisitServices.Add(visitService);
            }

            visit.remainingAmount = visit.totalAmount - visit.paidAmount;
            visit.ModifiedOn = DateTime.Now;
            visit.ModifiedBy = loggedInUser.Id;
            context.Visit.Update(visit);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
    #endregion

    #region Record Payment

    public class RecordVisitPaymentCommand : IRequest<VisitDto>
    {
        public int VisitId { get; set; }
        public decimal AmountPaid { get; set; }
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
                .Include(v => v.Services)
                .ThenInclude(vs => vs.service)
                .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken);
            if (visit == null)
            {
                throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");
            }

            if (request.AmountPaid > visit.remainingAmount)
            {
                throw new InvalidOperationException("Payment amount exceeds remaining balance.");
            }

            var payment = new VisitPayment
            {
                visitId = request.VisitId,
                amountPaid = request.AmountPaid,
                paymentStatus = request.AmountPaid == visit.remainingAmount ? PaymentStatus.Completed : PaymentStatus.Paid,
                paymentDate = DateTime.UtcNow,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.UtcNow
            };

            visit.paidAmount += request.AmountPaid;
            visit.remainingAmount = visit.totalAmount - visit.paidAmount;

            if (visit.remainingAmount == 0)
            {
                visit.status = VisitStatus.COMPLETED;
            }

            context.VisitPayment.Add(payment);
            context.Visit.Update(visit);
            await context.SaveChangesAsync(cancellationToken);

            return new VisitDto
            {
                VisitId = visit.visitId,
                PatientId = visit.patientId,
                PatientName = visit.Patient.name,
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
                    Name = m.stock.item.Name,
                    Dosage = m.dosage,
                    Quantity = m.quantity,
                    Price = m.price,
                    BatchNumber = m.stock.batchNumber
                }).ToList(),
                Services = visit.Services.Select(s => new VisitServiceDto
                {
                    VisitServiceId = s.visitServiceId,
                    ServiceName = s.service.Name,
                    TotalSessions = s.totalSessions,
                    CompletedSessions = s.completedSessions,
                    PricePerSession = s.pricePerSession,
                    TotalPrice = s.totalPrice,
                    PaidAmount = s.paidAmount,
                    RemainAmount = s.remainAmount
                }).ToList()
            };
        }
    }

    #endregion

    #region Pay for Service

    public class PayVisitServiceCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public int? VisitServiceId { get; set; } // Optional: Specify for single service or session
        public int? SessionCount { get; set; } // Optional: Set to 1 for single session payment
        public decimal AmountPaid { get; set; }
    }

    public class PayVisitServiceCommandValidator : AbstractValidator<PayVisitServiceCommand>
    {
        public PayVisitServiceCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x.VisitServiceId)
                .GreaterThan(0)
                .When(x => x.VisitServiceId.HasValue)
                .WithMessage("VisitServiceId must be greater than 0.");

            RuleFor(x => x.SessionCount)
                .Equal(1)
                .When(x => x.SessionCount.HasValue)
                .WithMessage("SessionCount must be 1 for single session payment.");

            RuleFor(x => x.AmountPaid)
                .GreaterThan(0)
                .WithMessage("AmountPaid must be greater than 0.");

            RuleFor(x => x)
                .Must(x => !(x.SessionCount.HasValue && !x.VisitServiceId.HasValue))
                .WithMessage("VisitServiceId is required when SessionCount is specified.");
        }
    }

    public class PayVisitServiceHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<PayVisitServiceCommand, bool>
    {
        public async Task<bool> Handle(PayVisitServiceCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var visit = await context.Visit
                    .Include(v => v.Services)
                    .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

                if (request.SessionCount.HasValue && request.VisitServiceId.HasValue)
                {
                    // Pay for a single session
                    var visitService = visit.Services
                        .FirstOrDefault(s => s.visitServiceId == request.VisitServiceId.Value)
                        ?? throw new KeyNotFoundException($"VisitService with ID {request.VisitServiceId} not found.");

                    if (visitService.completedSessions >= visitService.totalSessions)
                    {
                        throw new InvalidOperationException("All sessions for this service are already completed.");
                    }

                    if (request.AmountPaid != visitService.pricePerSession)
                    {
                        throw new InvalidOperationException($"AmountPaid ({request.AmountPaid}) must equal the price per session ({visitService.pricePerSession}).");
                    }

                    var payment = new VisitPayment
                    {
                        visitId = request.VisitId,
                        amountPaid = request.AmountPaid,
                        paymentType = PaymentType.Service,
                        paymentStatus = PaymentStatus.Paid,
                        paymentDate = DateTime.UtcNow,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    };

                    visitService.paidAmount += request.AmountPaid;
                    visitService.remainAmount -= request.AmountPaid;
                    visitService.completedSessions += 1;
                    visitService.paymentStatus = visitService.remainAmount == 0 ? PaymentStatus.Completed : PaymentStatus.Paid;

                    visit.paidAmount += request.AmountPaid;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;
                    if (visit.remainingAmount == 0)
                    {
                        visit.status = VisitStatus.COMPLETED;
                    }

                    context.VisitPayment.Add(payment);
                    context.VisitServices.Update(visitService);
                    context.Visit.Update(visit);
                }
                else if (request.VisitServiceId.HasValue)
                {
                    // Pay for a single service
                    var visitService = visit.Services
                        .FirstOrDefault(s => s.visitServiceId == request.VisitServiceId.Value)
                        ?? throw new KeyNotFoundException($"VisitService with ID {request.VisitServiceId} not found.");

                    if (request.AmountPaid > visitService.remainAmount)
                    {
                        throw new InvalidOperationException($"AmountPaid ({request.AmountPaid}) exceeds remaining balance ({visitService.remainAmount}).");
                    }

                    var payment = new VisitPayment
                    {
                        visitId = request.VisitId,
                        amountPaid = request.AmountPaid,
                        paymentType = PaymentType.Service,
                        paymentStatus = request.AmountPaid == visitService.remainAmount ? PaymentStatus.Completed : PaymentStatus.Paid,
                        paymentDate = DateTime.UtcNow,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    };

                    visitService.paidAmount += request.AmountPaid;
                    visitService.remainAmount -= request.AmountPaid;
                    visitService.paymentStatus = visitService.remainAmount == 0 ? PaymentStatus.Completed : PaymentStatus.Paid;

                    visit.paidAmount += request.AmountPaid;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;
                    if (visit.remainingAmount == 0)
                    {
                        visit.status = VisitStatus.COMPLETED;
                    }

                    context.VisitPayment.Add(payment);
                    context.VisitServices.Update(visitService);
                    context.Visit.Update(visit);
                }
                else
                {
                    // Pay for all services
                    var services = visit.Services
                        .Where(s => s.remainAmount > 0)
                        .OrderBy(s => s.startDate)
                        .ThenBy(s => s.visitServiceId)
                        .ToList();

                    if (!services.Any())
                    {
                        throw new InvalidOperationException("No services with remaining balance found for this visit.");
                    }

                    var totalRemaining = services.Sum(s => s.remainAmount);
                    if (request.AmountPaid > totalRemaining)
                    {
                        throw new InvalidOperationException($"AmountPaid ({request.AmountPaid}) exceeds total remaining balance ({totalRemaining}).");
                    }

                    decimal remainingPayment = request.AmountPaid;
                    foreach (var service in services)
                    {
                        if (remainingPayment <= 0)
                            break;

                        var paymentForService = Math.Min(remainingPayment, service.remainAmount);
                        if (paymentForService <= 0)
                            continue;

                        var payment = new VisitPayment
                        {
                            visitId = request.VisitId,
                            amountPaid = paymentForService,
                            paymentType = PaymentType.Service,
                            paymentStatus = paymentForService == service.remainAmount ? PaymentStatus.Completed : PaymentStatus.Paid,
                            paymentDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        service.paidAmount += paymentForService;
                        service.remainAmount -= paymentForService;
                        service.paymentStatus = service.remainAmount == 0 ? PaymentStatus.Completed : PaymentStatus.Paid;

                        visit.paidAmount += paymentForService;
                        remainingPayment -= paymentForService;

                        context.VisitPayment.Add(payment);
                        context.VisitServices.Update(service);
                    }

                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;
                    if (visit.remainingAmount == 0)
                    {
                        visit.status = VisitStatus.COMPLETED;
                    }

                    context.Visit.Update(visit);
                }

                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Pay for Medication

    public class PayVisitMedicationCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public decimal AmountPaid { get; set; }
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

                var medicationTotal = visit.Medications
                    .Where(m => !m.IsDeleted).Sum(x => x.quantity * x.price);

                var paidForMedication = await context.VisitPayment
                    .Where(p => p.visitId == request.VisitId && p.paymentType == PaymentType.Medication && !p.IsDeleted)
                    .SumAsync(p => p.amountPaid, cancellationToken);
                var medicationRemaining = medicationTotal - paidForMedication;

                if (request.AmountPaid > medicationRemaining)
                {
                    throw new InvalidOperationException("Payment amount exceeds remaining balance for the medication.");
                }

                var payment = new VisitPayment
                {
                    visitId = request.VisitId,
                    amountPaid = request.AmountPaid,
                    paymentType = PaymentType.Medication,
                    paymentStatus = request.AmountPaid == medicationRemaining ? PaymentStatus.Completed : PaymentStatus.Paid,
                    paymentDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                visit.paidAmount += request.AmountPaid;
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

    #region Change Visit Status

    public class ChangeVisitStatusCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public VisitStatus Status { get; set; }
    }

    public class ChangeVisitStatusCommandValidator : AbstractValidator<ChangeVisitStatusCommand>
    {
        public ChangeVisitStatusCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid VisitStatus.");
        }
    }

    public class ChangeVisitStatusHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ChangeVisitStatusCommand, bool>
    {
        public async Task<bool> Handle(ChangeVisitStatusCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var visit = await context.Visit
                    .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

                visit.status = request.Status;
                visit.ModifiedBy = loggedInUser.Id;
                visit.ModifiedOn = DateTime.UtcNow;

                context.Visit.Update(visit);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Postpone Visit

    public class PostponeVisitCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public DateTime NewVisitDate { get; set; }
    }

    public class PostponeVisitCommandValidator : AbstractValidator<PostponeVisitCommand>
    {
        public PostponeVisitCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x.NewVisitDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("NewVisitDate must be in the present or future.");
        }
    }

    public class PostponeVisitHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<PostponeVisitCommand, bool>
    {
        public async Task<bool> Handle(PostponeVisitCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var visit = await context.Visit
                    .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

                if (visit.doctorId.HasValue)
                {
                    var conflictingVisit = await context.Visit
                        .AnyAsync(v => !v.IsDeleted && v.visitId != request.VisitId && v.doctorId == visit.doctorId && v.visitDate == request.NewVisitDate && v.status != VisitStatus.COMPLETED, cancellationToken);
                    if (conflictingVisit)
                    {
                        throw new InvalidOperationException("Doctor has a conflicting visit at the new time.");
                    }
                }

                visit.visitDate = request.NewVisitDate;
                visit.ModifiedBy = loggedInUser.Id;
                visit.ModifiedOn = DateTime.UtcNow;

                context.Visit.Update(visit);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Remove Payment, Medication, or Service

    public class RemoveVisitItemCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public int? VisitPaymentId { get; set; }
        public int? MedicationId { get; set; }
        public int? VisitServiceId { get; set; }
    }

    public class RemoveVisitItemCommandValidator : AbstractValidator<RemoveVisitItemCommand>
    {
        public RemoveVisitItemCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x)
                .Must(x => x.VisitPaymentId.HasValue || x.MedicationId.HasValue || x.VisitServiceId.HasValue)
                .WithMessage("At least one of VisitPaymentId, MedicationId, or VisitServiceId must be provided.");

            RuleFor(x => x)
                .Must(x => new[] { x.VisitPaymentId, x.MedicationId, x.VisitServiceId }.Count(id => id.HasValue) == 1)
                .WithMessage("Only one of VisitPaymentId, MedicationId, or VisitServiceId can be provided at a time.");

            RuleFor(x => x.VisitPaymentId)
                .GreaterThan(0)
                .When(x => x.VisitPaymentId.HasValue)
                .WithMessage("VisitPaymentId must be greater than 0.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0)
                .When(x => x.MedicationId.HasValue)
                .WithMessage("MedicationId must be greater than 0.");

            RuleFor(x => x.VisitServiceId)
                .GreaterThan(0)
                .When(x => x.VisitServiceId.HasValue)
                .WithMessage("VisitServiceId must be greater than 0.");
        }
    }

    public class RemoveVisitItemHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RemoveVisitItemCommand, bool>
    {
        public async Task<bool> Handle(RemoveVisitItemCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var visit = await context.Visit
                    .Include(v => v.Medications)
                    .Include(v => v.Services)
                    .Include(v => v.Payments)
                    .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

                if (request.VisitPaymentId.HasValue)
                {
                    var payment = visit.Payments
                        .FirstOrDefault(p => p.visitPaymentId == request.VisitPaymentId.Value && !p.IsDeleted)
                        ?? throw new KeyNotFoundException($"Payment with ID {request.VisitPaymentId} not found.");

                    payment.IsDeleted = true;
                    payment.ModifiedBy = loggedInUser.Id;
                    payment.ModifiedOn = DateTime.UtcNow;

                    visit.paidAmount -= payment.amountPaid;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;

                    if (payment.paymentType == PaymentType.Service)
                    {
                        var service = visit.Services.FirstOrDefault(s => s.serviceId == payment.serviceId);
                        if (service != null)
                        {
                            service.paidAmount -= payment.amountPaid;
                            service.remainAmount += payment.amountPaid;
                            service.paymentStatus = service.remainAmount == service.totalPrice ? PaymentStatus.Pending : service.paymentStatus;
                            context.VisitServices.Update(service);
                        }
                    }

                    context.VisitPayment.Update(payment);
                }
                else if (request.MedicationId.HasValue)
                {
                    var medication = visit.Medications
                        .FirstOrDefault(m => m.medicationId == request.MedicationId.Value && !m.IsDeleted)
                        ?? throw new KeyNotFoundException($"Medication with ID {request.MedicationId} not found.");

                    medication.IsDeleted = true;
                    medication.ModifiedBy = loggedInUser.Id;
                    medication.ModifiedOn = DateTime.UtcNow;

                    var stock = await context.Stocks.FindAsync(medication.stockId, cancellationToken);
                    if (stock != null)
                    {
                        stock.quantity += medication.quantity;
                        context.Stocks.Update(stock);
                    }

                    visit.totalAmount -= medication.quantity * medication.price;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;

                    context.VisitMedication.Update(medication);
                }
                else if (request.VisitServiceId.HasValue)
                {
                    var service = visit.Services
                        .FirstOrDefault(s => s.visitServiceId == request.VisitServiceId.Value)
                        ?? throw new KeyNotFoundException($"Service with ID {request.VisitServiceId} not found.");

                    visit.totalAmount -= service.totalPrice;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;

                    context.VisitServices.Remove(service);

                }


                if (visit.remainingAmount > 0 && visit.status == VisitStatus.COMPLETED)
                {
                    visit.status = VisitStatus.PENDING;
                }

                context.Visit.Update(visit);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Update Payment, Medication, or Service

    public class UpdateVisitItemCommand : IRequest<bool>
    {
        public int VisitId { get; set; }
        public int? VisitPaymentId { get; set; }
        public int? MedicationId { get; set; }
        public int? VisitServiceId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string? Dosage { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string? BatchNumber { get; set; }
        public int? TotalSessions { get; set; }
        public decimal? PricePerSession { get; set; }
        public DateTime? StartDate { get; set; }
    }

    public class UpdateVisitItemCommandValidator : AbstractValidator<UpdateVisitItemCommand>
    {
        public UpdateVisitItemCommandValidator()
        {
            RuleFor(x => x.VisitId)
                .GreaterThan(0)
                .WithMessage("VisitId must be greater than 0.");

            RuleFor(x => x)
                .Must(x => x.VisitPaymentId.HasValue || x.MedicationId.HasValue || x.VisitServiceId.HasValue)
                .WithMessage("At least one of VisitPaymentId, MedicationId, or VisitServiceId must be provided.");

            RuleFor(x => x)
                .Must(x => new[] { x.VisitPaymentId, x.MedicationId, x.VisitServiceId }.Count(id => id.HasValue) == 1)
                .WithMessage("Only one of VisitPaymentId, MedicationId, or VisitServiceId can be provided at a time.");

            RuleFor(x => x.VisitPaymentId)
                .GreaterThan(0)
                .When(x => x.VisitPaymentId.HasValue)
                .WithMessage("VisitPaymentId must be greater than 0.");

            RuleFor(x => x.MedicationId)
                .GreaterThan(0)
                .When(x => x.MedicationId.HasValue)
                .WithMessage("MedicationId must be greater than 0.");

            RuleFor(x => x.VisitServiceId)
                .GreaterThan(0)
                .When(x => x.VisitServiceId.HasValue)
                .WithMessage("VisitServiceId must be greater than 0.");

            RuleFor(x => x.AmountPaid)
                .GreaterThan(0)
                .When(x => x.AmountPaid.HasValue)
                .WithMessage("AmountPaid must be greater than 0.");

            RuleFor(x => x.Dosage)
                .NotEmpty()
                .When(x => x.Dosage != null)
                .WithMessage("Dosage cannot be empty.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .When(x => x.Quantity.HasValue)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Price.HasValue)
                .WithMessage("Price must be non-negative.");

            RuleFor(x => x.BatchNumber)
                .NotEmpty()
                .When(x => x.BatchNumber != null)
                .WithMessage("BatchNumber cannot be empty.");

            RuleFor(x => x.TotalSessions)
                .GreaterThan(0)
                .When(x => x.TotalSessions.HasValue)
                .WithMessage("TotalSessions must be greater than 0.");

            RuleFor(x => x.PricePerSession)
                .GreaterThanOrEqualTo(0)
                .When(x => x.PricePerSession.HasValue)
                .WithMessage("PricePerSession must be non-negative.");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.StartDate.HasValue)
                .WithMessage("StartDate must be in the present or future.");
        }
    }

    public class UpdateVisitItemHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateVisitItemCommand, bool>
    {
        public async Task<bool> Handle(UpdateVisitItemCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var visit = await context.Visit
                    .Include(v => v.Medications)
                    .Include(v => v.Services)
                    .Include(v => v.Payments)
                    .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

                if (request.VisitPaymentId.HasValue)
                {
                    var payment = visit.Payments
                        .FirstOrDefault(p => p.visitPaymentId == request.VisitPaymentId.Value && !p.IsDeleted)
                        ?? throw new KeyNotFoundException($"Payment with ID {request.VisitPaymentId} not found.");

                    if (!request.AmountPaid.HasValue)
                    {
                        throw new InvalidOperationException("AmountPaid is required to update a payment.");
                    }

                    var oldAmount = payment.amountPaid;
                    payment.amountPaid = request.AmountPaid.Value;
                    payment.paymentStatus = payment.paymentType == PaymentType.Service
                        ? (visit.Services.FirstOrDefault(s => s.visitServiceId == request.VisitServiceId)?.remainAmount == 0 ? PaymentStatus.Completed : PaymentStatus.Paid)
                        : (visit.Payments.Where(p => p.paymentType == PaymentType.Medication && !p.IsDeleted).Sum(p => p.amountPaid) == visit.Medications.Where(m => !m.IsDeleted).Sum(m => m.quantity * m.price) ? PaymentStatus.Completed : PaymentStatus.Paid);
                    payment.ModifiedBy = loggedInUser.Id;
                    payment.ModifiedOn = DateTime.UtcNow;

                    visit.paidAmount = visit.paidAmount - oldAmount + request.AmountPaid.Value;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;

                    if (payment.paymentType == PaymentType.Service)
                    {
                        var service = visit.Services.FirstOrDefault(s => s.remainAmount > 0);
                        if (service != null)
                        {
                            service.paidAmount = service.paidAmount - oldAmount + request.AmountPaid.Value;
                            service.remainAmount = service.totalPrice - service.paidAmount;
                            service.paymentStatus = service.remainAmount == 0 ? PaymentStatus.Completed : PaymentStatus.Paid;
                            context.VisitServices.Update(service);
                        }
                    }

                    context.VisitPayment.Update(payment);
                }
                else if (request.MedicationId.HasValue)
                {
                    var medication = visit.Medications
                        .FirstOrDefault(m => m.medicationId == request.MedicationId.Value && !m.IsDeleted)
                        ?? throw new KeyNotFoundException($"Medication with ID {request.MedicationId} not found.");

                    var oldQuantity = medication.quantity;
                    var oldPrice = medication.price;

                    if (request.Dosage != null)
                        medication.dosage = request.Dosage;
                    if (request.Quantity.HasValue)
                    {
                        var stock = await context.Stocks.FindAsync(medication.stockId, cancellationToken);
                        if (stock != null)
                        {
                            stock.quantity += oldQuantity;
                            if (stock.quantity < request.Quantity.Value)
                            {
                                throw new InvalidOperationException($"Insufficient stock for medication ID {request.MedicationId}.");
                            }
                            stock.quantity -= request.Quantity.Value;
                            context.Stocks.Update(stock);
                        }
                        medication.quantity = request.Quantity.Value;
                    }
                    if (request.Price.HasValue)
                        medication.price = request.Price.Value;
                    if (request.BatchNumber != null)
                    {
                        var stock = await context.Stocks.FindAsync(medication.stockId, cancellationToken);
                        if (stock?.batchNumber != request.BatchNumber)
                        {
                            throw new InvalidOperationException($"Batch number does not match for stock ID {medication.stockId}.");
                        }
                        medication.stock.batchNumber = request.BatchNumber;
                    }

                    medication.ModifiedBy = loggedInUser.Id;
                    medication.ModifiedOn = DateTime.UtcNow;

                    visit.totalAmount = visit.totalAmount - (oldQuantity * oldPrice) + (medication.quantity * medication.price);
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;

                    context.VisitMedication.Update(medication);
                }
                else if (request.VisitServiceId.HasValue)
                {
                    var service = visit.Services
                        .FirstOrDefault(s => s.visitServiceId == request.VisitServiceId.Value)
                        ?? throw new KeyNotFoundException($"Service with ID {request.VisitServiceId} not found.");

                    var oldTotalPrice = service.totalPrice;

                    if (request.TotalSessions.HasValue)
                        service.totalSessions = request.TotalSessions.Value;
                    if (request.PricePerSession.HasValue)
                        service.pricePerSession = request.PricePerSession.Value;
                    if (request.StartDate.HasValue)
                        service.startDate = request.StartDate.Value;

                    service.totalPrice = service.pricePerSession * service.totalSessions;
                    service.remainAmount = service.totalPrice - service.paidAmount;
                    service.paymentStatus = service.remainAmount == 0 ? PaymentStatus.Completed : PaymentStatus.Paid;

                    visit.totalAmount = visit.totalAmount - oldTotalPrice + service.totalPrice;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;

                    context.VisitServices.Update(service);
                }

                if (visit.remainingAmount > 0 && visit.status == VisitStatus.COMPLETED)
                {
                    visit.status = VisitStatus.PENDING;
                }

                context.Visit.Update(visit);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Visit DTOs

    public class VisitDto
    {
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitStatus Status { get; set; }
        public string StatusName { get => this.Status.ToString(); }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public List<VisitMedicationDto> Medications { get; set; } = new();
        public List<VisitServiceDto> Services { get; set; } = new();
        public List<VisitPaymentDto> Payments { get; set; } = new();
    }

    public class VisitListDto
    {
        public List<VisitDto> Data { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class VisitMedicationDto
    {
        public int MedicationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
    }

    public class VisitServiceDto
    {
        public int VisitServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int TotalSessions { get; set; }
        public int CompletedSessions { get; set; }
        public decimal PricePerSession { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainAmount { get; set; }
    }


    public class VisitPaymentDto
    {
        public int visitPaymentId { get; set; }
        public int visitId { get; set; }
        public int? serviceId { get; set; }
        public string? serviceName { get; set; }
        public int sessionNumber { get; set; }
        public decimal amountPaid { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public PaymentType paymentType { get; set; }
        public DateTime paymentDate { get; set; }
    }

    #endregion
}