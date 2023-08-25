using AutoMapper;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.IndividualAdvanceService;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
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
    public class AdvancesController : Controller
    {
        private readonly IIndividualAdvanceService _individualAdvanceService;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly HrDb _db;

        public AdvancesController(IIndividualAdvanceService individualAdvanceService, IMapper mapper, IEmployeeService employeeService, IEmailService emailService, HrDb db)
        {
            _individualAdvanceService = individualAdvanceService;
            _mapper = mapper;
            _employeeService = employeeService;
            _emailService = emailService;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;

            var advancesList = _individualAdvanceService.GetAllIndividualAdvanceReq();
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);


            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(advancesList)
                .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();

            _individualAdvanceService.GetAdvanceApprovelName(mappingQuery);
            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;
             
            }
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;

            ViewBag.ManagerComp = userCompany;

            return View(mappingQuery);
        }

        public async Task<IActionResult> AdvanceAccepted(int id)
        {
            var adv = _db.IndividualAdvances.Find(id);
            var user = _db.Users.Find(adv.EmployeeRequestingId);
            await _individualAdvanceService.TChangeToTrueforAdvance(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "individual advance");


            return RedirectToAction("Index", "Advances", new { area = "Manager" });
        }

        public async Task<IActionResult> AdvanceRefused(int id)
        {
            var adv = _db.IndividualAdvances.Find(id);
            var user = _db.Users.Find(adv.EmployeeRequestingId);
            await _individualAdvanceService.TChangeToFalseforAdvance(id);

            await _emailService.RequestApprovedMail("aa", user.Email, "individual advance");
            return RedirectToAction("Index", "Advances", new { area = "Manager" });
        }

        public async Task<IActionResult> WaitingForApprovalAdvance()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _individualAdvanceService.GetAllIndividualAdvanceReq().Where(x => x.ApprovalStatus == null);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();
            _individualAdvanceService.GetAdvanceApprovelName(mappingQuery);
            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;

            }

            return View(mappingQuery);


        }

        public async Task<IActionResult> ConfirmedApprovalAdvance()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _individualAdvanceService.GetAllIndividualAdvanceReq().Where(x => x.ApprovalStatus == true);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();
            _individualAdvanceService.GetAdvanceApprovelName(mappingQuery);
            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
                
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;

            }

            return View(mappingQuery);


        }

        public async Task<IActionResult> NotConfirmedForApprovalAdvance()
        {

            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var manager = _employeeService.TGetById(managerId);
            var compName = manager.CompanyName;
            var user = _db.Personels.FirstOrDefault(u => u.Id == managerId);
            ViewBag.UserImageUrl = user?.ImageUrl;
            ViewBag.UserFullName = user?.FullName;


            var query = _individualAdvanceService.GetAllIndividualAdvanceReq().Where(x => x.ApprovalStatus == false);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();

            foreach (var item in mappingQuery)
            {
                //item.LeaveTypeName = (_LeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                //item.ImgLink = (_employeeService.TGetById(item.EmployeeRequestingId)).ImageUrl;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;

            }

            return View(mappingQuery);
        }

    }
}
