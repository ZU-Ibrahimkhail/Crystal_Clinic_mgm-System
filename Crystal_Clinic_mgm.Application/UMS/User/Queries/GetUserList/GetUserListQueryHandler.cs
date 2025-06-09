using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserList
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, DataTableResponse>
    {

        public readonly UMS_DbContext _context;
        public readonly ERP_DbContext _erpDbContext;
        private readonly IGeneralHelperRepositoryAsync _helper;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoUser;
        public GetUserListQueryHandler(UMS_DbContext context, IGeneralHelperRepositoryAsync helper, IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> gRepoUser, ERP_DbContext erpDbContext)
        {
            _context = context;
            _helper = helper;
            _GRepoUser = gRepoUser;
            _erpDbContext = erpDbContext;
        }
        public async Task<DataTableResponse> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var language = GeneralHelper.SelectedLanauge(request.Language);
            var query = _GRepoUser.FindByCondition(r => r.IsDeleted == false && r.UserName != "SuperAdmin");
            if (!string.IsNullOrWhiteSpace(request.SearchBy))
            {
                query = query.Where(u =>
                    EF.Functions.Like(u.UserName, $"%{request.SearchBy}%")
                    || EF.Functions.Like(u.Email, $"%{request.SearchBy}%")
                );
            }

            var data = query.OrderByDescending(x => x.ModifiedOn)
                .Select(x => UserListLookupModel.Projection.Compile().Invoke(x, _context, _erpDbContext, _helper, language)).ToList();
            return await DataTable<UserListLookupModel>.Generate(data, request.PageSize, request.PageIndex);
        }
    }
}

