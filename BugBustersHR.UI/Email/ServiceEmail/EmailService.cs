using BugBustersHR.UI.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text.Encodings.Web;
using BugBustersHR.ENTITY.Concrete;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Hosting;

namespace BugBustersHR.UI.Email.ServiceEmail
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _settings;
        private readonly IWebHostEnvironment _env;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;

        public EmailService(IOptions<EmailSettings> options, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment)
        {
            _settings = options.Value;
            _env = env;
            _hostEnvironment = hostEnvironment;
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
            // string htmlFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "EmailTemplate", "index.html");
            string htmlFilePath = Path.Combine(_hostEnvironment.WebRootPath, "assets", "mailconfirmation", "html", "emailconfirmation.html");
            string emailTemplate = File.ReadAllText(htmlFilePath);
            //string emailTemplate = File.ReadAllText("\\assets\\mailconfirmation\\html\\emailconfirmation.html");

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
            string wwwrootPath = _hostEnvironment.WebRootPath;
            //string emailTemplate = File.ReadAllText("C:\\Users\\cagri\\Desktop\\dddddddd\\BugBustersFinall\\BugBustersHR.UI\\wwwroot\\assets\\mailconfirmation\\html\\leavemail.html");
            string emailTemplate = Path.Combine(wwwrootPath, "C:\\Users\\cagri\\Desktop\\dddddddd\\BugBustersFinall\\BugBustersHR.UI\\wwwroot\\assets\\mailconfirmation\\html\\leavemail.html");
            emailTemplate = emailTemplate.Replace("{DateTime.Now.Year}", DateTime.Now.Year.ToString());
            emailTemplate = emailTemplate.Replace("{employeeFullName}", employeeFullName);
            emailTemplate = emailTemplate.Replace("{approvedStatus}", approvedStatus);
            emailTemplate = emailTemplate.Replace("{manager.FullName}", manager.FullName);
            emailTemplate = emailTemplate.Replace("{request.StartDate.ToString(\"yyyy-MM-dd\")}", request.StartDate.ToString("yyyy-MM-dd"));
            emailTemplate = emailTemplate.Replace("{request.EndDate.ToString(\"yyyy-MM-dd\")}", request.EndDate.ToString("yyyy-MM-dd"));
             

            mailMessage.From = new MailAddress(_settings.Username);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Your request has been answered.";
            mailMessage.Body = emailTemplate;

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
            string emailTemplate = File.ReadAllText("C:\\Users\\cagri\\Desktop\\dddddddd\\BugBustersFinall\\BugBustersHR.UI\\wwwroot\\assets\\mailconfirmation\\html\\advancemail.html");
            emailTemplate = emailTemplate.Replace("{DateTime.Now.Year}", DateTime.Now.Year.ToString());
            emailTemplate = emailTemplate.Replace("{employeeFullName}", employeeFullName);
            emailTemplate = emailTemplate.Replace("{approvedStatus}", approvedStatus);
            emailTemplate = emailTemplate.Replace("{manager.FullName}", manager.FullName);
            emailTemplate = emailTemplate.Replace("{request.Amount}", request.Amount.ToString());
            emailTemplate = emailTemplate.Replace("{request.RequestDate}", request.RequestDate.ToString("yyyy-MM-dd"));



            mailMessage.From = new MailAddress(_settings.Username);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Your request has been answered.";
            mailMessage.Body = emailTemplate;

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }



        //--------------------------------------------------------------------------
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
            string emailTemplate = File.ReadAllText("C:\\Users\\cagri\\Desktop\\dddddddd\\BugBustersFinall\\BugBustersHR.UI\\wwwroot\\assets\\mailconfirmation\\html\\allowanceMail.html");

            emailTemplate = emailTemplate.Replace("{DateTime.Now.Year}", DateTime.Now.Year.ToString());
            emailTemplate = emailTemplate.Replace("{employeeFullName}", employeeFullName);
            emailTemplate = emailTemplate.Replace("{approvedStatus}", approvedStatus);
            emailTemplate = emailTemplate.Replace("{manager.FullName}", manager.FullName);
            emailTemplate = emailTemplate.Replace("{request.AmountOfAllowance}", request.AmountOfAllowance.ToString());
            emailTemplate = emailTemplate.Replace("{request.Currency.Value}", request.Currency.Value.ToString());
            emailTemplate = emailTemplate.Replace("{request.RequestDate}", request.RequestDate.ToString("yyyy-MM-dd"));



            mailMessage.From = new MailAddress(_settings.Username);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Your request has been answered.";
            mailMessage.Body = emailTemplate;

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
            string emailTemplate = File.ReadAllText("C:\\Users\\cagri\\Desktop\\dddddddd\\BugBustersFinall\\BugBustersHR.UI\\wwwroot\\assets\\mailconfirmation\\html\\expendituremail.html");
            emailTemplate = emailTemplate.Replace("{DateTime.Now.Year}", DateTime.Now.Year.ToString());
            emailTemplate = emailTemplate.Replace("{employeeFullName}", employeeFullName);
            emailTemplate = emailTemplate.Replace("{approvedStatus}", approvedStatus);
            emailTemplate = emailTemplate.Replace("{manager.FullName}", manager.FullName);
            emailTemplate = emailTemplate.Replace("{request.AmountOfExpenditure}", request.AmountOfExpenditure.ToString());
            emailTemplate = emailTemplate.Replace("{request.Currency.Value}", request.Currency.Value.ToString());
            emailTemplate = emailTemplate.Replace("{request.RequestDate}", request.RequestDate.ToString("yyyy-MM-dd"));


            mailMessage.From = new MailAddress(_settings.Username);
            mailMessage.To.Add(ToEmail);
            mailMessage.Subject = "Your request has been answered.";
            mailMessage.Body = emailTemplate;

            mailMessage.IsBodyHtml = true;

            await smptClient.SendMailAsync(mailMessage);
        }
    }
}