using BugBustersHR.BLL.Utilities.Price;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ManagerViewModel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Validatons.CreateEnployeeValidations
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeFromManagerVM>
    {

        public CreateEmployeeValidator()
        {

            DateTime minDateOfBirth = DateTime.Now.AddYears(-18);
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


            RuleFor(x => x.BirthDate)
              .Must((viewModel, birthDate) => BeAtLeast18YearsOld(birthDate))
              .WithMessage("Age must be greater than 18..");

            RuleFor(x => x.HiredDate)
             .Must((viewModel, xy) =>
             {
                 var today = DateTime.Today;
                 var minStartDate = today.AddDays(7); // Şuandan 1 hafta sonrası
                 var maxStartDate = today.AddDays(-7); // Şuandan 1 hafta öncesi

                 return xy >= maxStartDate && xy.Date <= minStartDate;
             }).WithMessage("Hired Date must be within at most 1 week before or after..");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Please enter name..")
                .NotNull().WithMessage("Please enter name..")
                .MaximumLength(30).WithMessage("The name cannot exceed 30 characters..");


            RuleFor(x => x.SecondName)
                .MaximumLength(30).WithMessage("The SecondName cannot exceed 30 characters..");

            RuleFor(x => x.SecondSurname)
                .MaximumLength(30).WithMessage("The SecondSurname cannot exceed 30 characters..");

            RuleFor(x => x.BirthPlace)
      .NotEmpty().WithMessage("Please enter BirthPlace..")
      .NotNull().WithMessage("Please enter BirthPlace..")
      .MaximumLength(50).WithMessage("The BirthPlace cannot exceed 50 characters..");

            RuleFor(x => x.TC)
             .NotEmpty().WithMessage("Please enter TC..")
             .NotNull().WithMessage("Please enter TC..").
          Matches(@"^[0-9\s\(\)]+$").
          WithMessage("TC should only have numbers..").
          Length(11).WithMessage("TC must have 11 digits..");

            RuleFor(x => x.Section)
                .NotEmpty().WithMessage("Please enter Section..")
                .NotNull().WithMessage("Please enter Section..")
                .MaximumLength(50).WithMessage("The Section cannot exceed 50 characters..");

            RuleFor(x => x.Salary)
            .GreaterThan(PriceUtilities.minimumWage)
            .WithMessage("Salary must be grater than Minimum Wage..");

        }

        private bool BeAtLeast18YearsOld(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;

            return age >= 18 && birthDate.Date.Year > 1900;
        }


    }
}
