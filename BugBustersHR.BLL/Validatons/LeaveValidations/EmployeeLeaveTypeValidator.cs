using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.LeaveValidations
{
    public class EmployeeLeaveTypeValidator : AbstractValidator<EmployeeLeaveTypeVM>
    {
        public EmployeeLeaveTypeValidator()
        {
            RuleFor(x => x.Name).
        NotEmpty().WithMessage("Please enter name..").
        NotNull().WithMessage("Please enter name..");

        }
    }

}
