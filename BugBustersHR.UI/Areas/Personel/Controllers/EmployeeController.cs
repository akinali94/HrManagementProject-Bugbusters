using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BugBustersHR.BLL.Options;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.Validatons;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;
        private readonly AzureOptions _azureOptions;
        private readonly IValidator<EmployeeUpdateVM> _personelValidator;
        private readonly HrDb _hrDb;

        public EmployeeController(IEmployeeService service, IMapper mapper, IOptions<AzureOptions> azureOptions, IValidator<EmployeeUpdateVM> personelValidator, HrDb hrDb)
        {
            _service = service;
            _mapper = mapper;
            _azureOptions = azureOptions.Value;
            _personelValidator = personelValidator;
            _hrDb = hrDb;
        }
      
        public IActionResult Index()
        {

            var qury1 = _service.TGetById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var mappingQuery = _mapper.Map<EmployeeSummaryListVM>(qury1);


            ViewBag.UserImageUrl = qury1?.ImageUrl;
            ViewBag.UserFullName = qury1?.FullName;

            return View(mappingQuery);
        }

        public IActionResult Edit(string id)
        {

            var personel = _service.TGetById(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var mapli = _mapper.Map<EmployeeUpdateVM>(personel);


            ViewBag.UserImageUrl = personel?.ImageUrl;
            ViewBag.UserFullName = personel?.FullName;

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeUpdateVM updateVm, IFormFile backgroundImageFile)
        {

            EmployeeValidator validator = new EmployeeValidator();
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
            return View(updateVm);
        }


        public IActionResult Details(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query1 = _service.TGetById(userId);
            var mappingQuery1 = _mapper.Map<EmployeeListWithoutSalaryVM>(query1);

            ViewBag.UserImageUrl = query1?.ImageUrl;
            ViewBag.UserFullName = query1?.FullName;
            return View(mappingQuery1);
        }
    }
}
