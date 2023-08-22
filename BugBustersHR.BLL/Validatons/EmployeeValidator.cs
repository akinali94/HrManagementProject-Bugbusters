using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons
{
    public class EmployeeValidator : AbstractValidator<EmployeeUpdateVM>
    {
        public EmployeeValidator()
        {

            // bdcfskjafsa
            RuleFor(x => x.TelephoneNumber).
            NotEmpty().WithMessage("Please enter phone number..").
            NotNull().WithMessage("Please enter phone number..").
            Matches(@"^[0-9\s\(\)]+$").
            WithMessage("Phone number should only have numbers..").
            Length(11).WithMessage("Phone number must have 11 digits..");

            RuleFor(x => x.Address).
            NotEmpty().WithMessage("Please enter address information.").
            NotNull().WithMessage("Please enter address information.").
            When(x => !string.IsNullOrWhiteSpace(x.Address));

        }

    }
}
