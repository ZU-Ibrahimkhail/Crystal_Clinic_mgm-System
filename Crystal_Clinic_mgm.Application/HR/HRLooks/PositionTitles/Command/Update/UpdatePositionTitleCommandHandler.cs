using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Update
{
    public class UpdatePositionTitleCommandHandler(
        IMessage message,
        ILoggedInUser loggedInUser,
        IGenericRepositoryAsync<ERP_DbContext, PositionTitle> genericRepositoryAsync,
        IStringLocalizer<CommonValidationResource> localizer,
        IStringLocalizer<CommonColumnNameResource> columnLocalizer,
        IGeneralHelperRepositoryAsync<ERP_DbContext, PositionTitle> helper) : IRequestHandler<UpdatePositionTitleCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(UpdatePositionTitleCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdatePositionTitleCommandValidator(helper, localizer, columnLocalizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return message.CheckValidationError(validator);
            }
            var entity = await genericRepositoryAsync.GetDetailAsync(request.Id);
            if (entity == null || entity.IsDeleted)
            {
                return message.RecordNotFound();
            }
            else
            {
                entity.EnglishName = request.EnglishName;
                entity.PashtoName = request.PashtoName;
                entity.DariName = request.DariName;
                entity.Code = request.Code;
                entity.BranchId = request.BranchId;
                entity.IsActive = request.IsActive;
                entity.JobDescription = request.JobDescription;
                entity.ModifiedBy = loggedInUser.Id;
                entity.ModifiedOn = DateTime.Now;
                //----Update
                return await genericRepositoryAsync.UpdateAsync(entity, cancellationToken);
            }

        }
    }
}
