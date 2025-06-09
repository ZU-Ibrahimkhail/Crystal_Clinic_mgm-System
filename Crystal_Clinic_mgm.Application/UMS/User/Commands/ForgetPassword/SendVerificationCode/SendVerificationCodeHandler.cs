using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using static Crystal_Clinic_Mgm.Common.Constants.Constants;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ForgetPassword.SendVerificationCode
{
    public class SendVerificationCodeHandler : IRequestHandler<SendVerificationCodeCommand, JsonResult>
    {
        private readonly IGenericRepositoryAsync<UMS_DbContext, EmailHistory> _GRepoEmailHistory;
        private readonly IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> _GRepoApplicationUser;
        private readonly IMessage _messge;
        private readonly IMailRepositoy _mail;

        public SendVerificationCodeHandler(
            IGenericRepositoryAsync<UMS_DbContext, EmailHistory> repositor,
            IMessage messge,
            IMailRepositoy mail,
            IGenericRepositoryAsync<UMS_DbContext, ApplicationUser> gRepoApplicationUser)
        {
            _GRepoEmailHistory = repositor;
            _messge = messge;
            _mail = mail;
            _GRepoApplicationUser = gRepoApplicationUser;
        }

        public async Task<JsonResult> Handle(SendVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            var user = _GRepoApplicationUser.FindByCondition(x => !x.IsDeleted && x.IsActive && x.Email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return _messge.RecordNotFound();
            }
            string verificationCode = new Random().Next(1000, 9999).ToString();


            var emailRequest = new Common.Configuration.MailRequest
            {
                ToEmail = request.Email,
                Subject = "Email Verification for Password Reset",
                Body = @"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {
                                font-family: Arial, sans-serif;
                                background-color: #f7f7f7;
                            }
                            .email-container {
                                width: 100%;
                                max-width: 600px;
                                margin: auto;
                                padding: 20px;
                            }
                            .email-content {
                                padding: 20px;
                                border: 1px solid #dddddd;
                                background-color: #ffffff;
                            }
                            .footer {
                                margin-top: 20px;
                                text-align: center;
                                color: #777777;
                                background-color: #f7f7f7;
                                padding: 10px;
                                position: relative; 
                                width: 100%;
                            }
                        </style>
                    </head>" + $@"
                    <body>
                        <div class='email-container'>
                            <div class='email-content'>
                                <p>Dear <strong>{user.UserName}</strong>,<br />
                                <br />Your verification code is: <strong>{verificationCode}</strong>
                                <br />Please use this code within the next {EmailSetting.allowMinuts} minutes. The code will expire at:
                                <br /><strong>{DateTime.Now.AddMinutes(EmailSetting.allowMinuts):dddd, MMMM dd, yyyy HH:mm:ss tt}</strong>
                                <br />If you did not request a password reset, please ignore this email or contact our support team.
                                <br />Best regards,
                                <br />MIS Branch</p>
                            </div>
                            <div class='footer'>
                                 {DateTime.Now.Year} Crystal_Clinic Management. All rights reserved.
                            </div>
                        </div>
                    </body>
                    </html>"
            };
            await _mail.SendEmailAsync(emailRequest).ConfigureAwait(false);
            var history = new EmailHistory
            {
                ToEmail = request.Email,
                Subject = emailRequest.Subject,
                Body = emailRequest.Body,
                VerificationCode = verificationCode,
                CreatedOn = DateTime.Now,
                CreatedBy = user.Id
            };
            return await _GRepoEmailHistory.AddAsync(history, cancellationToken);

        }
    }
}
