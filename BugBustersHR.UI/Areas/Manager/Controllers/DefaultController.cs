using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BugBustersHR.BLL.Options;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Validatons;
using BugBustersHR.BLL.ViewModels.ManagerViewModel;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using BugBustersHR.UI.Email.ServiceEmail;
using BugBustersHR.BLL.Validatons.CreateEnployeeValidations;
using BugBustersHR.BLL.ViewModels.AdminViewModel;

namespace BugBustersHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = AppRoles.Role_Manager)]
    public class DefaultController : Controller
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;
        private readonly AzureOptions _azureOptions;
        private readonly IValidator<EmployeeUpdateVM> _personelValidator;
        private readonly HrDb _hrDb;
        //private readonly IUserStore<IdentityUser> _userStore;
        private readonly IEmailSender _emailSender;
        //private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
       


        public DefaultController(IEmployeeService service, IMapper mapper, IOptions<AzureOptions> azureOptions, IValidator<EmployeeUpdateVM> personelValidator, HrDb hrDb, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender, IEmailService emailService)
        {
            _service = service;
            _mapper = mapper;
            _azureOptions = azureOptions.Value;
            _personelValidator = personelValidator;
            _hrDb = hrDb;

            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _emailService = emailService;
          
        }
        public IActionResult Index()
        {
            var query2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var qury1 = _service.TGetById(query2);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

           
            var mappingQuery = _mapper.Map<ManagerSummaryListVM>(qury1);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            return View(mappingQuery);
        }

        public IActionResult Edit(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var personel = _service.TGetById(userId);

            var mapli = _mapper.Map<ManagerUpdateVM>(personel);

            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Edit(ManagerUpdateVM updateVm, IFormFile backgroundImageFile)
        {

            ManagerValidator validator = new ManagerValidator();
            var validationResult = validator.Validate(updateVm);

            if (validationResult.IsValid)
            {
                try
                {
                    var entity = _service.TGetById(updateVm.Id);

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

                    _service.TUpdate(entity);

                    return RedirectToAction("Index", new { imageUrl = entity.ImageUrl, backgroundImageUrl = entity.BackgroundImageUrl });


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            return View(updateVm);
        }


        public IActionResult Details(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query1 = _service.TGetById(userId);
            var mappingQuery1 = _mapper.Map<ManagerListWithoutSalaryVM>(query1);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            return View(mappingQuery1);
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            ViewBag.gender = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Woman", Value = "1"},
                new SelectListItem{Text = "Man", Value = "0"}
            }, "Value", "Text");

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Bunun servisi yazılabilir
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);

            ViewBag.Company = admin.CompanyName;

            string passwordGenerated = _service.GenerateRandomPassword(null);
            ViewBag.GeneratedPassword = passwordGenerated;

            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeFromManagerVM createEmployeeVM)
        {
            CreateEmployeeValidator validator= new CreateEmployeeValidator();
            var validationResult = validator.Validate(createEmployeeVM);

            if (validationResult.IsValid)
            {
                var mapEmployee = _mapper.Map<Employee>(createEmployeeVM);

                mapEmployee.UserName = createEmployeeVM.Email;
                mapEmployee.Role = AppRoles.Role_Employee;

                var result = await _userManager.CreateAsync(mapEmployee, createEmployeeVM.Password);


                if (!await _roleManager.RoleExistsAsync(AppRoles.Role_Employee))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Employee));
                }

                await _userManager.AddToRoleAsync(mapEmployee, AppRoles.Role_Employee);


                string code = await _userManager.GeneratePasswordResetTokenAsync(mapEmployee);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var link = Url.Page("/Account/ResetPassword", pageHandler: null, values: new { area = "Identity", userId = mapEmployee.Id, code }, protocol: Request.Scheme);


                await _emailService.SendConfirmEmail(link,mapEmployee.Email,mapEmployee.PasswordHash);

                return RedirectToAction("Index", "Default");
            }

            ViewBag.gender = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Woman", Value = "1"},
                new SelectListItem{Text = "Man", Value = "0"}
            }, "Value", "Text");

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Bunun servisi yazılabilir
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);

            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            ViewBag.Company = admin.CompanyName;
            ViewBag.Company = admin.CompanyName;

            string passwordGenerated = _service.GenerateRandomPassword(null);
            ViewBag.GeneratedPassword = passwordGenerated;

            
            return View();
        }

        public IActionResult GetEmployeeList()
        {
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            //ROLE ÇEKİLECEK

            var getList = _hrDb.Personels.Where(x => x.CompanyName == admin.CompanyName && x.Id != adminID).ToList();

            var mappingList = _mapper.Map<List<GetEmployeeListVM>>(getList);
            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            return View(mappingList);
        }

        [HttpGet]
        public IActionResult GetEmployeeDetails(string id)
        {
            
            var getEmployee = _service.TGetById(id);
            var mappingQuery1 = _mapper.Map<EmployeeListWithoutSalaryVM>(getEmployee);

            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            return View(mappingQuery1);
            
        }

  
    }
}
