using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Common.CommonColumnNameLocalization;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Command.Create
{
    public class CreatePositionTitleCommandHandler(
        IMessage message,
        ILoggedInUser loggedInUser,
        IGenericRepositoryAsync<ERP_DbContext, PositionTitle> genericRepositoryAsync,
        IStringLocalizer<CommonValidationResource> localizer,
        IStringLocalizer<CommonColumnNameResource> columnLocalizer,
        IGeneralHelperRepositoryAsync<ERP_DbContext, PositionTitle> helper) : IRequestHandler<CreatePositionTitleCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(CreatePositionTitleCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreatePositionTitleCommandValidator(helper, columnLocalizer, localizer).Validate(request).Errors;
            if (validator.Count > 0)
            {
                return message.CheckValidationError(validator);
            }
            var entity = new PositionTitle()
            {
                EnglishName = request.EnglishName,
                PashtoName = request.PashtoName,
                DariName = request.DariName,
                Code = request.Code,
                BranchId = request.BranchId,
                JobDescription = request.JobDescription,
                IsActive = request.IsActive,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
            return await genericRepositoryAsync.AddAsync(entity, cancellationToken);

        }
    }
}
