using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.InstitutionalAllowanceValidations
{
    public class InstitutionalAllowanceValidator : AbstractValidator<InstitutionalAllowanceVM>
    {
        public InstitutionalAllowanceValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Please Enter Name..")
            .MaximumLength(255).WithMessage("Maximum character 255");

            RuleFor(x => x.Currency)
               .NotEmpty().WithMessage("Please Enter Currency")
               .NotNull().WithMessage("Please Enter Currency");

            RuleFor(x => x.InstitutionalAllowanceTypeId)
              .NotEmpty().WithMessage("Please select an Allowance Type.")
              .NotNull().WithMessage("Please select an Allowance Type.");

            RuleFor(x => x.AmountOfAllowance).NotEmpty().WithMessage("Please Enter Amount of Allowance");
        }
    }
}
