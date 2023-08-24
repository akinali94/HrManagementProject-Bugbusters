using BugBustersHR.BLL.ViewModels.CompanyViewModel;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.CompanyValidation
{
    public class CompanyValidator:AbstractValidator<CompanyVM>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.TelephoneNumber).
        NotEmpty().WithMessage("Please enter phone number..").
        NotNull().WithMessage("Please enter phone number..").
        Matches(@"^[0-9\s\(\)]+$").
        WithMessage("Phone number should only have numbers..").
        Length(11).WithMessage("Phone number must have 11 digits..");

            RuleFor(x => x.Address).
                NotEmpty().WithMessage("Please enter address information..").
                NotNull().WithMessage("Please enter address information..").
                When(x => !string.IsNullOrWhiteSpace(x.Address));
        }
        

    }
}
