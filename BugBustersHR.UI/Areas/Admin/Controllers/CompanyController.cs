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
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IValidator<CompanyVM> _companyValidator;
        private readonly CompanyVM _companyVM;
        private readonly HrDb _hrDb;

        public CompanyController(ICompanyService service, IMapper mapper, IValidator<CompanyVM> companyValidator, HrDb hrDb, IWebHostEnvironment webHostEnvironment)
        {
            _service = service;
            _mapper = mapper;
            _companyValidator = companyValidator;
            _hrDb = hrDb;
            _webHostEnvironment = webHostEnvironment;
        }

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
            //CompanyValidator validator = new CompanyValidator();
            
            var validationResult = _companyValidator.Validate(companyVm);

            if (validationResult.IsValid)
            {
                var company = _mapper.Map<Companies>(companyVm);
                
                await _service.TAddAsync(company);
                return RedirectToAction("Index");
                //await _service.TAddAsync(_mapper.Map<EmployeeLeaveType>(typeVm));
                //return RedirectToAction("Index");

                

                if (validationResult.IsValid)
                {
                   

                    // Handle logo file
                    if (companyVm.LogoFile != null)
                    {
                        // Upload and save logo file to a desired location
                        var logoFileName = Guid.NewGuid().ToString() + Path.GetExtension(companyVm.LogoFile.FileName);
                        var logoFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "logo_uploads", logoFileName);

                        using (var fileStream = new FileStream(logoFilePath, FileMode.Create))
                        {
                            await companyVm.LogoFile.CopyToAsync(fileStream);
                        }

                        company.Logo = "/logo_uploads/" + logoFileName; // Store relative path in the database
                    }

                    await _service.TAddAsync(company);
                    return RedirectToAction("Index");
                }

                return View(companyVm);
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
            //CompanyValidator validator = new CompanyValidator();

            var validationResult = _companyValidator.Validate(companyVM);

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

        [HttpGet]
        public IActionResult GetCompanyDetails(string id)
        {

            var getCompany = _service.TGetById(id);
            var mappingQuery1 = _mapper.Map<CompanyDetailsVM>(getCompany);


            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            return View(mappingQuery1);

        }

    }
}
