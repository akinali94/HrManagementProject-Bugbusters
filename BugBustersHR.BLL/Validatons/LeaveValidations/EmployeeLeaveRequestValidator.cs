using BugBustersHR.BLL.Services.Abstract.LeaveAbstractService;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.LeaveValidations
{
    public class EmployeeLeaveRequestValidator : AbstractValidator<EmployeeLeaveRequestVM>
    {
        public EmployeeLeaveRequestValidator(IEmployeeLeaveTypeService _typeService)
        {
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Please enter Start date")
                .NotNull().WithMessage("Please enter Start date")
                .LessThan(x => x.EndDate).WithMessage("Start date must be before End date")
                .GreaterThan(DateTime.Now).WithMessage("Start date cannot be less than today")
                .LessThan(DateTime.Now.AddYears(1)).WithMessage("Start date cannot be more than 1 year in the future");


            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Please enter End date")
                .NotNull().WithMessage("Please enter End date");

            RuleFor(x => x.StartDate).GreaterThan(DateTime.Now).WithMessage("Start date cannot be less than today");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.StartDate == null || request.EndDate == null)
                {
                    return;
                }

                var leavetype = _typeService.GetByIdType(request.SelectedLeaveTypeId);

                if (leavetype == null)
                {
                    return;
                }

                var dayDifference = (request.EndDate - request.StartDate).Days + 1;

                if (dayDifference > leavetype.DefaultDay)
                {
                    context.AddFailure($"Leave period cannot be greater than {leavetype.DefaultDay} days");
                }
            });
        }
    }
}

