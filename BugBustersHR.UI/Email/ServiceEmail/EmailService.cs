using BugBustersHR.UI.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;

namespace BugBustersHR.UI.Email.ServiceEmail
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendConfirmEmail(string emailLink, string ToEmail, string Password)
        {
            var smptClient = new SmtpClient();

            smptClient.Host = _settings.Host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
            smptClient.EnableSsl = true;


            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_settings.Username);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Confirm your account.!";
            mailMessage.Body = @$"<h4> Your account has been created by your manager. To log in, please use the link below to change your password using this email address. </h4>
                        
                               <p><a href='{HtmlEncoder.Default.Encode(emailLink)}'>clicking here</a> </p>"
                             ;
            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }


        public async Task RequestApprovedMail(string emailLink, string ToEmail, string request)
        {
            var smptClient = new SmtpClient();

            smptClient.Host = _settings.Host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
            smptClient.EnableSsl = true;


            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(_settings.Username);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Your request has been answered.";
            mailMessage.Body = @$"<h4> Your {request} request has been responded to by your manager. Please check your system. If there is an issue, please contact your manager. </h4>";

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }


    }
}