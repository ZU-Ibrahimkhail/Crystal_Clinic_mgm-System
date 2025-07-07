using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    #region Add Service Session

    public class AddServiceSessionCommand : IRequest<bool>
    {
        public int VisitServiceId { get; set; }
    }

    public class AddServiceSessionCommandValidator : AbstractValidator<AddServiceSessionCommand>
    {
        public AddServiceSessionCommandValidator()
        {
            RuleFor(x => x.VisitServiceId)
                .GreaterThan(0)
                .WithMessage("VisitServiceId must be greater than 0.");

        }
    }

    public class AddServiceSessionHandler(ERP_DbContext context) : IRequestHandler<AddServiceSessionCommand, bool>
    {
        public async Task<bool> Handle(AddServiceSessionCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var visitService = await context.VisitServices.Include(x=>x.service)
                    .FirstOrDefaultAsync(v => v.visitServiceId == request.VisitServiceId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Visit Service with ID {request.VisitServiceId} not found.");

                var visit = await context.Visit.Include(x=>x.Patient)
                    .FirstOrDefaultAsync(v => v.visitId == visitService.visitId, cancellationToken)
                                       ?? throw new KeyNotFoundException($"Visit with ID {visitService.visitId} not found.");

                if (visitService.completedSessions == visitService.totalSessions)
                {
                    throw new InvalidOperationException("All sessions of the service are completed.");
                }

                var afnCurrencyId = (await context.CurrencyType.FirstAsync(c => c.Code == "AFN", cancellationToken)).ID;
                var exchangeRateToAFN = await context.GetExchangeRate(visitService.CurrencyTypeId, afnCurrencyId, cancellationToken);

                var priceInAfn = visitService.pricePerSession * exchangeRateToAFN;
                var serviceSession = new ServiceSessions
                {
                    visitServiceId = visitService.visitServiceId,
                    visitId = visitService.visitId,
                    serviceId = visitService.visitId,
                    serviceName = visitService.service!.Name,
                    patientName = visit.Patient!.name,
                    contactInfo = visit.Patient!.contactInfo,
                    sessionNumber = visitService.completedSessions + 1,
                    PriceInAFN = priceInAfn,
                    ImplementationDate = visitService.nextSessionDate
                };
                context.ServiceSessions.Add(serviceSession);
                visitService.PaidSessions += 1;
                visit.totalAmount += priceInAfn;
                visit.remainingAmount = visit.totalAmount - visit.paidAmount;
                context.Visit.Update(visit);
                context.VisitServices.Update(visitService);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Service Session implementation

    public class ServiceSessionImpCommand : IRequest<bool>
    {
        public int sessionId { get; set; }
    }

    public class ServiceSessionCommandImpValidator : AbstractValidator<ServiceSessionImpCommand>
    {
        public ServiceSessionCommandImpValidator()
        {
            RuleFor(x => x.sessionId)
                .GreaterThan(0)
                .WithMessage("VisitServiceId must be greater than 0.");

        }
    }

    public class ServiceSessionImpHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ServiceSessionImpCommand, bool>
    {
        public async Task<bool> Handle(ServiceSessionImpCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var session = await context.ServiceSessions
                    .FirstOrDefaultAsync(v => v.Id == request.sessionId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Service session with ID {request.sessionId} not found.");

                var visitService = await context.VisitServices
                    .FirstOrDefaultAsync(v => v.visitServiceId == session.visitServiceId, cancellationToken)
                                       ?? throw new KeyNotFoundException($"visit service ID {session.visitServiceId} not found.");
                if (visitService.completedSessions == visitService.totalSessions)
                {
                    throw new InvalidOperationException("All sessions of the service are completed.");
                }

                session.ImplementationDate = DateTime.Today;
                session.IsImplemented = true;
                session.ImplementorEmployeeId = loggedInUser.EmployeeId;
                visitService.completedSessions += 1;
                context.VisitServices.Update(visitService);
                context.ServiceSessions.Update(session);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region GetServiceSessionById
    public class GetServiceSessionByIdQuery : IRequest<ServiceSessions>
    {
        public int sessionId { get; set; }
    }

    public class GetServiceSessionByIdHandler(ERP_DbContext context) : IRequestHandler<GetServiceSessionByIdQuery, ServiceSessions>
    {
        public async Task<ServiceSessions> Handle(GetServiceSessionByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.ServiceSessions.FirstOrDefaultAsync(s => s.Id == request.sessionId, cancellationToken) ?? new ServiceSessions();
        }
    }
    #endregion

    #region ListAllServicesSessions

    public class ListAllServicesSessionsQuery : IRequest<List<ServiceSessionDto>>
    {
        public string? Search { get; set; }
        public int? LastSessionId { get; set; } // For cursor pagination
        public int PageSize { get; set; } = 20;
    }

    public class ServiceSessionDto
    {
        public int Id { get; set; }
        public int visitServiceId { get; set; }
        public int visitId { get; set; }
        public int serviceId { get; set; }
        public string serviceName { get; set; } = string.Empty;
        public string patientName { get; set; } = string.Empty;
        public string contactInfo { get; set; } = string.Empty; 
        public int sessionNumber { get; set; }
        public decimal PriceInAFN { get; set; }
        public bool IsImplemented { get; set; } = false;
        public DateTime? ImplementationDate { get; set; }
        public int? ImplementorEmployeeId { get; set; }
        public string? ImplementorEmployee { get; set; }
    }

    public class ListAllServicesSessionsHandler(ERP_DbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<ListAllServicesSessionsQuery, List<ServiceSessionDto>>
    {
        public async Task<List<ServiceSessionDto>> Handle(ListAllServicesSessionsQuery request, CancellationToken cancellationToken)
        {
            var query = context.ServiceSessions.AsQueryable();

            Localization localize = new(httpContextAccessor);
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(s => s.ImplementorEmployee!.EnglishFirstName.Contains(request.Search));
            }

            if (request.LastSessionId.HasValue)
            {
                query = query.Where(s => s.Id > request.LastSessionId.Value);
            }

            query = query
                //.Include(x => x.CurrencyType)
                .OrderByDescending(s => s.ImplementationDate)
                .Take(request.PageSize);

            return await query.Select(s => new ServiceSessionDto
            {
                Id = s.Id,
                visitServiceId = s.visitServiceId,
                visitId = s.visitId,
                serviceId = s.serviceId,
                serviceName = s.serviceName,
                patientName = s.patientName,
                contactInfo = s.contactInfo,
                sessionNumber = s.sessionNumber,
                PriceInAFN = s.PriceInAFN,
                IsImplemented = s.IsImplemented,
                ImplementationDate = s.ImplementationDate,
                ImplementorEmployeeId = s.ImplementorEmployeeId,
                ImplementorEmployee = s.ImplementorEmployee!.EnglishFirstName,
            }).ToListAsync(cancellationToken);
        }
    }
    #endregion
}
