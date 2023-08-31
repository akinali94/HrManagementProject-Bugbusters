using BugBustersHR.BLL.Utilities.Price;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ManagerViewModel;
using FluentValidation;
using FluentValidation.Results;
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
             Length(11).WithMessage("Phone number must have 11 digits..").
             Must(x => x.StartsWith("05")).WithMessage("Phone number should start with '05'...");

            RuleFor(x => x.Address).
            NotEmpty().WithMessage("Please enter address information..").
            NotNull().WithMessage("Please enter address information..").
            When(x => !string.IsNullOrWhiteSpace(x.Address));


            RuleFor(x => x.BirthDate)
              .Must((viewModel, birthDate) => IsValidAge(birthDate))
              .WithMessage("Age must be between 18 and 119.");

            RuleFor(x => x.HiredDate)
             .Must((viewModel, xy) =>
             {
                 var today = DateTime.Today;
                 var minStartDate = today.AddDays(7); // Şuandan 1 hafta sonrası
                 var maxStartDate = today.AddDays(-7); // Şuandan 1 hafta öncesi

                 return xy >= maxStartDate && xy.Date <= minStartDate;
             }).WithMessage("Hired Date must be within at most 1 week before or after..");

            RuleFor(x => x.Name).
            NotEmpty().WithMessage("Please enter name.").
            NotNull().WithMessage("Please enter name.").
            //Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ]+$").WithMessage("Name can only contain letters.").
            MaximumLength(30).WithMessage("The name cannot exceed 30 characters.");

            RuleFor(x => x.Surname).
            NotEmpty().WithMessage("Please enter name.").
            NotNull().WithMessage("Please enter name.").
            //Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ]+$").WithMessage("Name can only contain letters.").
            MaximumLength(30).WithMessage("The name cannot exceed 30 characters.");

            RuleFor(x => x.SecondName).
            //Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ]*$").WithMessage("SecondName can only contain letters.").
            MaximumLength(30).WithMessage("The SecondName cannot exceed 30 characters.");

            RuleFor(x => x.SecondSurname).
            //Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ]*$").WithMessage("SecondName can only contain letters.").
            MaximumLength(30).WithMessage("The SecondName cannot exceed 30 characters.");

            RuleFor(x => x.BirthPlace).
            NotEmpty().WithMessage("Please enter BirthPlace..").
            NotNull().WithMessage("Please enter BirthPlace..").
            MaximumLength(50).WithMessage("The BirthPlace cannot exceed 50 characters..");

            RuleFor(x => x.TC)
             .NotEmpty().WithMessage("Please enter TC.")
             .NotNull().WithMessage("Please enter TC.")
             .Must(BeValidTC).WithMessage("Invalid TC identification number.")
             .Length(11).WithMessage("TC must have 11 digits.");

            RuleFor(x => x.Section)
                .NotEmpty().WithMessage("Please enter Section..")
                .NotNull().WithMessage("Please enter Section..")
                .MaximumLength(50).WithMessage("The Section cannot exceed 50 characters..");

            RuleFor(x => x.Salary).
            Must(salary => salary > 11407)
            .WithMessage("Salary must be grater than Minimum Wage..");

        }

        private bool BeValidTC(string tc)
        {
            if (tc == null) return false;

            if (tc.Length != 11 || !IsAllDigits(tc))
            {
                return false;
            }

            int[] digits = tc.Select(c => int.Parse(c.ToString())).ToArray();

            if (digits[0] == 0 || digits.Take(10).All(d => d == digits[0]))
            {
                return false;
            }

            int oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int evenSum = digits[1] + digits[3] + digits[5] + digits[7];
            int tenthDigit = (oddSum * 7 - evenSum) % 10;
            int eleventhDigit = digits.Take(10).Sum() % 10;

            if (digits[9] != tenthDigit || digits[10] != eleventhDigit)
            {
                return false;
            }

            return true;
        }

        private bool IsValidAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {
                age--; // Eğer doğum günü bu yıl henüz gelmemişse yaş bir azaltılmalı
            }

            return age >= 18 && age < 120;
        }



        private static bool IsAllDigits(string str)
        {
            return str.All(char.IsDigit);
        }
    }
}

