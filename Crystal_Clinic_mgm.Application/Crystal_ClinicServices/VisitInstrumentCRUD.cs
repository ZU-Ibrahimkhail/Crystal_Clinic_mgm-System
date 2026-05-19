using Azure;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    #region Create Visit Instrument
    public class VisitInstrumentCommand : IRequest<Result>
    {
        public int VisitId { get; set; }
        public List<InstrumentDto>? ServiceSessions { get; set; }
        public List<InstrumentDto>? Kits { get; set; }
        public List<InstrumentDto>? Medications { get; set; }
        public List<InstrumentDto>? Reservations { get; set; }
    }

    public class InstrumentDto
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public bool IsFreeForPatient { get; set; }
    }


    public class VisitInvoicesCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser, IMessage message
        ) : IRequestHandler<VisitInstrumentCommand, Result>
    {
        public async Task<Result> Handle(VisitInstrumentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ServiceSessions != null)
                {
                    foreach (InstrumentDto record in request.ServiceSessions)
                    {
                        var visitInstrumets = new VisitInstrument
                        {
                            VisitId = request.VisitId,
                            ServiceSessionsId = record.Id,
                            Price = record.Price,
                            Count = record.Count,
                            IsFreeForPatient = record.IsFreeForPatient,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.VisitInstrument.Add(visitInstrumets);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }

                if (request.Kits != null)
                {
                    foreach (InstrumentDto record in request.Kits)
                    {
                        var visitInstrumets = new VisitInstrument
                        {
                            VisitId = request.VisitId,
                            KitId = record.Id,
                            Price = record.Price,
                            Count = record.Count,
                            IsFreeForPatient = record.IsFreeForPatient,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow

                        };

                        context.VisitInstrument.Add(visitInstrumets);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }
                if (request.Medications != null)
                {
                    foreach (InstrumentDto record in request.Medications)
                    {
                        var visitInstrumets = new VisitInstrument
                        {
                            VisitId = request.VisitId,
                            ItemId = record.Id,
                            Price = record.Price,
                            Count = record.Count,
                            IsFreeForPatient = record.IsFreeForPatient,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.VisitInstrument.Add(visitInstrumets);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }

                if (request.Reservations != null)
                {
                    foreach (InstrumentDto record in request.Reservations)
                    {
                        var visitInstrumets = new VisitInstrument
                        {
                            VisitId = request.VisitId,
                            InventoryReservationId = record.Id,
                            Price = record.Price,
                            Count = record.Count,
                            IsFreeForPatient = record.IsFreeForPatient,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        context.VisitInstrument.Add(visitInstrumets);
                        await context.SaveChangesAsync(cancellationToken);
                    }
                }

                return Result.Success($"Visit Instrument are successfully created for the {request.VisitId}");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Visit Instrument: {ex.Message}");
            }

        }
    }
    #endregion
    #region Get Instruments List

    public class VisitInstrumentListItemDto
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int? ServiceSessionsId { get; set; }
        public string? ServiceSessionName { get; set; }
        public int? KitId { get; set; }
        public string? KitName { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool IsFreeForPatient { get; set; }
    }

    public class GetVisitInstrumentListQuery : IRequest<Result>
    {
        public int VisitId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetVisitInstrumentListQueryHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<GetVisitInstrumentListQuery, Result>
    {
        public async Task<Result> Handle(GetVisitInstrumentListQuery request, CancellationToken cancellationToken)
        {
            var query = context.VisitInstrument
                .Where(x => x.VisitId == request.VisitId)
                .Include(x => x.ServiceSessions)
                .Include(x => x.InventoryKit)
                .Include(x => x.Item)
                .AsQueryable();

            int totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new VisitInstrumentListItemDto
                {
                    Id = x.Id,
                    VisitId = x.VisitId,
                    ServiceSessionsId = x.ServiceSessionsId,
                    ServiceSessionName = x.ServiceSessions != null ? x.ServiceSessions.serviceName : null,
                    KitId = x.KitId,
                    KitName = x.InventoryKit != null ? x.InventoryKit.KitName : null,
                    ItemId = x.ItemId,
                    ItemName = x.Item != null ? x.Item.Name : null,
                    Price = x.Price,
                    Count = x.Count,
                    IsFreeForPatient = x.IsFreeForPatient
                })
                .ToListAsync(cancellationToken);

            return Result.Success(new { Data = items, TotalCount = totalCount });
        }
    }

    #endregion

    #region Get Instrument Detail

    public class VisitInstrumentDetailDto
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int? ServiceSessionsId { get; set; }
        public string? ServiceSessionName { get; set; }
        public int? SessionNumber { get; set; }
        public bool? IsSessionImplemented { get; set; }
        public int? KitId { get; set; }
        public string? KitName { get; set; }
        public string? KitDescription { get; set; }
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool IsFreeForPatient { get; set; }
    }

    public class GetVisitInstrumentDetailQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class GetVisitInstrumentDetailQueryHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<GetVisitInstrumentDetailQuery, Result>
    {
        public async Task<Result> Handle(GetVisitInstrumentDetailQuery request, CancellationToken cancellationToken)
        {
            var instrument = await context.VisitInstrument
                .Include(x => x.ServiceSessions)
                .Include(x => x.InventoryKit)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (instrument == null)
                return Result.Fail($"Visit Instrument with ID {request.Id} not found.");

            var dto = new VisitInstrumentDetailDto
            {
                Id = instrument.Id,
                VisitId = instrument.VisitId,
                ServiceSessionsId = instrument.ServiceSessionsId,
                ServiceSessionName = instrument.ServiceSessions?.serviceName,
                SessionNumber = instrument.ServiceSessions?.sessionNumber,
                IsSessionImplemented = instrument.ServiceSessions?.IsImplemented,
                KitId = instrument.KitId,
                KitName = instrument.InventoryKit?.KitName,
                KitDescription = instrument.InventoryKit?.Description,
                ItemId = instrument.ItemId,
                ItemName = instrument.Item?.Name,
                ItemDescription = instrument.Item?.Description,
                Price = instrument.Price,
                Count = instrument.Count,
                IsFreeForPatient = instrument.IsFreeForPatient
            };

            return Result.Success(dto);
        }
    }

    #endregion
}
