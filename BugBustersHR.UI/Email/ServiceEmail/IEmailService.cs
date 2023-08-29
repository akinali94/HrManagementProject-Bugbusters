using BugBustersHR.ENTITY.Concrete;

namespace BugBustersHR.UI.Email.ServiceEmail
{
    public interface IEmailService
    {

        Task SendConfirmEmail(string emailLink, string ToEmail, string Password);
        Task RequestApprovedMail(string emailLink, string ToEmail, string request);
        Task LeaveRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, EmployeeLeaveRequest request);
        Task AdvanceRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, IndividualAdvance request);
        Task AllowanceRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, InstitutionalAllowance request);
        Task ExpenditureRequestApprovedMail(string ToEmail, string approvedStatus, string employeeFullName, Employee manager, ExpenditureRequest request);

    }
}
