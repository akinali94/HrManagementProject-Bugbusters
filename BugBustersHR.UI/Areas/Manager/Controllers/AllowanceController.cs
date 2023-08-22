using AutoMapper;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.UI.Email.ServiceEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace BugBustersHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = AppRoles.Role_Manager)]
    public class AllowanceController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IInstitutionalAllowanceService _institutionalAllowanceService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly HrDb _db;

        public AllowanceController(IEmployeeService employeeService, IInstitutionalAllowanceService institutionalAllowanceService, IMapper mapper, IEmailService emailService, HrDb db)
        {
            _employeeService = employeeService;
            _institutionalAllowanceService = institutionalAllowanceService;
            _mapper = mapper;
            _emailService = emailService;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            var allowanceList = _institutionalAllowanceService.GetAllInstitutionalAllowances();
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            //var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(leaveList)
            //    .Where(leave => userCompanyIds.Contains(leave.RequestingId)).ToList();
            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(allowanceList)
                .Where(allowance => userCompanyIds.Contains(allowance.EmployeeId)).ToList();


            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;


                //foreach (var item in mappingQuery)
                // İkinci foreach döngüsünü çıkarabilirsiniz çünkü zaten
                // ilk foreach döngüsünde her bir talep öğesi için aynı işlemi yapıyorsunuz
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

            // ViewBag.ManagerComp içine kullanıcı şirketindeki çalışanları ekle
            ViewBag.ManagerComp = userCompany;

            return View(mappingQuery);
        }

        public async Task<IActionResult> AllowanceAccepted(int id)
        {
            var allw = _db.InstitutionalAllowances.Find(id);
            var user = _db.Users.Find(allw.EmployeeId);
            await _institutionalAllowanceService.TChangeToTrueforAllowance(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "allowance");
            return RedirectToAction("Index", "Allowance", new { area = "Manager" });
        }

        public async Task<IActionResult> AllowanceRefused(int id)
        {
            var allw = _db.InstitutionalAllowances.Find(id);
            var user = _db.Users.Find(allw.EmployeeId);
            await _institutionalAllowanceService.TChangeToFalseforAllowance(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "allowance");
            return RedirectToAction("Index", "Allowance", new { area = "Manager" });
        }

        public async Task<IActionResult> WaitingForApprovalAllowance()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _institutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.ApprovalStatus == null);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();

            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;


                //foreach (var item in mappingQuery)
                // İkinci foreach döngüsünü çıkarabilirsiniz çünkü zaten
                // ilk foreach döngüsünde her bir talep öğesi için aynı işlemi yapıyorsunuz
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

            return View(mappingQuery);


        }

        public async Task<IActionResult> ConfirmedApprovalAllowance()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _institutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.ApprovalStatus == true);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();

            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;


                //foreach (var item in mappingQuery)
                // İkinci foreach döngüsünü çıkarabilirsiniz çünkü zaten
                // ilk foreach döngüsünde her bir talep öğesi için aynı işlemi yapıyorsunuz
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

            return View(mappingQuery);
        }

        public async Task<IActionResult> NotConfirmedApprovalAllowance()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _institutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.ApprovalStatus == false);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();

            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;


                //foreach (var item in mappingQuery)
                // İkinci foreach döngüsünü çıkarabilirsiniz çünkü zaten
                // ilk foreach döngüsünde her bir talep öğesi için aynı işlemi yapıyorsunuz
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

            return View(mappingQuery);
        }

    }
}
