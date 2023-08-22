namespace BugBustersHR.UI.Email.ServiceEmail
{
    public interface IEmailService
    {

        Task SendConfirmEmail(string emailLink, string ToEmail, string Password);
        Task RequestApprovedMail(string emailLink, string ToEmail, string request);
        
    }
}
