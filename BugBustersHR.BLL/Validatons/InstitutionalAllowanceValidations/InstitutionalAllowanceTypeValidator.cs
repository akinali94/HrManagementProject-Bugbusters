using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceTypeViewModel;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.InstitutionalAllowanceValidations
{
    public class InstitutionalAllowanceTypeValidator : AbstractValidator<InstitutionalAllowanceTypeVM>
    {

        public InstitutionalAllowanceTypeValidator()
        {
            RuleFor(x => x.InstitutionalAllowanceTypeName)
                .NotEmpty().WithMessage("Please Enter Name..")
                .NotNull().WithMessage("Please Enter Name..");
            RuleFor(x=>x.MaxPrice)
                .NotEmpty().WithMessage("Please Enter Max Price..")
                .NotNull().WithMessage("Please Enter Max Price..");
        }
    }
}
