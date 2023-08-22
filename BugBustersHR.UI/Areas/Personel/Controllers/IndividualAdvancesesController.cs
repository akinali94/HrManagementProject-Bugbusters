using AutoMapper;
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

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _hrDb.Personels.First(x => x.Id == userId);

            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == userId);

            var mapping = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);
            foreach (var item in mapping)
            {
                if (item.ApprovalStatus == null)
                {
                    item.ApprovalStatusName = "Waiting for Approval";
                }
                else if (item.ApprovalStatus == true)
                {
                    item.ApprovalStatusName = "Confirmed";
                }
                else
                {
                    item.ApprovalStatusName = "Not Confirmed";
                }
            }


            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(mapping);
        }

        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            var currencyValues = Enum.GetValues(typeof(Currency));
            ViewBag.CurrencyOptions = new SelectList(currencyValues);



            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(IndividualAdvanceRequestVM individualAdvanceRequest)
        //{
        //    var validation = _validator.Validate(individualAdvanceRequest);
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);



        //    var harcama = individualAdvanceRequest.Amount;



        //    if (validation.IsValid)
        //    {
        //        if (user.MaxAdvanceAmount > harcama)
        //        {



        //            individualAdvanceRequest.EmployeeRequestingId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            var mappingQuery = _mapper.Map<IndividualAdvance>(individualAdvanceRequest);
        //            await _hrDb.AddAsync(mappingQuery);
        //            await _hrDb.SaveChangesAsync();


        //            var newmax = user.MaxAdvanceAmount - individualAdvanceRequest.Amount;

        //            user.MaxAdvanceAmount = newmax;


        //            _hrDb.Update(user);
        //            _hrDb.SaveChanges();
        //            return RedirectToAction("Index");

        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Amount", "Requested amount exceeds the maximum allowance.");

        //            ViewBag.UserImageUrl = user?.ImageUrl;
        //            ViewBag.UserFullName = user?.FullName;



        //            return View(individualAdvanceRequest);
        //        }
        //    }



        //    // Doğrulama başarısız ise

        //    ViewBag.UserImageUrl = user?.ImageUrl;
        //    ViewBag.UserFullName = user?.FullName;

        //    var currencyValues = Enum.GetValues(typeof(Currency));
        //    ViewBag.CurrencyOptions = new SelectList(currencyValues);

        //    return View(individualAdvanceRequest);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IndividualAdvanceRequestVM individualAdvanceRequest)
        {
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
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            var currencyValues = Enum.GetValues(typeof(Currency));
            ViewBag.CurrencyOptions = new SelectList(currencyValues);

            return View(individualAdvanceRequest);

        }





        public IActionResult Delete(int id)
        {

            var request = _ındividualAdvanceRequestService.GetByIdIndividualAdvanceRequest(id);
            var mapli = _mapper.Map<IndividualAdvanceRequestVM>(request);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(IndividualAdvanceRequestVM requestVm)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            _ındividualAdvanceRequestService.TDelete(_mapper.Map<IndividualAdvance>(requestVm));
            return RedirectToAction("Index");

        }


        public IActionResult NotConfirmedForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);


            foreach (var item in mappingQuery)
            {
                if (item.ApprovalStatus == null)
                {
                    item.ApprovalStatusName = "Waiting for Approval";
                }
                else if (item.ApprovalStatus == true)
                {
                    item.ApprovalStatusName = "Confirmed";
                }
                else
                {
                    item.ApprovalStatusName = "Not Confirmed";
                }
            }

            var notConfirmedApprovalExp = mappingQuery.Where(item => item.ApprovalStatus == false);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(notConfirmedApprovalExp);


        }


        public IActionResult ConfirmedForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);


            foreach (var item in mappingQuery)
            {
                if (item.ApprovalStatus == null)
                {
                    item.ApprovalStatusName = "Waiting for Approval";
                }
                else if (item.ApprovalStatus == true)
                {
                    item.ApprovalStatusName = "Confirmed";
                }
                else
                {
                    item.ApprovalStatusName = "Not Confirmed";
                }
            }

            var confirmedForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == true);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(confirmedForApprovalexp);


        }


        public IActionResult WaitingForApprovalexp()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);

            var query = _ındividualAdvanceRequestService.GetAllIndividualAdvanceReq().Where(x => x.EmployeeRequestingId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query);

            foreach (var item in mappingQuery)
            {
                if (item.ApprovalStatus == null)
                {
                    item.ApprovalStatusName = "Waiting for Approval";
                }
                else if (item.ApprovalStatus == true)
                {
                    item.ApprovalStatusName = "Confirmed";
                }
                else
                {
                    item.ApprovalStatusName = "Not Confirmed";
                }
            }

            var waitingForApprovalexp = mappingQuery.Where(item => item.ApprovalStatus == null);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            return View(waitingForApprovalexp);
        }
    }



}


