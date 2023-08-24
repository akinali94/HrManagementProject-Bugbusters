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

namespace BugBustersHR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly ICompanyService _service;
        private readonly IMapper _mapper;
       
        private readonly IValidator<CompanyVM> _companyValidator;
        private readonly HrDb _hrDb;
        public IActionResult Index()
        {
            var query = _service.GetAllCompany();
            var mappingQuery = _mapper.Map<List<CompanyVM>>(query);

            return View(mappingQuery);
       
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CompanyVM companyVm)
        {
            CompanyValidator validator = new CompanyValidator();
            var validationResult = validator.Validate(companyVm);

            if (validationResult.IsValid)
            {
                var company = _mapper.Map<Companies>(companyVm);
                
                await _service.TAddAsync(company);
                return RedirectToAction("Index");
                //await _service.TAddAsync(_mapper.Map<EmployeeLeaveType>(typeVm));
                //return RedirectToAction("Index");
            }

            return View(companyVm);
        }



        public IActionResult Edit(int id)
        {

            var company = _service.GetByIdCompany(id);

            var mapli = _mapper.Map<CompanyVM>(company);

            return View(mapli);
        }
        [HttpPost]
        public IActionResult Edit(CompanyVM companyVM)
        {
            CompanyValidator validator = new CompanyValidator();
            var validationResult = validator.Validate(companyVM);

            if (validationResult.IsValid)
            {
                _service.TUpdate(_mapper.Map<Companies>(companyVM));
                return RedirectToAction("Index");
            }

            return View(companyVM);
        }


        public IActionResult Delete(int id)
        {

            var company = _service.GetByIdCompany(id);

            var mapli = _mapper.Map<CompanyVM>(company);

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(CompanyVM companyVM)
        {



            _service.TDelete(_mapper.Map<Companies>(companyVM));
            return RedirectToAction("Index");


        }

    }
}
