using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BugBustersHR.BLL.Options;
using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.Validatons.ExpanditureValidations;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]
    public class ExpenditureRequestController : Controller
    {
        private readonly IExpenditureRequestService _expenditureRequestService;
        private readonly IExpenditureTypeService _typeService;
        private readonly IMapper _mapper;
        private readonly HrDb _hrDb;
        private readonly IValidator<ExpenditureRequestVM> _requestValidator;
        private readonly AzureOptions _azureOptions;


        public ExpenditureRequestController(IOptions<AzureOptions> azureOptions, IExpenditureRequestService expenditureRequestService, IMapper mapper, HrDb hrDb, IExpenditureTypeService typeService, IValidator<ExpenditureRequestVM> requestValidator)
        {
            _expenditureRequestService = expenditureRequestService;
            _mapper = mapper;
            _hrDb = hrDb;
            _typeService = typeService;
            _requestValidator = requestValidator;
            _azureOptions = azureOptions.Value;
        }

        public IActionResult Index()
        {

            var query = _expenditureRequestService.GetAllExReq().Where(x => x.EmployeeId == GetEmployee().Id);
            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(query);
            _expenditureRequestService.GetExpenditureTypeName(mappingQuery);
            _expenditureRequestService.GetExpenditureApprovelName(mappingQuery);
                   
            SetUserImageViewBag();
            return View(mappingQuery);
        }

        public IActionResult Create()
        {
            SetUserImageViewBag();
            GetExpenditureTypes();
            GetCurrencyType();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Currency,ApprovalStatus,RequestDate,ExpenditureTypeId,AmountOfExpenditure,ImageModel,ImageModel.File")] ExpenditureRequestVM expenditureRequest)
        {
            //ModelState.Clear();

            GetExpenditureTypes();
            GetCurrencyType();

            var existingPendingRequest = _expenditureRequestService.GetAllExReq().FirstOrDefault(x => x.EmployeeId == GetEmployee().Id && x.ApprovalStatus == null);

            if (existingPendingRequest != null)
            {
                ModelState.AddModelError("", "You already have a pending request. Please wait for its approval.");

                ViewData["ExpentitureTypeId"] = new SelectList(_hrDb.ExpenditureTypes, "Id", "ExpenditureName", expenditureRequest.ExpenditureTypeId);
                GetCurrencyType();
                SetUserImageViewBag();
                return View(expenditureRequest);


            }

            var expenditureType = _typeService.GetByIdExpenditureType(expenditureRequest.ExpenditureTypeId);

            if (expenditureType == null)
            {
                ModelState.AddModelError("ExpenditureTypeId", "Please select a valid Expenditure Type.");
            }
            else
            {
                var minPrice = expenditureType.MinPrice;
                var maxPrice = expenditureType.MaxPrice;
                var amountOfExpenditure = expenditureRequest.AmountOfExpenditure;

                if (amountOfExpenditure < minPrice || amountOfExpenditure > maxPrice)
                {
                    ModelState.AddModelError("AmountOfExpenditure", $"Amount must be within the valid range for the selected Expenditure Type ({minPrice} - {maxPrice}).");
                }
                else
                {
                    try
                    {
                        if (expenditureRequest.ImageModel.File != null)
                        {
                            string fileExtension = Path.GetExtension(expenditureRequest.ImageModel.File.FileName);
                            var uniqueName = Guid.NewGuid().ToString() + fileExtension;

                            using (MemoryStream fileUploadStream = new MemoryStream())
                            {
                                expenditureRequest.ImageModel.File.CopyTo(fileUploadStream);
                                fileUploadStream.Position = 0;

                                BlobContainerClient blobContainerClient = new BlobContainerClient(_azureOptions.ConnectionString, _azureOptions.Container);
                                BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueName);

                                blobClient.Upload(fileUploadStream, new BlobHttpHeaders
                                {
                                    ContentType = expenditureRequest.ImageModel.File.ContentType,
                                });

                                expenditureRequest.ImageUrl = "https://bugbustersstorage.blob.core.windows.net/contentupload/" + uniqueName;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    var validationResults2 = _requestValidator.Validate(expenditureRequest);

                    if (validationResults2.IsValid)
                    {
                        expenditureRequest.EmployeeId = userId;
                        var mappingQuery = _mapper.Map<ExpenditureRequest>(expenditureRequest);
                        await _hrDb.AddAsync(mappingQuery);
                        await _hrDb.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var item in validationResults2.Errors)
                        {
                            ModelState.AddModelError("", item.ErrorMessage);
                        }
                    }
                }
            }

            ViewData["ExpentitureTypeId"] = new SelectList(_hrDb.ExpenditureTypes, "Id", "ExpenditureName", expenditureRequest.ExpenditureTypeId);
            ViewBag.CurrencyList = Enum.GetValues(typeof(Currency))
                        .Cast<Currency>()
                        .Select(c => new SelectListItem
                        {
                            Value = c.ToString(),
                            Text = c.ToString()
                        });
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();
            return View(expenditureRequest);
        }
        public IActionResult Delete(int id)
        {

            var request = _expenditureRequestService.GetByIdExpenditureRequest(id);

            var mapli = _mapper.Map<ExpenditureRequestVM>(request);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();
            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(ExpenditureRequestVM requestVm)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();

            _expenditureRequestService.TDelete(_mapper.Map<ExpenditureRequest>(requestVm));
            return RedirectToAction("Index");

        }
        public IActionResult WaitingForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _expenditureRequestService.GetAllExReq().Where(x => x.EmployeeId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.TypeName = (_typeService.GetByIdExpenditureType(item.ExpenditureTypeId)).ExpenditureName;
            }
           _expenditureRequestService.GetExpenditureApprovelName(mappingQuery);

            var waitingForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == null);
            SetUserImageViewBag();
            return View(waitingForApprovalexp);

        }


        public IActionResult ConfirmedForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _expenditureRequestService.GetAllExReq().Where(x => x.EmployeeId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.TypeName = (_typeService.GetByIdExpenditureType(item.ExpenditureTypeId)).ExpenditureName;
            }

          _expenditureRequestService.GetExpenditureApprovelName(mappingQuery);

            var confirmedForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == true);
            SetUserImageViewBag();
            return View(confirmedForApprovalexp);

        }


        public IActionResult NotConfirmedForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _expenditureRequestService.GetAllExReq().Where(x => x.EmployeeId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.TypeName = (_typeService.GetByIdExpenditureType(item.ExpenditureTypeId)).ExpenditureName;
            }



                _expenditureRequestService.GetExpenditureApprovelName(mappingQuery);


            var notConfirmedApprovalExp = mappingQuery.Where(item => item.ApprovalStatus == false);
            SetUserImageViewBag();
            return View(notConfirmedApprovalExp);


        }
        [NonAction]
        private void SetUserImageViewBag()
        {
            var qury2 = _expenditureRequestService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.UserImageUrl = qury2?.ImageUrl;
            ViewBag.UserFullName = qury2?.FullName;

        }
        [NonAction]
        private void GetCurrencyType()
        {
            ViewBag.CurrencyList = Enum.GetValues(typeof(Currency))
                .Cast<Currency>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                });
        }
        [NonAction]
        private Employee GetEmployee()
        {
            return _expenditureRequestService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        [NonAction]
        private void GetExpenditureTypes()
        {
            ViewData["ExpentitureTypeId"] = new SelectList(_hrDb.ExpenditureTypes, "Id", "ExpenditureName");
        }
    }
}
