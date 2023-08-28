using BugBustersHR.UI.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;
using BugBustersHR.ENTITY.Concrete;

namespace BugBustersHR.UI.Email.ServiceEmail
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _settings;
        private readonly IWebHostEnvironment _env;

        public EmailService(IOptions<EmailSettings> options, IWebHostEnvironment env)
        {
            _settings = options.Value;
            _env = env;
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

            string emailTemplate = File.ReadAllText("C:\\Users\\JUVENİS\\Source\\Repos\\BugBustersFinall\\BugBustersHR.UI\\wwwroot\\assets\\mailconfirmation\\html\\emailconfirmation.html");

            emailTemplate = emailTemplate.Replace("{DateTime.Now.Year}", DateTime.Now.Year.ToString());

            // Replace {HtmlEncoder.Default.Encode(emailLink)} with the encoded email link
            emailTemplate = emailTemplate.Replace("{HtmlEncoder.Default.Encode(emailLink)}", HtmlEncoder.Default.Encode(emailLink));

            // Assign the modified content to mailMessage.Body

            mailMessage.From = new MailAddress(_settings.Username);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Confirm your account.!";
           

            mailMessage.Body = emailTemplate;

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

        public async Task LeaveRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, EmployeeLeaveRequest request)
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
            mailMessage.Body = @$"<h3>Hello {employeeFullName}</h3>
                    <h4> Your Leave request has been {approvedStatus} to by your manager {manager.FullName}</h4>

                    <h4>Leave Started Date : {request.StartDate.ToString("yyyy-MM-dd")}</h4>
                    <h4>Leave End Date : {request.EndDate.ToString("yyyy-MM-dd")}</h4>

                    <h3>Please check your system. If there is an issue, please contact your manager. </h3>";

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);

        }

        public async Task AdvanceRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, IndividualAdvance request)
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
            mailMessage.Body = @$"<h3>Hello {employeeFullName}</h3>
                    <h4> Your Advance request has been {approvedStatus} to by your manager {manager.FullName}</h4>

                    <h4>Amount of Advance : {request.Amount}</h4>
                    
                    <h3>Please check your system. If there is an issue, please contact your manager. </h3>";

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }

        public async Task AllowanceRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, InstitutionalAllowance request)
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
            mailMessage.Body = @$"<h3>Hello {employeeFullName}</h3>
                    <h4> Your instituonal allowance request has been {approvedStatus} to by your manager {manager.FullName}</h4>

                    <h4>Amount of Allowance : {request.AmountOfAllowance}</h4>
                    <h4>Currency : {request.Currency.Value}</h4>
    
                    
                    <h3>Please check your system. If there is an issue, please contact your manager. </h3>";

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }

        public async Task ExpenditureRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, ExpenditureRequest request)
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
            mailMessage.Body = @$"<h3>Hello {employeeFullName}</h3>
                    <h4> Your expenditure request has been {approvedStatus} to by your manager {manager.FullName}</h4>

                    <h4>Amount of Expenditure : {request.AmountOfExpenditure}</h4>
                    <h4>Currency : {request.Currency.Value}</h4>
                    
    
                    
                    <h3>Please check your system. If there is an issue, please contact your manager. </h3>";

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }
    }
}