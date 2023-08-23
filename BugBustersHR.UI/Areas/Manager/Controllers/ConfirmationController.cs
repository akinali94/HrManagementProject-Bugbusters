using AutoMapper;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.LeaveAbstractService;
using BugBustersHR.BLL.Services.Concrete.LeaveConcreteService;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.UI.Email.ServiceEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BugBustersHR.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = AppRoles.Role_Manager)]
    public class ConfirmationController : Controller
    {
        private readonly IEmployeeLeaveRequestService _leaveReqService;
        private readonly IMapper _mapper;
        private readonly IEmployeeLeaveTypeService _LeaveTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly HrDb _db;

        public ConfirmationController(IEmployeeLeaveRequestService leavereqService, IMapper mapper, IEmployeeLeaveTypeService LeaveTypeService, IEmployeeService employeeService, IEmailService emailService, HrDb db)
        {
            _leaveReqService = leavereqService;
            _mapper = mapper;
            _LeaveTypeService = LeaveTypeService;
            _employeeService = employeeService;
            _emailService = emailService;
            _db = db;
        }

        public async Task<IActionResult> LeaveConfirmation()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;
            var leaveList = _leaveReqService.GetAllLeaveReq();
            // Yöneticinin şirketindeki tüm çalışanları al ve ID'lerini liste olarak al
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            // Talepleri kullanıcı şirketindeki çalışanlarla eşleştirerek bir liste oluştur
            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(leaveList)
                .Where(leave => userCompanyIds.Contains(leave.RequestingId)).ToList();
            foreach (var item in mappingQuery) _leaveReqService.GetLeaveApprovelName(item);
            foreach (var item in mappingQuery)
            {
                item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.RequestingId)).FullName;
                item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                item.CompanyName = (_employeeService.TGetById(item.RequestingId)).CompanyName;

            }

             // ViewBag.ManagerComp içine kullanıcı şirketindeki çalışanları ekle
             ViewBag.ManagerComp = userCompany;
             return View(mappingQuery);
        }


        //[Route("LeaveAccepted/{id}")]
        public async Task<IActionResult> LeaveAccepted(int id)
        {
            var leave = _db.EmployeeLeaveRequests.Find(id);
            var user = _db.Users.Find(leave.RequestingId);


            await _leaveReqService.TChangeToTrueforLeave(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "leave");

            return RedirectToAction("LeaveConfirmation", "Confirmation", new { area = "Manager" });
        }

        //[Route("LeaveRefused/{id}")]
        public async Task<IActionResult> LeaveRefused(int id)
        {
            var leave = _db.EmployeeLeaveRequests.Find(id);
            var user = _db.Users.Find(leave.RequestingId);

            await _leaveReqService.TChangeToFalseforLeave(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "leave");
            return RedirectToAction("LeaveConfirmation", "Confirmation", new { area = "Manager" });
        }
        public async Task<IActionResult> WaitingForApprovalLeave()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _leaveReqService.GetAllLeaveReq().Where(x => x.Approved == null);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.RequestingId)).ToList();
            foreach (var item in mappingQuery) _leaveReqService.GetLeaveApprovelName(item);
            foreach (var item in mappingQuery)
            {
                item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.RequestingId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.RequestingId)).CompanyName;


            }

            return View(mappingQuery);
        }

        public async Task<IActionResult> AcceptedApprovalLeave()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _leaveReqService.GetAllLeaveReq().Where(x => x.Approved == true);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.RequestingId)).ToList();
            foreach (var item in mappingQuery) _leaveReqService.GetLeaveApprovelName(item);
            foreach (var item in mappingQuery)
            {
                item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.RequestingId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.RequestingId)).CompanyName;

            }

            return View(mappingQuery);
        }

        public async Task<IActionResult> NotAcceptedApprovalLeave()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _leaveReqService.GetAllLeaveReq().Where(x => x.Approved == false);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.RequestingId)).ToList();
            foreach (var item in mappingQuery) _leaveReqService.GetLeaveApprovelName(item);
            foreach (var item in mappingQuery)
            {
                item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.RequestingId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.RequestingId)).CompanyName;

            }

            return View(mappingQuery);
        }


    }
}


