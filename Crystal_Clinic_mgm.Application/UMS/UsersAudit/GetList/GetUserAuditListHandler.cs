using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.UsersAudit.GetList
{
    public class GetUserAuditListHandler : IRequestHandler<GetUserAuditListQuery, ResponseDataTable<GetUserAuditListModel>>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, UserAudit> _genericRepositoryAsync;
        private readonly IMapper _mapper;

        public GetUserAuditListHandler(IGenericRepositoryAsync<UMS_DbContext, UserAudit> genericRepositoryAsync, IMapper mapper)
        {
            _genericRepositoryAsync = genericRepositoryAsync;
            _mapper = mapper;
        }

        public async Task<ResponseDataTable<GetUserAuditListModel>> Handle(GetUserAuditListQuery request, CancellationToken cancellationToken)
        {
            string? search = request.Search?.ToLower().Trim();
            var entity = await _genericRepositoryAsync.FindByCondition(x => x.UserName != null).ToListAsync(cancellationToken);
            var records = _mapper.Map<IEnumerable<GetUserAuditListModel>>(entity);
            if (search != null)
            {
                records =
                    records.Where(x => x.UserName.ToLower().Contains(search)
                    || x.DeviceName.ToLower().Contains(search)
                    || x.BrowserName.ToLower().Contains(search)
                    || x.Branch.ToLower().Contains(search)
                ).OrderByDescending(x => x.ActionOn).ThenBy(x => x.ActionEnd);
            }

            // var records = _mapper.Map<IEnumerable<GetUserAuditListModel>>(entity);         
            return MyDataTable<GetUserAuditListModel>.Generate(records!, request.PageSize, request.PageIndex);
        }
    }
}
