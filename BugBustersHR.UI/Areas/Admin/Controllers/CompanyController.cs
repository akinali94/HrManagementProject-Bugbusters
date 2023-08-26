using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BugBustersHR.BLL.Options;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.CompanyService;
using BugBustersHR.BLL.Validatons.CompanyValidation;
using BugBustersHR.BLL.Validatons.LeaveValidations;
using BugBustersHR.BLL.ViewModels.CompanyViewModel;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
     
        private readonly AzureOptions _azureOptions;
        private readonly IValidator<CompanyVM> _companyValidator;
        private readonly CompanyVM _companyVM;
        private readonly HrDb _hrDb;

        public CompanyController(ICompanyService service, IMapper mapper, IValidator<CompanyVM> companyValidator, HrDb hrDb, IOptions<AzureOptions> azureOptions)
        {
            _service = service;
            _mapper = mapper;
            _companyValidator = companyValidator;
            _hrDb = hrDb;
            _azureOptions = azureOptions.Value;

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
            //CompanyValidator validator = new CompanyValidator();
            try
            {
                if (companyVm.ImageModel.File != null)
                {
                    string fileExtension = Path.GetExtension(companyVm.ImageModel.File.FileName);
                    var uniqueName = Guid.NewGuid().ToString() + fileExtension;

                    using (MemoryStream fileUploadStream = new MemoryStream())
                    {
                        companyVm.ImageModel.File.CopyTo(fileUploadStream);
                        fileUploadStream.Position = 0;

                        BlobContainerClient blobContainerClient = new BlobContainerClient(_azureOptions.ConnectionString, _azureOptions.Container);
                        BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueName);

                        blobClient.Upload(fileUploadStream, new BlobHttpHeaders
                        {
                            ContentType = companyVm.ImageModel.File.ContentType,
                        });

                        companyVm.Logo = "https://bugbustersstorage.blob.core.windows.net/contentupload/" + uniqueName;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            

            var validationResult = _companyValidator.Validate(companyVm);

            if (validationResult.IsValid)
            {
                var company = _mapper.Map<Companies>(companyVm);
                
                await _service.TAddAsync(company);
            
                //await _service.TAddAsync(_mapper.Map<EmployeeLeaveType>(typeVm));
                //return RedirectToAction("Index");

                

               
                return RedirectToAction("Index");
            }



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
        public IActionResult GetCompanyDetails(int id)
        {
            var getCompany = _service.GetByIdCompany(id);
            var mappingQuery1 = _mapper.Map<CompanyDetailsVM>(getCompany);

            SetUserImageViewBag();
            return View(new List<CompanyDetailsVM> { mappingQuery1 });
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
