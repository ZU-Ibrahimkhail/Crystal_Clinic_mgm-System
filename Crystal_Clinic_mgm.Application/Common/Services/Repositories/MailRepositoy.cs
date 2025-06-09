using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Crystal_Clinic_Mgm.Application.Common.Configuration;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.AppConfig;
namespace Crystal_Clinic_Mgm.Application.Common.Services.Repositories;

public class MailRepositoy : IMailRepositoy
{

    public MailRepositoy()
    {
    }
    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(AppConfig.MailAddress);
        email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
        email.Subject = mailRequest.Subject;
        var builder = new BodyBuilder
        {
            HtmlBody = mailRequest.Body
        };
        email.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(AppConfig.MailHost, AppConfig.MailPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(AppConfig.MailAddress, AppConfig.MailPassword);
            await smtp.SendAsync(email);
        }
        catch (Exception ex)
        {
            // Log the exception (log mechanism not shown in this snippet)
            Console.WriteLine($"Error sending email: {ex.Message}");
            throw; // or handle the exception as needed
        }
        finally
        {
            await smtp.DisconnectAsync(true);
            smtp.Dispose();
        }
    }
}
