using AutoMapper;
using Azure.Core;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.IndividualAdvanceService;
using BugBustersHR.BLL.Services.Abstract.LeaveAbstractService;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using BugBustersHR.UI.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]
    public class IndividualAdvancesesController : Controller
    {

        private readonly IIndividualAdvanceService _ındividualAdvanceRequestService;

        private readonly IndividualAdvanceRequestVM _ındividualAdvanceRequestVM;
        private readonly IMapper _mapper;
        private readonly IValidator<IndividualAdvanceRequestVM> _validator;
        private readonly HrDb _hrDb;
        private readonly IEmployeeRepository _employeeRepository;
        public IndividualAdvancesesController(IIndividualAdvanceService ındividualAdvanceRequestService, IEmployeeRepository employeeRepository, IndividualAdvanceRequestVM ındividualAdvanceRequestVM, IMapper mapper, IValidator<IndividualAdvanceRequestVM> validator, HrDb hrDb)
        {
            _ındividualAdvanceRequestService = ındividualAdvanceRequestService;
            _ındividualAdvanceRequestVM = ındividualAdvanceRequestVM;
            _mapper = mapper;
            _validator = validator;
            _hrDb = hrDb;
            _employeeRepository = employeeRepository;
        }
        [NonAction]
        private void SetUserImageViewBag()
        {
            var qury2 = _ındividualAdvanceRequestService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.UserImageUrl = qury2?.ImageUrl;
            ViewBag.UserFullName = qury2?.FullName;

        }
        [NonAction]
        private Employee GetEmployee()
        {
            return _ındividualAdvanceRequestService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public IActionResult Index()
        {

            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == GetEmployee().Id);
            var mapping = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);
            _ındividualAdvanceRequestService.GetAdvanceApprovelName(mapping);
            SetUserImageViewBag();
            return View(mapping);
        }

        public IActionResult Create()
        {
            SetUserImageViewBag();
            var currencyValues = Enum.GetValues(typeof(Currency));
            ViewBag.CurrencyOptions = new SelectList(currencyValues);
            return View();
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndividualAdvanceRequestVM individualAdvanceRequest)
        {
            var existingPendingRequest = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().FirstOrDefault(x => x.EmployeeRequestingId ==GetEmployee().Id && x.ApprovalStatus==null);

            if (existingPendingRequest!=null)
            {
                ModelState.AddModelError("", "You already have a pending request. Please wait for its approval.");
                SetUserImageViewBag();
                GetExpenditureTypes();
                GetCurrencyType();
                return View(individualAdvanceRequest);

            }



            var validation = _validator.Validate(individualAdvanceRequest);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://finans.truncgil.com/today.json");
            var data = JsonConvert.DeserializeObject<DovizModel>(response);
            data.DovizList = new Dictionary<Currency, Doviz>
    {
        { Currency.Dolar, data.USD }, // Burada diğer döviz türlerini de eklemelisiniz
        { Currency.Euro, data.EUR },
        // Diğer döviz türleri
    }; if (validation.IsValid)
            {

                if (data != null)
                {
                    var selectedCurrency = individualAdvanceRequest.Currency;
                    if (selectedCurrency.HasValue)
                    {
                        var harcama = decimal.Zero;

                        if (selectedCurrency.Value == Currency.TL)
                        {
                            harcama = individualAdvanceRequest.Amount;
                        }
                        else if (selectedCurrency.Value == Currency.Euro)
                        {
                            var euroDoviz = data.DovizList[Currency.Euro];
                            harcama = decimal.Parse(euroDoviz.Satış) * individualAdvanceRequest.Amount;
                        }
                        else if (selectedCurrency.Value == Currency.Dolar)
                        {
                            var usdDoviz = data.DovizList[Currency.Dolar];
                            harcama = decimal.Parse(usdDoviz.Satış) * individualAdvanceRequest.Amount;
                        }


                        if (user.MaxAdvanceAmount > harcama)
                        {
                            individualAdvanceRequest.EmployeeRequestingId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            var mappingQuery = _mapper.Map<IndividualAdvance>(individualAdvanceRequest);
                            mappingQuery.RequestDate = DateTime.Now;
                            await _hrDb.AddAsync(mappingQuery);
                            await _hrDb.SaveChangesAsync();

                            var newmax = user.MaxAdvanceAmount - harcama;
                            user.MaxAdvanceAmount = newmax;

                            _hrDb.Update(user);
                            _hrDb.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("Amount", "Requested amount exceeds the maximum allowance.");
                        }
                    }

                    // Doğrulama başarısız ise veya izin verilen harcamayı aşamadıysa

                }
            }
            SetUserImageViewBag();

            var currencyValues = Enum.GetValues(typeof(Currency));
            ViewBag.CurrencyOptions = new SelectList(currencyValues);

            return View(individualAdvanceRequest);

        }

        public IActionResult Delete(int id)
        {

            var request = _ındividualAdvanceRequestService.GetByIdIndividualAdvanceRequest(id);
            var mapli = _mapper.Map<IndividualAdvanceRequestVM>(request);
            SetUserImageViewBag();
            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(IndividualAdvanceRequestVM requestVm)
        {

            SetUserImageViewBag();
            _ındividualAdvanceRequestService.TDelete(_mapper.Map<IndividualAdvance>(requestVm));
            return RedirectToAction("Index");

        }


        public IActionResult NotConfirmedForApprovalexp()
        {
            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == GetEmployee().Id);
            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);

            _ındividualAdvanceRequestService.GetAdvanceApprovelName(mappingQuery);

            var notConfirmedApprovalExp = mappingQuery.Where(item => item.ApprovalStatus == false);
            SetUserImageViewBag();
            return View(notConfirmedApprovalExp);


        }


        public IActionResult ConfirmedForApprovalexp()
        {
          

            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == GetEmployee().Id);
            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);


            _ındividualAdvanceRequestService.GetAdvanceApprovelName(mappingQuery);

            var confirmedForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == true);
          
            SetUserImageViewBag();
            return View(confirmedForApprovalexp);


        }


        public IActionResult WaitingForApprovalexp()
        {
           
            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == GetEmployee().Id);
            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);
            _ındividualAdvanceRequestService.GetAdvanceApprovelName(mappingQuery);
            var waitingForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == null);
            SetUserImageViewBag();
            return View(waitingForApprovalexp);
        }


        [NonAction]
        private void GetExpenditureTypes()
        {
            ViewData["ExpentitureTypeId"] = new SelectList(_hrDb.ExpenditureTypes, "Id", "ExpenditureName");
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


    }



}


