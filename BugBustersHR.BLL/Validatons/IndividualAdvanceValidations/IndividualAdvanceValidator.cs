using BugBustersHR.BLL.Services.Abstract.IndividualAdvanceService;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.IndividualAdvanceValidator
{
    public class IndividualAdvanceValidator:AbstractValidator<IndividualAdvanceRequestVM>
    {
        public IndividualAdvanceValidator(IIndividualAdvanceService _service)
        {
          
            
            RuleFor(request => request.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0");
            RuleFor(request => request.Currency).NotNull().WithMessage("Currency is required.");
            RuleFor(request => request.Explanation).MaximumLength(500).WithMessage("Explanation can be at most 500 characters.");
       
         
          
           

        }
    }
}
