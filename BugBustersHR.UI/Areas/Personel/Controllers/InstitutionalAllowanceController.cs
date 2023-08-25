using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BugBustersHR.BLL.Options;
using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices;
using BugBustersHR.BLL.Services.Concrete.ExpenditureConcreteServices;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]

    public class InstitutionalAllowanceController : Controller
    {
        private readonly IInstitutionalAllowanceService _ınstitutionalAllowanceService;
        private readonly IInstitutionalAllowanceTypeService _ınstitutionalAllowanceTypeService;
        private readonly IMapper _mapper;
        private readonly HrDb _hrDb;
        private readonly IValidator<InstitutionalAllowanceVM> _requestValidator;

        public InstitutionalAllowanceController(IInstitutionalAllowanceService ınstitutionalAllowanceService, IInstitutionalAllowanceTypeService ınstitutionalAllowanceTypeService, IMapper mapper, HrDb hrDb, IValidator<InstitutionalAllowanceVM> requestValidator)
        {
            _ınstitutionalAllowanceService = ınstitutionalAllowanceService;
            _ınstitutionalAllowanceTypeService = ınstitutionalAllowanceTypeService;
            _mapper = mapper;
            _hrDb = hrDb;
            _requestValidator = requestValidator;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            var query = _ınstitutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.EmployeeId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.TypeName = (_ınstitutionalAllowanceTypeService.GetByIdInstitutionalAllowanceType(item.InstitutionalAllowanceTypeId)).InstitutionalAllowanceTypeName;
            }
            foreach (var item in mappingQuery) _ınstitutionalAllowanceService.GetInstAllApprovelName(item);

            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(mappingQuery);
        }


        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            ViewData["InstitutionalAllowanceTypeId"] = new SelectList(_hrDb.InstitutionalAllowanceTypes, "Id", "InstitutionalAllowanceTypeName");
            ViewBag.CurrencyList = Enum.GetValues(typeof(Currency))
                .Cast<Currency>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                });

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Currency,ApprovalStatus,InstitutionalAllowanceTypeId,AmountOfAllowance")] InstitutionalAllowanceVM request)
        {
            // Kullanıcının bekleyen talebini kontrol et
            var hasPendingRequest = _ınstitutionalAllowanceService.GetAllInstitutionalAllowances()
                .Any(x => x.EmployeeId == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.ApprovalStatus == null);

            if (hasPendingRequest)
            {
                ModelState.AddModelError("", "You already have a pending allowance request. Please wait for its approval.");
            }
            else
            {
                var institutionalAllowanceType = _ınstitutionalAllowanceTypeService.GetByIdInstitutionalAllowanceType(request.InstitutionalAllowanceTypeId);

                if (institutionalAllowanceType != null)
                {
                    var minAmount = institutionalAllowanceType.MinPrice ?? 0; // Varsayılan olarak 0 kabul edebiliriz
                    var maxAmount = institutionalAllowanceType.MaxPrice ?? decimal.MaxValue; // Varsayılan olarak en büyük değeri kabul edebiliriz

                    if (request.AmountOfAllowance >= minAmount && request.AmountOfAllowance <= maxAmount)
                    {
                        // Değer uygun, kayıt işlemini yapabilirsiniz
                        request.EmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        var mappingQuery = _mapper.Map<InstitutionalAllowance>(request);
                        await _hrDb.AddAsync(mappingQuery);
                        await _hrDb.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("AmountOfAllowance", "Amount must be between " + minAmount + " and " + maxAmount + ".");
                    }
                }
            }

            // Hatalı giriş veya bekleyen talep olduğunda sayfayı tekrar yükle
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewData["InstitutionalAllowanceTypeId"] = new SelectList(_hrDb.InstitutionalAllowanceTypes, "Id", "InstitutionalAllowanceTypeName", request.InstitutionalAllowanceTypeId);
            ViewBag.CurrencyList = Enum.GetValues(typeof(Currency))
                .Cast<Currency>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                });

            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            return View(request);
        }


        public IActionResult Delete(int id)
        {

            var request = _ınstitutionalAllowanceService.GetByIdInstitutionalAllowance(id);
            var mapli = _mapper.Map<InstitutionalAllowanceVM>(request);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(InstitutionalAllowanceVM requestVm)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            _ınstitutionalAllowanceService.TDelete(_mapper.Map<InstitutionalAllowance>(requestVm));
            return RedirectToAction("Index");

        }


        public IActionResult NotConfirmedForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            var query = _ınstitutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.EmployeeId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.TypeName = (_ınstitutionalAllowanceTypeService.GetByIdInstitutionalAllowanceType(item.InstitutionalAllowanceTypeId)).InstitutionalAllowanceTypeName;
            }
            foreach (var item in mappingQuery) _ınstitutionalAllowanceService.GetInstAllApprovelName(item);

            var notConfirmedApprovalExp = mappingQuery.Where(item => item.ApprovalStatus == false);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(notConfirmedApprovalExp);


        }


        public IActionResult ConfirmedForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            var query = _ınstitutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.EmployeeId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.TypeName = (_ınstitutionalAllowanceTypeService.GetByIdInstitutionalAllowanceType(item.InstitutionalAllowanceTypeId)).InstitutionalAllowanceTypeName;
            }
            foreach (var item in mappingQuery) _ınstitutionalAllowanceService.GetInstAllApprovelName(item);

            var confirmedForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == true);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(confirmedForApprovalexp);

            //yorummm
        }


        public IActionResult WaitingForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            var query = _ınstitutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.EmployeeId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.TypeName = (_ınstitutionalAllowanceTypeService.GetByIdInstitutionalAllowanceType(item.InstitutionalAllowanceTypeId)).InstitutionalAllowanceTypeName;
            }
            foreach (var item in mappingQuery) _ınstitutionalAllowanceService.GetInstAllApprovelName(item);

            var waitingForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == null);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(waitingForApprovalexp);


        }


    }
}
