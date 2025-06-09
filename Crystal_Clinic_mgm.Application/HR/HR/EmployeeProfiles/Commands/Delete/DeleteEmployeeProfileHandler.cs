using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HR.EmployeeProfiles.Commands.Delete
{
    public class DeleteEmployeeProfileHandler : IRequestHandler<DeleteEmployeeProfileCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> _GRepoEmployee;
        private readonly IGenericRepositoryAsync<ERP_DbContext, ContractDetails> _GRepoContractDetails;
        private readonly IMessage _message;
        private readonly ILoggedInUser _loggedInUser;

        public DeleteEmployeeProfileHandler(IGenericRepositoryAsync<ERP_DbContext, EmployeeProfile> gRepoEmployee, IMessage message, ILoggedInUser loggedInUser, IGenericRepositoryAsync<ERP_DbContext, ContractDetails> gRepoContractDetails)
        {
            _GRepoEmployee = gRepoEmployee;
            _message = message;
            _loggedInUser = loggedInUser;
            _GRepoContractDetails = gRepoContractDetails;
        }

        public async Task<JsonResult> Handle(DeleteEmployeeProfileCommand request, CancellationToken cancellationToken)
        {
            var entity = await _GRepoEmployee.FindByCondition(x => !x.IsDeleted && x.ID == request.ID).SingleOrDefaultAsync(cancellationToken);
            if (entity == null)
            {
                return _message.RecordNotFound();
            }
            else
            {
                #region Deleting Education level details and contract details
                var contractDetials = _GRepoContractDetails.FindByCondition(x => !x.IsDeleted && x.EmployeeProfileId == request.ID);

                foreach (var details in contractDetials)
                {
                    details.IsDeleted = true;
                    details.Remarks = request.Remarks;
                    details.ModifiedOn = DateTime.Now;
                    details.ModifiedBy = _loggedInUser.Id;
                    _GRepoContractDetails.EditeAsync(details, cancellationToken);
                }
                #endregion

                entity.IsDeleted = true;
                entity.Remarks = request.Remarks;
                entity.ModifiedOn = DateTime.Now;
                entity.ModifiedBy = _loggedInUser.Id;
                return await _GRepoEmployee.DeleteAsync(entity, cancellationToken);
            }
        }
    }
}
