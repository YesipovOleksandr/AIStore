using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Abstract.Services.Mail;
using AIStore.Domain.Models.Email;
using AIStore.Domain.Models.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AIStore.BLL.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IRazorViewToStringRenderer _renderer;
        public MailService(IOptions<AppSettings> settings, IRazorViewToStringRenderer renderer)
        {
            _mailSettings = settings.Value.MailSettings;
            _renderer = renderer;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailConfirm(EmailConfirm model)
        {
            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = model.Email;
            mailRequest.Subject = "Email Confirm";
            var htmlBody = await _renderer.RenderViewToStringAsync($"{model.ViewName}Html.cshtml", model);
            mailRequest.Body = htmlBody;
            try
            {
                await SendEmailAsync(mailRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}