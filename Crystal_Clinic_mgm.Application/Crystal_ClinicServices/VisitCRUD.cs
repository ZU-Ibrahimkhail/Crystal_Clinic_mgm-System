using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.Services.Repositories;
using Crystal_Clinic_Mgm.Application.Common.SignalR;
using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Crystal_Clinic_Mgm.Application.CrystalClinic.Visits
{
    #region Create Visit

    public class CreateVisitCommand : IRequest<JsonResult>
    {
        public int? PatientId { get; set; } // Nullable to allow new patient creation
        public string? PatientName { get; set; } // Required if PatientId is null
        public string? PatientContactInfo { get; set; } // Required if PatientId is null
        public string? PatientEmail { get; set; } // Optional
        public decimal? age { get; set; }
        public string? gender { get; set; }
        public int? DoctorId { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitStatus Status { get; set; } = VisitStatus.SCHEDULED;
        public decimal FeeAmount { get; set; } = 0;
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
                .WithMessage("VisitDate is required.");

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
                    age = request.age,
                    gender = request.gender,
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

            var branchDetails = context.BranchDetails.Where(x => x.BranchId == loggedInUser.BranchId).ToList();
            int? branchDetailId = null;
            if (branchDetails.Count > 0)
            {
                branchDetailId = branchDetails.FirstOrDefault(x => x.IsActive)?.Id ?? branchDetails.First().Id;
            }

            var visit = new Visit
            {
                patientId = patientId,
                doctorId = request.DoctorId,
                visitDate = request.VisitDate,
                BranchId = loggedInUser.BranchId,
                BranchDetailsId = branchDetailId,
                status = request.Status,
                FeeAmount = request.FeeAmount,
                totalAmount = request.FeeAmount,
                paidAmount = 0,
                remainingAmount = request.FeeAmount,
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
        public decimal FeeAmount { get; set; } = 0;
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
                .WithMessage("VisitDate is required.");

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
            visit.totalAmount -= visit.FeeAmount;
            visit.totalAmount += request.FeeAmount;
            visit.remainingAmount = visit.totalAmount - visit.paidAmount;
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

    public class GetVisitDetailsHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<GetVisitDetailsQuery, VisitDto>
    {
        public async Task<VisitDto> Handle(GetVisitDetailsQuery request, CancellationToken cancellationToken)
        {
            var visit = await context.Visit
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Include(v => v.Payments).Include(x => x.Services)
                .Include(v => v.Medications)
                .ThenInclude(vm => vm.stock).ThenInclude(s => s.Item)
                .Include(v => v.Services).ThenInclude(x => x.CurrencyType)
                .Include(v => v.Services)
                .ThenInclude(vs => vs.service)
                .Include(v => v.Services)
            .ThenInclude(vs => vs.sessions)
            .FirstOrDefaultAsync(v => !v.IsDeleted && (loggedInUser.IsSuperAdmin || v.BranchId == loggedInUser.BranchId) && v.visitId == request.VisitId, cancellationToken);

            if (visit == null)
            {
                throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");
            }

            return new VisitDto
            {
                VisitId = visit.visitId,
                PatientId = visit.patientId,
                PatientName = visit.Patient!.name,
                DoctorId = visit.doctorId,
                DoctorName = visit.Doctor != null ? $"{visit.Doctor.firstName} {visit.Doctor.lastName}" : null,
                speciality = visit.Doctor!.specialty,
                VisitDate = visit.visitDate,
                Status = visit.status,
                FeeAmount = visit.FeeAmount,
                TotalAmount = visit.totalAmount,
                PaidAmount = visit.paidAmount,
                RemainingAmount = visit.remainingAmount,
                Medications = visit.Medications.Select(m => new VisitMedicationDto
                {
                    MedicationId = m.medicationId,
                    Name = m.stock.Item.Name,
                    Dosage = m.dosage,
                    Quantity = m.quantity,
                    Price = m.price,
                    BatchNumber = m.stock.BatchNumber
                }).ToList(),
                Services = visit.Services.Select(s => new VisitServiceDto
                {
                    VisitServiceId = s.visitServiceId,
                    ServiceName = s.service?.Name ?? "",
                    TotalSessions = s.totalSessions,
                    CompletedSessions = s.completedSessions,
                    AddedSessions = s.sessions.Count,
                    PricePerSession = s.pricePerSession,
                    TotalPrice = s.totalPrice,
                    CurrencyTypeId = s.CurrencyTypeId,
                    CurrencyCode = s.CurrencyType?.Code
                }).ToList(),

                Payments = visit.Payments.Where(x => !x.IsDeleted).Select(s => new VisitPaymentDto
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
                    CurrencyTypeId = s.CurrencyTypeId,
                    CurrencyCode = s.CurrencyType?.Code ?? "",
                    ExchangeRateToAFN = s.ExchangeRateToAFN,
                    AmountInAFN = s.AmountInAFN,

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
        public VisitStatus? Status { get; set; }
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

    public class GetVisitListHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<GetVisitListQuery, VisitListDto>
    {
        public async Task<VisitListDto> Handle(GetVisitListQuery request, CancellationToken cancellationToken)
        {
            var query = context.Visit
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .Where(v => !v.IsDeleted && (loggedInUser.IsSuperAdmin || v.BranchId == loggedInUser.BranchId))
                .AsQueryable();

            if (request.PatientId.HasValue)
            {
                query = query.Where(v => v.patientId == request.PatientId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(v =>
                                    v.visitId.ToString() == (request.Search) ||
                                    v.Patient.name.Contains(request.Search) ||
                                    v.Patient.contactInfo.Contains(request.Search) ||
                                         (v.Doctor != null && (v.Doctor.firstName.Contains(request.Search) || v.Doctor.lastName.Contains(request.Search))));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            if (request.LastVisitId.HasValue)
            {
                query = query.Where(v => v.visitId > request.LastVisitId.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(v => v.status > request.Status.Value);
            }

            query = query.OrderBy(v => v.visitId).Take(request.PageSize);

            var visits = await query
                .Select(v => new VisitDto
                {
                    VisitId = v.visitId,
                    PatientId = v.patientId,
                    PatientName = v.Patient!.name,
                    DoctorId = v.doctorId,
                    DoctorName = v.Doctor != null ? $"{v.Doctor.firstName} {v.Doctor.lastName}" : null,
                    speciality = v.Doctor!.specialty,
                    VisitDate = v.visitDate,
                    Status = v.status,
                    FeeAmount = v.FeeAmount,
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
                .Include(s => s.Item)
                .Where(s => stockIds.Contains(s.StockId))
                .ToDictionaryAsync(s => s.StockId, s => s, cancellationToken);

            foreach (var med in request.Medications)
            {
                if (!stocks.TryGetValue(med.StockId, out var stock) || stock.Quantity < med.Quantity)
                {
                    throw new InvalidOperationException($"Stock with ID {med.StockId} not found or insufficient quantity.");
                }

                if (stock.BatchNumber != med.BatchNumber)
                {
                    throw new InvalidOperationException($"Batch number does not match for stock ID {med.StockId}.");
                }

                var medication = new VisitMedication
                {
                    visitId = request.VisitId,
                    stockId = med.StockId,
                    name = stock.Item.Name, // Fixed: Use stock.item.Name
                    dosage = med.Dosage,
                    quantity = med.Quantity,
                    price = med.Price,
                    stock = stock,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                stock.Quantity -= med.Quantity;
                if (stock.Quantity < stock.Item.ReorderLevel)
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
        public int CurrencyTypeId { get; set; } = 2;
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

    public class AddVisitServiceHandler(ERP_DbContext context, UMS_DbContext ums_dbContext,
     ILoggedInUser loggedInUser,
     INotificationRepository notificationRepository,
     IMessageHubClient signal,
     ILogger<AddVisitServiceHandler> logger) : IRequestHandler<AddVisitServiceCommand, bool>
    {
        private ILogger<AddVisitServiceHandler> _logger => logger;
        public async Task<bool> Handle(AddVisitServiceCommand request, CancellationToken cancellationToken)
        {
            var visit = await context.Visit.Include(x => x.Patient).FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken) ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");
            var serviceIds = request.Services.Select(s => s.ServiceId).ToList();
            var services = await context.Services
                .Where(s => serviceIds.Contains(s.ServiceId))
                .ToDictionaryAsync(s => s.ServiceId, s => s, cancellationToken);
            var afnCurrencyId = (await context.CurrencyType.FirstAsync(c => c.Code == "AFN", cancellationToken)).ID;
            List<VisitServices> visitServices = [];
            Dictionary<int, decimal> exchangeRates = []; ;

            foreach (var svc in request.Services)
            {
                if (!services.TryGetValue(svc.ServiceId, out var service))
                {
                    throw new KeyNotFoundException($"Service with ID {svc.ServiceId} not found.");
                }
                var currency = await context.CurrencyType
                                 .FirstOrDefaultAsync(c => c.ID == svc.CurrencyTypeId, cancellationToken)
                                 ?? throw new KeyNotFoundException($"CurrencyType with ID {svc.CurrencyTypeId} not found.");

                var exchangeRateToAFN = svc.CurrencyTypeId == afnCurrencyId ? 1 : await context.GetExchangeRate(svc.CurrencyTypeId, afnCurrencyId, cancellationToken);
                exchangeRates.Add(svc.CurrencyTypeId, exchangeRateToAFN);
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
                    paymentStatus = PaymentStatus.Pending,
                    CurrencyTypeId = svc.CurrencyTypeId,
                };


                visitServices.Add(visitService);


            }

            visit.ModifiedOn = DateTime.Now;
            visit.ModifiedBy = loggedInUser.Id;
            context.VisitServices.AddRange(visitServices);
            context.SaveChanges();
            List<ServiceSessions> serviceSessions = [];
            foreach (var svc in visitServices)
            {
                var exRate = exchangeRates.First(x => x.Key == svc.CurrencyTypeId).Value;
                if (svc.startDate.Date == DateTime.Today)
                {
                    services.TryGetValue(svc.serviceId, out var service);
                    var serviceSession = new ServiceSessions
                    {
                        visitServiceId = svc.visitServiceId,
                        visitId = visit.visitId,
                        serviceId = svc.serviceId,
                        serviceName = service?.Name ?? "Unknown",
                        patientName = visit.Patient?.name ?? "Unknown",
                        contactInfo = visit.Patient?.contactInfo ?? "Unknown",
                        ImplementationDate = svc.startDate,
                        sessionNumber = 1,
                        PriceInAFN = svc.pricePerSession * exRate
                    };
                    visit.totalAmount += serviceSession.PriceInAFN;
                    context.ServiceSessions.Add(serviceSession);
                    serviceSessions.Add(serviceSession);
                }
            }
            visit.remainingAmount = visit.totalAmount - visit.paidAmount;
            context.Visit.Update(visit);
            context.SaveChanges();
            foreach (var item in serviceSessions)
            {
                _logger.LogInformation("Preparing to send notification for ServiceSession ID: {serviceId}", item.serviceId);
                var employees = context.Doctor.Where(x => !x.IsDeleted && x.services.Contains(item.serviceId.ToString())).ToList();
                int employeeId = 0;
                foreach (var emp in employees)
                {
                    List<string> sids = emp.services.Split(',').ToList();
                    if (sids.Contains(item.serviceId.ToString()))
                    {
                        employeeId = emp.employeeId;
                        _logger.LogInformation("Found Employee ID: {EmployeeId} for ServiceSession ID: {ServiceSessionId}", employeeId, item.Id);
                        if (employeeId != null)
                        {
                            var user = ums_dbContext.Users.FirstOrDefault(x => x.EmployeeId == employeeId);
                            if (user != null)
                            { 
                                notificationRepository.AddNotification(user.Id, Constants.NotificationMessage.NewServiceSessionRecord, Constants.ApplicationModule.Clinic, user.BranchId ?? 0, item.Id, null);
                                await signal.PushAsync(user.Id, Constants.NotificationMessage.NewServiceSessionRecord);
                            }
                        }
                    }
                }


            }
            return true;
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
                    .Include(v => v.Services).ThenInclude(s => s.CurrencyType)
                    .Include(v => v.Payments).ThenInclude(p => p.CurrencyType)
                    .FirstOrDefaultAsync(v => !v.IsDeleted && v.visitId == request.VisitId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit with ID {request.VisitId} not found.");

                var afnCurrencyId = (await context.CurrencyType.FirstAsync(c => c.Code == "AFN", cancellationToken)).ID;

                if (request.VisitPaymentId.HasValue)
                {
                    var payment = visit.Payments
                        .FirstOrDefault(p => p.visitPaymentId == request.VisitPaymentId.Value && !p.IsDeleted)
                        ?? throw new KeyNotFoundException($"Payment with ID {request.VisitPaymentId} not found.");

                    payment.IsDeleted = true;
                    payment.ModifiedBy = loggedInUser.Id;
                    payment.ModifiedOn = DateTime.UtcNow;

                    visit.paidAmount -= payment.AmountInAFN;
                    visit.paidAmount += payment.RefundAmountInAFN;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;
                    // Find or create MainAccount for the payment currency
                    var mainAccount = await context.MainAccount
                        .FirstOrDefaultAsync(x => !x.IsDeleted && x.CurrencyTypeId == payment.CurrencyTypeId && x.OwnerUserId == loggedInUser.Id, cancellationToken);

                    if (mainAccount == null)
                    {
                        mainAccount = new MainAccount
                        {
                            CurrencyTypeId = payment.CurrencyTypeId ?? afnCurrencyId,
                            OwnerUserId = loggedInUser.Id,
                            BalanceAmount = 0,
                            TotalCreditAmount = 0,
                            TotalDebitAmount = 0,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };
                        context.MainAccount.Add(mainAccount);
                        await context.SaveChangesAsync(cancellationToken);
                    }

                    // Update Main Account for payment deletion
                    mainAccount.TotalCreditAmount -= Convert.ToDouble(payment.amountPaid); // Reverse payment received
                    mainAccount.TotalDebitAmount -= Convert.ToDouble(payment.RefundAmountInAFN / payment.ExchangeRateToAFN); // Reverse refund given
                    mainAccount.BalanceAmount -= Convert.ToDouble(payment.amountPaid - (payment.RefundAmountInAFN / payment.ExchangeRateToAFN));
                    mainAccount.ModifiedOn = DateTime.UtcNow;
                    mainAccount.ModifiedBy = loggedInUser.Id;

                    // Add Account Tracking Record
                    var accountTracking = new AccountTracking
                    {
                        CurrencyTypeId = payment.CurrencyTypeId ?? afnCurrencyId,
                        TransactionDate = DateTime.UtcNow,
                        Description = $"Payment Reversal for Visit Payment ID {payment.visitPaymentId}",
                        UserId = loggedInUser.Id,
                        DebitAmount = Convert.ToDouble(payment.amountPaid), // Reverse payment as debit
                        CreditAmount = Convert.ToDouble(payment.RefundAmountInAFN / payment.ExchangeRateToAFN), // Reverse refund as credit
                        BalanceAmount = mainAccount.BalanceAmount,
                        MainAccountId = mainAccount.ID,
                        trackType = TrackType.EXPENSE, // Reversal treated as expense
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow
                    };

                    context.MainAccount.Update(mainAccount);
                    context.AccountTracking.Add(accountTracking);
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
                        stock.Quantity += medication.quantity;
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


                    decimal amountToSubtruct = context.ServiceSessions.Where(x => x.visitServiceId == request.VisitServiceId.Value && !x.IsImplemented).Sum(x => x.PriceInAFN);
                    visit.totalAmount -= amountToSubtruct;
                    visit.remainingAmount = visit.totalAmount - visit.paidAmount;
                    var sesssions = context.ServiceSessions.Where(x => x.visitServiceId == request.VisitServiceId.Value);
                    var impCount = sesssions.Count(x => x.IsImplemented);
                    var remainCount = sesssions.Count(x => !x.IsImplemented);
                    if (impCount > 0)
                    {
                        service.totalSessions = impCount;
                        service.PaidSessions = impCount;
                        service.completedSessions = impCount;
                    }
                    else
                    {
                        context.VisitServices.Remove(service);
                    }
                    context.ServiceSessions.RemoveRange(sesssions.Where(x => !x.IsImplemented));
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
        public string? speciality { get; set; }
        public DateTime VisitDate { get; set; }
        public VisitStatus Status { get; set; }
        public string StatusName { get => this.Status.ToString(); }
        public decimal FeeAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public List<VisitMedicationDto> Medications { get; set; } = [];
        public List<VisitServiceDto> Services { get; set; } = [];
        public List<VisitPaymentDto> Payments { get; set; } = [];
        public Dictionary<string, decimal> CurrencyTotals { get; set; } = new(); // e.g., {"AFN": 500, "USD": 100, "EUR": 50}

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
        public int CurrencyTypeId { get; set; }
        public string? CurrencyCode { get; set; }
        public int AddedSessions { get; set; }
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
        public int? CurrencyTypeId { get; set; }
        public string CurrencyCode { get; set; } = "";
        public decimal AmountInAFN { get; set; }
        public decimal ExchangeRateToAFN { get; set; }
        public decimal RefundAmountInAFN { get; set; }

    }

    #endregion

}