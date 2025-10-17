using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices
{
    public class CreateCallListCommand : IRequest<int>
    {
        public CallingReason CallingReason { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ToBeCalledDate { get; set; }
        public int? AssignedEmployeeId { get; set; }
    }

    public class CreateCallListHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<CreateCallListCommand, int>
    {
        public async Task<int> Handle(CreateCallListCommand request, CancellationToken cancellationToken)
        {
            var callList = new CallList
            {
                CallingReason = request.CallingReason,
                Name = request.Name,
                Description = request.Description,
                PhoneNumber = request.PhoneNumber,
                ToBeCalledDate = request.ToBeCalledDate,
                AssignedEmployeeId = request.AssignedEmployeeId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = loggedInUser.Id
            };

            context.CallList.Add(callList);
            await context.SaveChangesAsync(cancellationToken);
            return callList.Id;
        }
    }

    public class UpdateCallListCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public CallingReason CallingReason { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ToBeCalledDate { get; set; }
        public int? AssignedEmployeeId { get; set; }
    }
    public class UpdateCallListHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<UpdateCallListCommand, bool>
    {
        public async Task<bool> Handle(UpdateCallListCommand request, CancellationToken cancellationToken)
        {
            var callList = await context.CallList.FindAsync(request.Id);
            if (callList == null || callList.IsDeleted)
                return false;

            callList.CallingReason = request.CallingReason;
            callList.Name = request.Name;
            callList.Description = request.Description;
            callList.PhoneNumber = request.PhoneNumber;
            callList.ToBeCalledDate = request.ToBeCalledDate;
            callList.AssignedEmployeeId = request.AssignedEmployeeId;
            callList.ModifiedOn = DateTime.UtcNow;
            callList.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public class DeleteCallListCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
    public class DeleteCallListHandler(ERP_DbContext context)
        : IRequestHandler<DeleteCallListCommand, bool>
    {
        public async Task<bool> Handle(DeleteCallListCommand request, CancellationToken cancellationToken)
        {
            var callList = await context.CallList.FindAsync(request.Id);
            if (callList == null || callList.IsDeleted)
                return false;

            callList.IsDeleted = true;
            context.CallList.Update(callList);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public class AssignCallListEmployeeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int? AssignedEmployeeId { get; set; }
    }

    public class AssignCallListEmployeeHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<AssignCallListEmployeeCommand, bool>
    {
        public async Task<bool> Handle(AssignCallListEmployeeCommand request, CancellationToken cancellationToken)
        {
            var callList = await context.CallList.FindAsync(request.Id);
            if (callList == null || callList.IsDeleted)
                return false;

            callList.AssignedEmployeeId = request.AssignedEmployeeId;
            callList.ModifiedOn = DateTime.UtcNow;
            callList.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    public class EnterCallResponseCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DateTime ActualCalledDate { get; set; }
        public CallResponseType CallResponse { get; set; }
        public string ResponseReasult { get; set; } = string.Empty;
    }

    public class EnterCallResponseHandler(ERP_DbContext context, ILoggedInUser loggedInUser)
        : IRequestHandler<EnterCallResponseCommand, bool>
    {
        public async Task<bool> Handle(EnterCallResponseCommand request, CancellationToken cancellationToken)
        {
            var callList = await context.CallList.FindAsync(request.Id);
            if (callList == null || callList.IsDeleted)
                return false;

            callList.ActualCalledDate = request.ActualCalledDate;
            callList.CallResponse = request.CallResponse;
            callList.ResponseReasult = request.ResponseReasult;
            callList.ModifiedOn = DateTime.UtcNow;
            callList.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    public class GetCallListByIdQuery : IRequest<CallList?>
    {
        public int Id { get; set; }
    }

    public class GetCallListByIdHandler(ERP_DbContext context)
        : IRequestHandler<GetCallListByIdQuery, CallList?>
    {
        public async Task<CallList?> Handle(GetCallListByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.CallList
                .Include(cl => cl.AssignedEmployee)
                .Where(cl => !cl.IsDeleted)
                .FirstOrDefaultAsync(cl => cl.Id == request.Id, cancellationToken);
        }
    }

    public class GetAllCallListsQuery : IRequest<List<CallList>>
    {
        public string? SearchBy { get; set; }
        public int PageSize { get; set; } = 30;
        public int? LastId { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public bool? UnAssinged { get; set; }
        public DateTime? TobeCalledDate { get; set; }
        public CallingReason? CallingReason { get; set; }
        public CallResponseType? CallResponseType { get; set; }
    }

    public class GetAllCallListsHandler(ERP_DbContext context)
        : IRequestHandler<GetAllCallListsQuery, List<CallList>>
    {
        public async Task<List<CallList>> Handle(GetAllCallListsQuery request, CancellationToken cancellationToken)
        {
            var data = context.CallList
                .Include(cl => cl.AssignedEmployee)
                .Where(cl => !cl.IsDeleted)
                .OrderByDescending(cl => cl.Id)
                .AsQueryable();
            if (request.TobeCalledDate.HasValue) 
            { 
                data = data.Where(x=>x.ToBeCalledDate.Date ==  request.TobeCalledDate.Value.Date);
            }
            if (request.AssignedEmployeeId.HasValue)
            {
                data = data.Where(x => x.AssignedEmployeeId == request.AssignedEmployeeId);
            }else if (request.UnAssinged.HasValue)
            {
                data = data.Where(x => x.AssignedEmployeeId == null);
            }
            if (request.CallingReason.HasValue)
            {
                data = data.Where(x=>x.CallingReason ==  request.CallingReason);
            }
            if (request.CallResponseType.HasValue)
            {
                data = data.Where(x => x.CallResponse == request.CallResponseType);
            }
            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                data = data.Where(cl => cl.Name.Contains(request.SearchBy)
                    || cl.Description.Contains(request.SearchBy)
                    || cl.PhoneNumber.Contains(request.SearchBy)
                    || cl.ResponseReasult.Contains(request.SearchBy));
            }

            if (request.LastId.HasValue)
            {
                data = data.Where(cl => cl.Id < request.LastId);
            }

            return await data.Take(request.PageSize).ToListAsync(cancellationToken);
        }
    }

    public class GetCallListReportQuery : IRequest<List<CallList>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool UseActualCalledDate { get; set; } // If true, filter by ActualCalledDate; if false, filter by ToBeCalledDate
        public string? SearchBy { get; set; }
        public int PageSize { get; set; } = 1000;
        public int? LastId { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public CallingReason? CallingReason { get; set; }
        public CallResponseType? CallResponseType { get; set; }
    }

    public class GetCallListReportHandler(ERP_DbContext context)
        : IRequestHandler<GetCallListReportQuery, List<CallList>>
    {
        public async Task<List<CallList>> Handle(GetCallListReportQuery request, CancellationToken cancellationToken)
        {
            var data = context.CallList
                .Include(cl => cl.AssignedEmployee)
                .Where(cl => !cl.IsDeleted)
                .OrderByDescending(cl => cl.Id)
                .AsQueryable();

            // Apply date range filter
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                if (request.UseActualCalledDate)
                {
                    data = data.Where(cl => cl.ActualCalledDate.HasValue
                        && cl.ActualCalledDate.Value.Date >= request.StartDate.Value.Date
                        && cl.ActualCalledDate.Value.Date <= request.EndDate.Value.Date);
                }
                else
                {
                    data = data.Where(cl => cl.ToBeCalledDate.Date >= request.StartDate.Value.Date
                        && cl.ToBeCalledDate.Date <= request.EndDate.Value.Date);
                }
            }

            // Apply additional filters
            if (request.AssignedEmployeeId.HasValue)
            {
                data = data.Where(cl => cl.AssignedEmployeeId == request.AssignedEmployeeId);
            }

            if (request.CallingReason.HasValue)
            {
                data = data.Where(cl => cl.CallingReason == request.CallingReason);
            }

            if (request.CallResponseType.HasValue)
            {
                data = data.Where(cl => cl.CallResponse == request.CallResponseType);
            }

            if (!string.IsNullOrEmpty(request.SearchBy))
            {
                data = data.Where(cl => cl.Name.Contains(request.SearchBy)
                    || cl.Description.Contains(request.SearchBy)
                    || cl.PhoneNumber.Contains(request.SearchBy)
                    || cl.ResponseReasult.Contains(request.SearchBy));
            }

            if (request.LastId.HasValue)
            {
                data = data.Where(cl => cl.Id < request.LastId);
            }

            return await data.Take(request.PageSize).ToListAsync(cancellationToken);
        }
    }
}