using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using FluentValidation;
using System.Globalization;

namespace BugBustersHR.BLL.Validatons.ExpanditureValidations
{
    public class ExpenditureRequestValidator : AbstractValidator<ExpenditureRequestVM>
    {
        public ExpenditureRequestValidator(IExpenditureTypeService typeService)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Please Enter Name..")
                .MaximumLength(255).WithMessage("Maximum character 255");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Please Enter Currency")
                .NotNull().WithMessage("Please Enter Currency");

            RuleFor(x => x.ExpenditureTypeId)
                .NotEmpty().WithMessage("Please select an Expenditure Type.")
                .NotNull().WithMessage("Please select an Expenditure Type.");

            RuleFor(x => x.ImageUrl).Must(IsValidImageUrl).WithMessage("Only JPG, PNG, WEBP, JPEG, PDF is allowed.");

            RuleFor(x => x.AmountOfExpenditure)
                .NotEmpty().WithMessage("Please Enter Amount")
                .Must((request, amount) =>
                {
                    if (request.ExpenditureTypeId == null)
                    {
                        return true; // ExpenditureTypeId null ise kuralı geçersiz sayma
                    }

                    // ExpenditureTypeId kullanarak ilgili ExpenditureType'ı çekin
                    var expenditureType = typeService.GetByIdExpenditureType(request.ExpenditureTypeId);

                    if (expenditureType == null)
                    {
                        return true; // ExpenditureType bulunamazsa kuralı geçersiz sayma
                    }

                    // AmountOfExpenditure, ExpenditureType'ın MinPrice ve MaxPrice aralığında mı kontrol edin
                    return amount >= expenditureType.MinPrice && amount <= expenditureType.MaxPrice;
                })
                .WithMessage(request => $"Amount must be within the Mininum ({GetMinPrice(typeService, request.ExpenditureTypeId).ToString("C", CultureInfo.CurrentCulture)}) and Maximum ({GetMaxPrice(typeService, request.ExpenditureTypeId).ToString("C", CultureInfo.CurrentCulture)}) Price of the Related Expenditure Name");
        }

        private bool IsValidImageUrl(string imageUrl)
        {
            return imageUrl == null || (imageUrl.EndsWith(".pdf") || imageUrl.EndsWith(".jpeg") || imageUrl.EndsWith(".jpg") || imageUrl.EndsWith(".png") || imageUrl.EndsWith(".webp"));
        }

        private decimal GetMinPrice(IExpenditureTypeService typeService, int expenditureTypeId)
        {
            var expenditureType = typeService.GetByIdExpenditureType(expenditureTypeId);
            return expenditureType?.MinPrice ?? 0;
        }

        private decimal GetMaxPrice(IExpenditureTypeService typeService, int expenditureTypeId)
        {
            var expenditureType = typeService.GetByIdExpenditureType(expenditureTypeId);
            return expenditureType?.MaxPrice ?? 0;
        }
    }
}

