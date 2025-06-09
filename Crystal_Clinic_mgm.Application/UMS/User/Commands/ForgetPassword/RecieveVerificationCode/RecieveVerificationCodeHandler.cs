using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using static Crystal_Clinic_Mgm.Common.Constants.Constants;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.RecieveVerificationCode
{
    public class RecieveVerificationCodeHandler : IRequestHandler<RecieveVerificationCodeCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, EmailHistory> _repositor;
        private readonly IMessage _messge;

        public RecieveVerificationCodeHandler(
            IGenericRepositoryAsync<UMS_DbContext, EmailHistory> repositor,
            IMessage messge)
        {
            _repositor = repositor;
            _messge = messge;
        }

        public async Task<JsonResult> Handle(RecieveVerificationCodeCommand request, CancellationToken cancellationToken)
        {

            var record = _repositor.FindByCondition(x => !x.IsDeleted && !x.IsVerified && x.ToEmail == request.Email && x.VerificationCode == request.VerificationCode).FirstOrDefault();

            if (record == null || (DateTime.Now - record.CreatedOn).TotalMinutes >= EmailSetting.allowMinuts)
            {
                return _messge.RecordNotFound();
            }
            return await Task.Run(() =>
            {
                record.IsVerified = true;
                record.ModifiedOn = DateTime.Now;
                record.ModifiedBy = record.CreatedBy;
                _repositor.EditeAsync(record, cancellationToken);
                return _messge.Update(record.CreatedBy);
            });
        }
    }
}
