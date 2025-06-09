using Crystal_Clinic_Mgm.Application.Common.Configuration;

namespace Crystal_Clinic_Mgm.Application.Common.Services.IRepositories
{
    public interface IMailRepositoy
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
