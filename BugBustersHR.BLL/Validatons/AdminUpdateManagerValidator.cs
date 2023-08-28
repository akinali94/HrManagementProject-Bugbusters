using BugBustersHR.BLL.ViewModels.AdminViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons
{
    internal class AdminUpdateManagerValidator:AbstractValidator<GetManagerListVM>
    {
        public AdminUpdateManagerValidator()
        {
           
                RuleFor(x => x.TelephoneNumber).
                NotEmpty().WithMessage("Please enter phone number..").
                NotNull().WithMessage("Please enter phone number..").
                Matches(@"^\d+$").WithMessage("Phone number should only have numbers..").
                Length(11).WithMessage("Phone number must have 11 digits..");

                RuleFor(x => x.Address).
                NotEmpty().WithMessage("Please enter address information..").
                NotNull().WithMessage("Please enter address information..");
            
        }
    }
}
