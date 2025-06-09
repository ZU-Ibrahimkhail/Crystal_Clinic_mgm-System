using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Common.Helper;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Queries.GetEmployeeProfileList
{
    public class GetEmployeeProfileListHandler : IRequestHandler<GetEmployeeProfileListQuery, ResponseDataTable<GetEmployeeProfileListModel>>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IMapper _mapper;

        public GetEmployeeProfileListHandler(IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee, IMapper mapper)
        {
            _GRepoEmployee = gRepoEmployee;
            _mapper = mapper;
        }

        public async Task<ResponseDataTable<GetEmployeeProfileListModel>> Handle(GetEmployeeProfileListQuery request, CancellationToken cancellationToken)
        {
            var Language = GeneralHelper.SelectedLanauge(request.Language);
            return await Task.Run(() =>
            {
                var entity = _GRepoEmployee.FindByCondition(x => !x.IsDeleted)
                    .Include(x => x.Branch)
                    .OrderByDescending(x => x.ModifiedOn);
                var records = _mapper.Map<IEnumerable<GetEmployeeProfileListModel>>(entity);
                if (!string.IsNullOrEmpty(request.SearchBy))
                {
                    var search = request.SearchBy.ToLower().Trim();
                    records = records.Where(x =>
                                          x.Name.ToLower().Contains(search) ||
                                          x.SurName.ToLower().Contains(search) ||
                                          x.PhoneNumber.ToLower().Contains(search) ||
                                          x.BranchName.ToLower().Contains(search));
                }
                var result = MyDataTable<GetEmployeeProfileListModel>.Generate(records, request.PageSize, request.PageIndex);
                return result;
            });
        }
    }
}
