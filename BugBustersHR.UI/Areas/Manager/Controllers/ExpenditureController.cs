using AutoMapper;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.Services.Concrete;
using BugBustersHR.BLL.Services.Concrete.ExpenditureConcreteServices;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
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
    public class ExpenditureController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IExpenditureRequestService _expenditureService;
        private readonly IMapper _mapper;
        private readonly IExpenditureTypeService _expenditureTypeService;
        private readonly IEmailService _emailService;
        private readonly HrDb _db;

        public ExpenditureController(IEmployeeService employeeService, IExpenditureRequestService expenditureService, IMapper mapper, IEmailService emailService, HrDb db)
        {
            _employeeService = employeeService;
            _expenditureService = expenditureService;
            _mapper = mapper;
            _emailService = emailService;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
      
            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();
            var expList = _expenditureService.GetAllExReq();
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);
            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(expList)
                .Where(expenditure => userCompanyIds.Contains(expenditure.EmployeeId)).ToList();
            _expenditureService.GetExpenditureApprovelName(mappingQuery);

            foreach (var item in mappingQuery)
            {
                //item.TypeName = (_expenditureTypeService.GetByIdExpenditureType(item.ExpenditureTypeId)).ExpenditureName;
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                //item.Title = (_employeeService.TGetById(item.RequestingId)).Title;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;

            }

            ViewBag.ManagerComp = userCompany;

            return View(mappingQuery);
        }

        public async Task<IActionResult> ExpenditureAccepted(int id)
        {
            var exp = _db.ExpenditureRequests.Find(id);
            var user = _db.Users.Find(exp.EmployeeId);
            await _expenditureService.TChangeToTrueforExpenditure(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "expenditure");
            SetUserImageViewBag();
            return RedirectToAction("Index", "Expenditure", new { area = "Manager" });
        }

        public async Task<IActionResult> ExpenditureRefused(int id)
        {
            var exp = _db.ExpenditureRequests.Find(id);
            var user = _db.Users.Find(exp.EmployeeId);
            await _expenditureService.TChangeToFalseforExpenditure(id);
            await _emailService.RequestApprovedMail("aa", user.Email, "expenditure");
            SetUserImageViewBag();
            return RedirectToAction("Index", "Expenditure", new { area = "Manager" });
        }
        public async Task<IActionResult> WaitingForApprovalExpenditure()
        {

            //var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var manager = _employeeService.TGetById(managerId);
            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _expenditureService.GetAllExReq().Where(x => x.ApprovalStatus == null);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();
          _expenditureService.GetExpenditureApprovelName(mappingQuery);
            foreach (var item in mappingQuery)
            {
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;
            }

            return View(mappingQuery);
        }

        public async Task<IActionResult> ConfirmedApprovalExpenditure()
        {

            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();


            var query = _expenditureService.GetAllExReq().Where(x => x.ApprovalStatus == true);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);
            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();
          _expenditureService.GetExpenditureApprovelName(mappingQuery);

            foreach (var item in mappingQuery)
            {
                item.FullName = (_employeeService.TGetById(item.EmployeeId)).FullName;
                item.CompanyName = (_employeeService.TGetById(item.EmployeeId)).CompanyName;

            }

            return View(mappingQuery);
        }

        public async Task<IActionResult> NotConfirmedApprovalExpenditure()
        {

        
            var compName = GetEmployee().CompanyName;
            SetUserImageViewBag();

            var query = _expenditureService.GetAllExReq().Where(x => x.ApprovalStatus == false);
            var userCompany = await _employeeService.TGetAllAsync(x => x.CompanyName == compName);
            var userCompanyIds = userCompany.Select(user => user.Id);

            var mappingQuery = _mapper.Map<IEnumerable<ExpenditureRequestVM>>(query)
               .Where(advances => userCompanyIds.Contains(advances.EmployeeId)).ToList();
       _expenditureService.GetExpenditureApprovelName(mappingQuery);
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
            var qury2 = _expenditureService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.UserImageUrl = qury2?.ImageUrl;
            ViewBag.UserFullName = qury2?.FullName;

        }
        [NonAction]
        private Employee GetEmployee()
        {
            return _expenditureService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
