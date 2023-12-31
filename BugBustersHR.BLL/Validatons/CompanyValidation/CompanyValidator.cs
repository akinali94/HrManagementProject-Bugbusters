﻿using BugBustersHR.BLL.Services.Abstract.CompanyService;
using BugBustersHR.BLL.Services.Concrete.CompanyService;
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
        private readonly ICompanyService _companyService;
        public CompanyValidator(ICompanyService companyService)
        {
            _companyService = companyService;

            //RuleFor(x => x.CompanyName).
            //    Must((viewModel, companyName) => !IsDuplicate(viewModel.CompanyName)).
            //    WithMessage("Same company name is founded in database");
            RuleFor(x => x.CompanyName)
    .NotEmpty().WithMessage("Please enter company name..")
    .NotNull().WithMessage("Please enter company name..")
    .Must((viewModel, companyName) => IsUniqueCompanyName(viewModel.Id, companyName))
    .WithMessage("A company with the same name already exists in the database.");


            RuleFor(x => x.TelephoneNumber).
                NotEmpty().WithMessage("Please enter phone number..").
                NotNull().WithMessage("Please enter phone number..").
                Matches(@"^[0-9\s\(\)]+$").
                WithMessage("Phone number should only have numbers..").
                Length(11).WithMessage("Phone number must have 11 digits..").
                Must(x => x.StartsWith("05")).WithMessage("Phone number should start with '05'...");


            RuleFor(x => x.Address).
                NotEmpty().WithMessage("Please enter address information..").
                NotNull().WithMessage("Please enter address information..").
                When(x => !string.IsNullOrWhiteSpace(x.Address));


            RuleFor(x => x.MersisNo).
                NotEmpty().WithMessage("Please enter Mersis No..").
                NotNull().WithMessage("Please enter Mersis No..").
                Matches(@"^[0-9\s\(\)]+$").
                WithMessage("Mersis No should only have numbers..").
                Length(16).WithMessage("Mersis No must have 16 digits..");

            RuleFor(x => x.TaxNumber).
                NotEmpty().WithMessage("Please enter Tax No..").
                NotNull().WithMessage("Please enter Tax No..").
                Matches(@"^[0-9\s\(\)]+$").
                WithMessage("Mersis No should only have numbers..").
                Length(10).WithMessage("Tax No must have 10 digits..");

            RuleFor(x => x.MersisNo).
                Must((viewModel, mersisNo) => IncludeTax(mersisNo, viewModel.TaxNumber)).
                WithMessage("Mersis No should include tax number");

            RuleFor(vm => vm.FoundationYear)
           .Must((vm, foundationYear) => foundationYear < vm.ContractStartDate)
           .WithMessage("Foundation year must be earlier than contract start date");

            RuleFor(vm => vm.ContractStartDate)
                .LessThan(vm => vm.ContractEndDate)
                .WithMessage("Contract start date must be earlier than contract end date");
        }

        //public CompanyValidator()
        //{


            //    RuleFor(x => x.TelephoneNumber).
            //        NotEmpty().WithMessage("Please enter phone number..").
            //        NotNull().WithMessage("Please enter phone number..").
            //        Matches(@"^[0-9\s\(\)]+$").
            //        WithMessage("Phone number should only have numbers..").
            //        Length(11).WithMessage("Phone number must have 11 digits..");

            //    RuleFor(x => x.Address).
            //        NotEmpty().WithMessage("Please enter address information..").
            //        NotNull().WithMessage("Please enter address information..").
            //        When(x => !string.IsNullOrWhiteSpace(x.Address));


            //    RuleFor(x => x.MersisNo).
            //        NotEmpty().WithMessage("Please enter Mersis No..").
            //        NotNull().WithMessage("Please enter Mersis No..").
            //        Matches(@"^[0-9\s\(\)]+$").
            //        WithMessage("Mersis No should only have numbers..").
            //        Length(16).WithMessage("Mersis No must have 16 digits..");

            //    RuleFor(x => x.TaxNumber).
            //        NotEmpty().WithMessage("Please enter Tax No..").
            //        NotNull().WithMessage("Please enter Tax No..").
            //        Matches(@"^[0-9\s\(\)]+$").
            //        WithMessage("Mersis No should only have numbers..").
            //        Length(10).WithMessage("Tax No must have 10 digits..");

            //    RuleFor(x => x.MersisNo).
            //        Must((viewModel, mersisNo) => IncludeTax(mersisNo, viewModel.TaxNumber)).
            //        WithMessage("Mersis No should include tax number");
        //}

        private bool IncludeTax(string mersisNo, string taxNo)
        {
            if (taxNo.Length == 10)
            {
                int count = 0;

                for (int i = 0; i < taxNo.Length; i++)
                {

                    if (mersisNo[i + 1] == taxNo[i])
                    {
                        count++;
                    }

                }
                if (count == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            { return false; }

        }

        //Şöyle bir metot yazdım. Ama problem Dependency yapmamız laızm
        //private bool IsDuplicate(string cVM)
        //{
        //    var companies = _companyService.GetAllCompany().ToList();
        //    return companies.Any(x => x.CompanyName == cVM);

        //}
        private bool IsUniqueCompanyName(int companyId, string companyName)
        {
            var existingCompany = _companyService.GetByIdCompany(companyId);

            if (existingCompany == null)
            {
                // Company not found, so it's unique
                return true;
            }

            var companiesWithSameName = _companyService.GetAllCompany()
                .Where(c => c.Id != companyId && c.CompanyName == companyName)
                .ToList();

            return companiesWithSameName.Count == 0;
        }

    }
}
