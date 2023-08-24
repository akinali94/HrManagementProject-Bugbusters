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
    public class CompanyValidator : AbstractValidator<CompanyVM>
    {
        public CompanyValidator()
        {

        //    RuleFor(x => x.MersisNo).
        //NotEmpty().WithMessage("Please enter Mersis No..").
        //NotNull().WithMessage("Please enter Mersis No..").
        //Matches(@"^[0-9\s\(\)]+$").
        //WithMessage("Mersis No should only have numbers..").
        //Length(16).WithMessage("Mersis No must have 16 digits..");

        //    RuleFor(x => x.TaxNumber).
        //    NotEmpty().WithMessage("Please enter Tax No..").
        //    NotNull().WithMessage("Please enter Tax No..").
        //    Matches(@"^[0-9\s\(\)]+$").
        //   WithMessage("Mersis No should only have numbers..").
        //   Length(10).WithMessage("Tax No must have 10 digits..");


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







            //RuleFor(x => x.MersisNo)
            //  .Must((viewModel, mersisNo) => IncludeTax(mersisNo, viewModel.TaxNumber))
            //  .WithMessage("Mersis No should include tax number");
        }

        //private bool IncludeTax(string mersisNo, string taxNo)
        //{
        //    if ( taxNo.Length==10)
        //    {
        //        int count = 0;

        //        for (int i = 0; i < taxNo.Length; i++)
        //        {

        //            if (mersisNo[i + 1] == taxNo[i])
        //            {
        //                count++;
        //            }

        //        }
        //        if (count == 10)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else 
        //    { return false; }
           
        //}


    }
}
