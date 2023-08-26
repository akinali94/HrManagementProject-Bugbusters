using AutoMapper;
using BugBustersHR.BLL.Options;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.CompanyService;
using BugBustersHR.BLL.Validatons.CompanyValidation;
using BugBustersHR.BLL.Validatons.LeaveValidations;
using BugBustersHR.BLL.ViewModels.CompanyViewModel;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BugBustersHR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly ICompanyService _service;
        private readonly IMapper _mapper;
       
        private readonly IValidator<CompanyVM> _companyValidator;
        private readonly CompanyVM _companyVM;
        private readonly HrDb _hrDb;

        public CompanyController(ICompanyService service, IMapper mapper, IValidator<CompanyVM> companyValidator, HrDb hrDb)
        {
            _service = service;
            _mapper = mapper;
            _companyValidator = companyValidator;
            _hrDb = hrDb;
        }

        public IActionResult Index()
        {
            var query = _service.GetAllCompany();
            var mappingQuery = _mapper.Map<List<CompanyVM>>(query);

            SetUserImageViewBag();
            return View(mappingQuery);
       
        }

        public IActionResult Create()
        {
            SetUserImageViewBag();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CompanyVM companyVm)
        {

            var validationResult = _companyValidator.Validate(companyVm);

            if (validationResult.IsValid)
            {
                var company = _mapper.Map<Companies>(companyVm);
                
                await _service.TAddAsync(company);
                SetUserImageViewBag();
                return RedirectToAction("Index");
                
            }
            SetUserImageViewBag();
            return View(companyVm);
        }



        public IActionResult Edit(int id)
        {

            var company = _service.GetByIdCompany(id);

            var mapli = _mapper.Map<CompanyVM>(company);
            SetUserImageViewBag();
            return View(mapli);
          
        }
        [HttpPost]
        public IActionResult Edit(CompanyVM companyVM)
        {
    

            var validationResult = _companyValidator.Validate(companyVM);

            if (validationResult.IsValid)
            {
                _service.TUpdate(_mapper.Map<Companies>(companyVM));
                SetUserImageViewBag();
                return RedirectToAction("Index");
            }
            SetUserImageViewBag();
            return View(companyVM);
        }


        public IActionResult Delete(int id)
        {

            var company = _service.GetByIdCompany(id);

            var mapli = _mapper.Map<CompanyVM>(company);
            SetUserImageViewBag();

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(CompanyVM companyVM)
        {



            _service.TDelete(_mapper.Map<Companies>(companyVM));
            SetUserImageViewBag();
            return RedirectToAction("Index");


        }

        [HttpGet]
        public IActionResult GetCompanyDetails(string id)
        {

            var getCompany = _service.TGetById(id);
            var mappingQuery1 = _mapper.Map<CompanyDetailsVM>(getCompany);

            SetUserImageViewBag();
            return View(mappingQuery1);

        }


        [NonAction]
        private void SetUserImageViewBag()
        {
      

            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
      

        }
        [NonAction]
        private Employee GetEmployee()
        {
            return _service.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    }
}
