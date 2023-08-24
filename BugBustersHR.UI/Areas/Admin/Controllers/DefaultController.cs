﻿using AutoMapper;
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

namespace BugBustersHR.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Role_Admin)]
    public class DefaultController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly HrDb _hrDb;
        private readonly IValidator<AdminUpdateVM> _adminValidator;
        private readonly IValidator<CreateManagerFromAdminVM> _createAdminValidator;
        private readonly AzureOptions _azureOptions;
        private readonly AdminVM _adminVM;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public DefaultController(IEmployeeService employeeService, IMapper mapper, HrDb hrDb, IValidator<AdminUpdateVM> adminValidator, IValidator<CreateManagerFromAdminVM> createAdminValidator, AzureOptions azureOptions, AdminVM adminVM, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _hrDb = hrDb;
            _adminValidator = adminValidator;
            _createAdminValidator = createAdminValidator;
            _azureOptions = azureOptions;
            _adminVM = adminVM;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var findAdminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var getAdmin = _employeeService.TGetById(findAdminID);
            var mappingQuery = _mapper.Map<AdminSummaryListVM>(getAdmin);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == findAdminID);
            ViewBag.AdminImagerUrl = user?.ImageUrl;
            ViewBag.AdminFullName = user?.FullName;

            return View(mappingQuery);
        }

        public IActionResult Edit(string id)
        {
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var getAdmin = _employeeService.TGetById(adminID);

            var mappingQuery = _mapper.Map<AdminUpdateVM>(getAdmin);

            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            ViewBag.AdminImagerUrl = user?.ImageUrl;
            ViewBag.AdminFullName = user?.FullName;

            return View(mappingQuery);
        }

        [HttpPost]
        public IActionResult Edit(AdminUpdateVM updateVm, IFormFile backgroundImageFile)
        {

            AdminValidator adminValidator = new AdminValidator();
            var validationResult = adminValidator.Validate(updateVm);

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

                    return RedirectToAction("Index", new { imageUrl = entity.ImageUrl, backgroundImageUrl = entity.BackgroundImageUrl });


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            ViewBag.AdminImageUrl = admin?.ImageUrl;
            ViewBag.AdminFullName = admin?.FullName;
            return View(updateVm);
        }

        public IActionResult Details(string id)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query1 = _employeeService.TGetById(adminId);
            var mappingQuery1 = _mapper.Map<AdminListWithoutSalaryVM>(query1);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == adminId);
            ViewBag.AdminImageUrl = user?.ImageUrl;
            ViewBag.AdminFullName = user?.FullName;

            return View(mappingQuery1);
        }



        [HttpGet]
        public IActionResult CreateAdmin()
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

            //ViewBag.Company = admin.CompanyName;

            string passwordGenerated = _employeeService.GenerateRandomPassword(null);
            ViewBag.GeneratedPassword = passwordGenerated;

            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateManagerFromAdminVM createManagerFromAdminVM)
        {
            CreateManagerValidator validator = new CreateManagerValidator();
            var validationResult = validator.Validate(createManagerFromAdminVM);

            if (validationResult.IsValid)
            {
                var mapManager = _mapper.Map<Employee>(createManagerFromAdminVM);

                mapManager.UserName = createManagerFromAdminVM.Email;
                mapManager.Role = AppRoles.Role_Manager;

                //await _userStore.SetUserNameAsync(mapEmployee, createEmployeeVM.Email, CancellationToken.None);
                //await _emailStore.SetEmailAsync(mapEmployee, createEmployeeVM.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(mapManager, createManagerFromAdminVM.Password);
                //_service.TAddAsync(mapEmployee);

                if (!await _roleManager.RoleExistsAsync(AppRoles.Role_Manager))
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Manager));
                }

                await _userManager.AddToRoleAsync(mapManager, AppRoles.Role_Manager);


                string code = await _userManager.GeneratePasswordResetTokenAsync(mapManager);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var link = Url.Page("/Account/ResetPassword", pageHandler: null, values: new { area = "Identity", userId = mapManager.Id, code }, protocol: Request.Scheme);


                await _emailService.SendConfirmEmail(link, mapManager.Email, mapManager.PasswordHash);

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
          

            string passwordGenerated = _employeeService.GenerateRandomPassword(null);
            ViewBag.GeneratedPassword = passwordGenerated;


            return View();
        }

        public IActionResult GetAdminList()
        {
            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _hrDb.Personels.FirstOrDefault(u => u.Id == adminID);
            //ROLE ÇEKİLECEK

            var getList = _hrDb.Personels.ToList();

            var mappingList = _mapper.Map<List<GetManagerListVM>>(getList);
            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;
            return View(mappingList);
        }
    }
}
