using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.ExpanditureValidations
{
    public class ExpanditureTypeValidator : AbstractValidator<ExpenditureTypeCreateVM>
    {
        public ExpanditureTypeValidator()
        {
            RuleFor(x => x.ExpenditureName).
         NotEmpty().WithMessage("Please enter name..").
         NotNull().WithMessage("Please enter name..");
        }
    }
}
