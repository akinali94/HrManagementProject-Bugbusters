using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BugBustersHR.BLL.Options;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Concrete;
using BugBustersHR.BLL.Validatons;
using BugBustersHR.BLL.ViewModels.AdminViewModel;
using BugBustersHR.BLL.ViewModels.ManagerViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using BugBustersHR.BLL.Validatons.CreateManagerValidation;
using BugBustersHR.UI.Email.ServiceEmail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.Extensions.Options;
using System.Collections;
using BugBustersHR.BLL.Services.Abstract.CompanyService;
using BugBustersHR.BLL.ViewModels.CompanyViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BugBustersHR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Role_Admin)]
    public class DefaultController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICompanyService _companyService;

        private readonly IMapper _mapper;
        private readonly HrDb _hrDb;
        private readonly IValidator<AdminUpdateVM> _adminValidator;
        private readonly IValidator<CreateManagerFromAdminVM> _CreateManagerValidator;
        private readonly AzureOptions _azureOptions;
    
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public DefaultController(IEmployeeService employeeService, IMapper mapper, HrDb hrDb, IValidator<AdminUpdateVM> adminValidator, IValidator<CreateManagerFromAdminVM> CreateManagerValidator, IOptions<AzureOptions> azureOptions, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, ICompanyService companyService)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _hrDb = hrDb;
            _adminValidator = adminValidator;
            _CreateManagerValidator = CreateManagerValidator;
            _azureOptions = azureOptions.Value;

            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _companyService = companyService;
        }


        public IActionResult Index()
        {
            
            var mappingQuery = _mapper.Map<AdminSummaryListVM>(GetEmployee());
            SetUserImageViewBag();

            return View(mappingQuery);
        }

        public IActionResult Edit(string id)
        {
            var mapli = _mapper.Map<AdminUpdateVM>(GetEmployee());
            SetUserImageViewBag();

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Edit(AdminUpdateVM updateVm, IFormFile backgroundImageFile)
        {

            AdminValidator validator = new AdminValidator();
            var validationResult = validator.Validate(updateVm);

            if (validationResult.IsValid)
            {
                try  
                {
                    var entity = _employeeService.TGetById(updateVm.Id);

                    entity.TelephoneNumber = updateVm.TelephoneNumber;
                    entity.Address = updateVm.Address;


                    if (updateVm.ImageModel.File != null)
                    {
                        string fileExtension = Path.GetExtension(updateVm.ImageModel.File.FileName);
                        var uniqueName = Guid.NewGuid().ToString() + fileExtension;

                        using (MemoryStream fileUploadStream = new MemoryStream())
                        {
                            updateVm.ImageModel.File.CopyTo(fileUploadStream);
                            fileUploadStream.Position = 0;

                            BlobContainerClient blobContainerClient = new BlobContainerClient(
                                _azureOptions.ConnectionString,
                                _azureOptions.Container);

                            BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueName);

                            blobClient.Upload(fileUploadStream, new BlobHttpHeaders()
                            {
                                ContentType = updateVm.ImageModel.File.ContentType,
                            });

                            entity.ImageUrl = "https://bugbustersstorage.blob.core.windows.net/contentupload/" + uniqueName;
                        }
                    }
                    if (backgroundImageFile != null)
                    {
                        string backgroundFileExtension = Path.GetExtension(backgroundImageFile.FileName);
                        var backgroundUniqueName = Guid.NewGuid().ToString() + backgroundFileExtension;

                        using (MemoryStream backgroundFileUploadStream = new MemoryStream())
                        {
                            backgroundImageFile.CopyTo(backgroundFileUploadStream);
                            backgroundFileUploadStream.Position = 0;

                            BlobContainerClient backgroundBlobContainerClient = new BlobContainerClient(
                                _azureOptions.ConnectionString,
                                _azureOptions.Container);

                            BlobClient backgroundBlobClient = backgroundBlobContainerClient.GetBlobClient(backgroundUniqueName);

                            backgroundBlobClient.Upload(backgroundFileUploadStream, new BlobHttpHeaders()
                            {
                                ContentType = backgroundImageFile.ContentType,
                            });

                            entity.BackgroundImageUrl = "https://bugbustersstorage.blob.core.windows.net/contentupload/" + backgroundUniqueName;
                        }
                    }

                    _employeeService.TUpdate(entity);
                    SetUserImageViewBag();
                    return RedirectToAction("Index", new { imageUrl = entity.ImageUrl, backgroundImageUrl = entity.BackgroundImageUrl });


                }
                catch (Exception ex)
                {
                    SetUserImageViewBag();
                    Console.WriteLine(ex);
                }
            }

            SetUserImageViewBag();
            return View(updateVm);
        }

        public IActionResult Details(string id)
        {
  
            var query1 = _employeeService.TGetById(GetEmployee().Id);
            var mappingQuery1 = _mapper.Map<AdminListWithoutSalaryVM>(query1);
            SetUserImageViewBag();

            return View(mappingQuery1);
        }



        [HttpGet]
        public IActionResult CreateManager()
        {
            ViewBag.gender = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Woman", Value = "1"},
                new SelectListItem{Text = "Man", Value = "0"}
            }, "Value", "Text");

       
            IEnumerable<Companies> companies = _companyService.GetAllCompany();

            List<SelectListItem> companyItems = companies.Select(c => new SelectListItem
            {
                Text = c.CompanyName,
                Value = c.CompanyName
            }).ToList();

            ViewBag.Companies = companyItems;

            string passwordGenerated = _employeeService.GenerateRandomPassword(null);
            ViewBag.GeneratedPassword = passwordGenerated;
            SetUserImageViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateManager(CreateManagerFromAdminVM createManagerFromAdminVM)
        {
            CreateManagerValidator validator = new CreateManagerValidator();
            var validationResult = validator.Validate(createManagerFromAdminVM);

            if (validationResult.IsValid)
            {
                var mapManager = _mapper.Map<Employee>(createManagerFromAdminVM);

                mapManager.UserName = createManagerFromAdminVM.Email;
                mapManager.Role = AppRoles.Role_Manager;

                var result = await _userManager.CreateAsync(mapManager, createManagerFromAdminVM.Password);

                if (!await _roleManager.RoleExistsAsync(AppRoles.Role_Manager))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Manager));
                }

                await _userManager.AddToRoleAsync(mapManager, AppRoles.Role_Manager);


                string code = await _userManager.GeneratePasswordResetTokenAsync(mapManager);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var link = Url.Page("/Account/ResetPassword", pageHandler: null, values: new { area = "Identity", userId = mapManager.Id, code }, protocol: Request.Scheme);


                await _emailService.SendConfirmEmail(link, mapManager.Email, mapManager.PasswordHash);
                SetUserImageViewBag();
                return RedirectToAction("Index", "Default");
            }

            ViewBag.gender = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Woman", Value = "1"},
                new SelectListItem{Text = "Man", Value = "0"}
            }, "Value", "Text");
            SetUserImageViewBag();
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Companies> companies = _companyService.GetAllCompany();

            List<SelectListItem> companyItems = companies.Select(c => new SelectListItem
            {
                Text = c.CompanyName,
                Value = c.CompanyName
            }).ToList();

            ViewBag.Companies = companyItems;

            string passwordGenerated = _employeeService.GenerateRandomPassword(null);
            ViewBag.GeneratedPassword = passwordGenerated;


            return View();
        }

        public IActionResult GetManagerList()
        {

            var getList = _hrDb.Personels.Where(x => x.Role == AppRoles.Role_Manager).ToList();
            
            var mappingList = _mapper.Map<List<GetManagerListVM>>(getList);
            SetUserImageViewBag();
            return View(mappingList);
        }

        public IActionResult GetManagerDetail(string id)
        {
            var manager = _employeeService.GetByIdEmployee(id);
            
            //var manager = _hrDb.Personels.FirstOrDefault(x => x.Id == id && x.Role == AppRoles.Role_Manager);

            if (manager == null)
            {

                return RedirectToAction("GetManagerList");
            }

            var mappedManager = _mapper.Map<GetManagerListVM>(manager);
            SetUserImageViewBag();
            return View(mappedManager);
        }

        public IActionResult ManagerEdit(string id)
        {
            var manager = _employeeService.GetByIdEmployee(id);
            //var manager = _hrDb.Personels.FirstOrDefault(x => x.Id == id && x.Role == AppRoles.Role_Manager);
            if (manager == null)
            {
                return View("GetManagerDetail");
            }

            var mapli = _mapper.Map<GetManagerListVM>(manager);
            mapli.SalaryString=mapli.Salary.ToString().Replace(",",".");

         
            SetUserImageViewBag();

            return View(mapli);
        }

        [HttpPost]
        public IActionResult ManagerEdit(GetManagerListVM model)
        {

            var existingManager = _hrDb.Personels.FirstOrDefault(x => x.Id == model.Id && x.Role == AppRoles.Role_Manager);


            if (existingManager == null)
            {
                return RedirectToAction("GetManagerList");
            }

            existingManager.Id = model.Id;

            existingManager.FullName = model.FullName;
            existingManager.Name = model.Name;
            existingManager.SecondName = model.SecondName;
            existingManager.Surname = model.Surname;
            existingManager.SecondSurname = model.SecondSurname;
            existingManager.BirthPlace = model.BirthPlace;
            existingManager.TC = model.TC;
            existingManager.BirthDate = model.BirthDate;
            existingManager.HiredDate = model.HiredDate;
            existingManager.IsActive = model.IsActive;
            existingManager.Title = model.Title;
            existingManager.Section = model.Section;
            existingManager.Salary = Convert.ToDecimal(model.SalaryString);
            existingManager.TelephoneNumber = model.TelephoneNumber;
            existingManager.Address = model.Address;
            existingManager.CompanyName = model.CompanyName;
            existingManager.Email = model.Email;


            _hrDb.Update(existingManager);
            _hrDb.SaveChanges();


            return RedirectToAction("GetManagerDetail", new { id = model.Id });

        }

        public IActionResult ManagerDelete(string id)
        {
            var manager = _employeeService.GetByIdEmployee(id);
            _employeeService.TDelete(manager);
            return RedirectToAction("GetManagerList");
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
            return _employeeService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
