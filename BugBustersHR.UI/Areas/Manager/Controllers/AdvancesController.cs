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

            var compName = GetEmployee().CompanyName;

            var advancesList = _individualAdvanceService.GetAllIndividualAdvanceReq();
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);


            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(advancesList)
                .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();

            _individualAdvanceService.GetAdvanceApprovelName(mappingQuery);
            foreach (var item in mappingQuery)
            {
                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;

            }
            SetUserImageViewBag();

            ViewBag.ManagerComp = userCompany;

            return View(mappingQuery);
        }

        public async Task<IActionResult> AdvanceAccepted(int id)
        {
            var adv = _db.IndividualAdvances.Find(id);
            var user = _employeeService.TGetById(adv.EmployeeRequestingId);
            var manager = GetEmployee();

            await _individualAdvanceService.TChangeToTrueforAdvance(id);
            await _emailService.AdvanceRequestApprovedMail(user.Email, "confirmed", user.FullName, manager, adv);

            SetUserImageViewBag();
            return RedirectToAction("Index", "Advances", new { area = "Manager" });
        }

        public async Task<IActionResult> AdvanceRefused(int id)
        {
            var adv = _db.IndividualAdvances.Find(id);
            var user = _employeeService.TGetById(adv.EmployeeRequestingId);
            var manager = GetEmployee();



            await _individualAdvanceService.TChangeToFalseforAdvance(id);

            await _emailService.AdvanceRequestApprovedMail(user.Email, "refused", user.FullName, manager, adv);
            SetUserImageViewBag();
            return RedirectToAction("Index", "Advances", new { area = "Manager" });
        }

        public async Task<IActionResult> WaitingForApprovalAdvance()
        {


            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _individualAdvanceService.GetAllIndividualAdvanceReq().Where(x => x.ApprovalStatus == null);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();
            _individualAdvanceService.GetAdvanceApprovelName(mappingQuery);
            foreach (var item in mappingQuery)
            {
               
                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;

            }

            return View(mappingQuery);


        }

        public async Task<IActionResult> ConfirmedApprovalAdvance()
        {

            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _individualAdvanceService.GetAllIndividualAdvanceReq().Where(x => x.ApprovalStatus == true);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();
            _individualAdvanceService.GetAdvanceApprovelName(mappingQuery);
            foreach (var item in mappingQuery)
            {
              
                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;

            }

            return View(mappingQuery);


        }

        public async Task<IActionResult> NotConfirmedForApprovalAdvance()
        {

            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _individualAdvanceService.GetAllIndividualAdvanceReq().Where(x => x.ApprovalStatus == false);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<IndividualAdvanceRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeRequestingId)).ToList();

            foreach (var item in mappingQuery)
            {

                item.FullName = (_employeeService.TGetById(item.EmployeeRequestingId)).FullName;
 
                item.CompanyName = (_employeeService.TGetById(item.EmployeeRequestingId)).CompanyName;

            }

            return View(mappingQuery);
        }
        [NonAction]
        private void SetUserImageViewBag()
        {


            var adminID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var admin = _db.Personels.FirstOrDefault(u => u.Id == adminID);
            ViewBag.UserImageUrl = admin?.ImageUrl;
            ViewBag.UserFullName = admin?.FullName;


        }
        [NonAction]
        private Employee GetEmployee()
        {
            return _individualAdvanceService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
