using AutoMapper;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices;
using BugBustersHR.BLL.Services.Concrete.InstitutionalAllowanceConcreteServices;
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
        
            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();

            var allowanceList = _institutionalAllowanceService.GetAllInstitutionalAllowances();
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);
            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(allowanceList)
                .Where(allowance => userCompanyIds.Contains(allowance.EmployeeId)).ToList();
            foreach (var item in mappingQuery) _institutionalAllowanceService.GetInstAllApprovelName(item);

            foreach (var item in mappingQuery)
            {
              
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
     
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;


            }

      
            ViewBag.ManagerComp = userCompany;

            return View(mappingQuery);
        }

        public async Task<IActionResult> AllowanceAccepted(int id)
        {
            var allw = _db.InstitutionalAllowances.Find(id);
            var user = _db.Users.Find(allw.EmployeeId);
            await _institutionalAllowanceService.TChangeToTrueforAllowance(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "allowance");
            SetUserImageViewBag();
            return RedirectToAction("Index", "Allowance", new { area = "Manager" });
        }

        public async Task<IActionResult> AllowanceRefused(int id)
        {
            var allw = _db.InstitutionalAllowances.Find(id);
            var user = _db.Users.Find(allw.EmployeeId);
            await _institutionalAllowanceService.TChangeToFalseforAllowance(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "allowance");
            SetUserImageViewBag();
            return RedirectToAction("Index", "Allowance", new { area = "Manager" });
        }

        public async Task<IActionResult> WaitingForApprovalAllowance()
        {

            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _institutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.ApprovalStatus == null);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();
            foreach (var item in mappingQuery) _institutionalAllowanceService.GetInstAllApprovelName(item);
            foreach (var item in mappingQuery)
            {
              
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;

            }

            return View(mappingQuery);


        }

        public async Task<IActionResult> ConfirmedApprovalAllowance()
        {

     
            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _institutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.ApprovalStatus == true);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();
            foreach (var item in mappingQuery) _institutionalAllowanceService.GetInstAllApprovelName(item);
            foreach (var item in mappingQuery)
            {
          
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;

            }

            return View(mappingQuery);
        }

        public async Task<IActionResult> NotConfirmedApprovalAllowance()
        {

      
            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _institutionalAllowanceService.GetAllInstitutionalAllowances().Where(x => x.ApprovalStatus == false);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<InstitutionalAllowanceVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();
            foreach (var item in mappingQuery) _institutionalAllowanceService.GetInstAllApprovelName(item);
            foreach (var item in mappingQuery)
            {
              
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;

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
            return _institutionalAllowanceService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    }
}
