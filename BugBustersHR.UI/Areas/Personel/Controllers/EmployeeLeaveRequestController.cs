using AutoMapper;
using BugBustersHR.BLL.Services.Abstract.LeaveAbstractService;
using BugBustersHR.BLL.Validatons.LeaveValidations;
using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{


    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]
    public class EmployeeLeaveRequestController : Controller
    {
        private readonly IEmployeeLeaveRequestService _employeeLeaveRequestService;
        private readonly IEmployeeLeaveTypeService _employeeLeaveTypeService;
        private readonly EmployeeLeaveRequestVM _employeeLeaveRequestVM;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeLeaveRequestVM> _validator;
        private readonly HrDb _hrDb;

        public EmployeeLeaveRequestController(IEmployeeLeaveRequestService employeeLeaveRequestService, IEmployeeLeaveTypeService employeeLeaveTypeService, EmployeeLeaveRequestVM employeeLeaveRequestVM, IMapper mapper, IValidator<EmployeeLeaveRequestVM> validator, HrDb hrDb)
        {
            _employeeLeaveRequestService = employeeLeaveRequestService;
            _employeeLeaveTypeService = employeeLeaveTypeService;
            _employeeLeaveRequestVM = employeeLeaveRequestVM;
            _mapper = mapper;
            _validator = validator;
            _hrDb = hrDb;
        }
        [NonAction]
        private void SetUserImageViewBag()
        {
            var qury2 = _employeeLeaveRequestService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.UserImageUrl = qury2?.ImageUrl;
            ViewBag.UserFullName = qury2?.FullName;

        }
        [NonAction]
        private Employee GetEmployee()
        {
            return _employeeLeaveRequestService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }



        public async Task<IActionResult> Index()
        {
            var query = _employeeLeaveRequestService.GetAllLeaveReq().Where(x => x.RequestingId == GetEmployee().Id);
            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(query);

            foreach (var item in mappingQuery) _employeeLeaveRequestService.GetLeaveTypeName(item);
            foreach (var item in mappingQuery) _employeeLeaveRequestService.GetLeaveApprovelName(item);

            SetUserImageViewBag();
            return View(mappingQuery);

        }



        public async Task<IActionResult> Create()
        {
            var selectedGender = GetEmployee().Gender;
            var leaveTypeList = _hrDb.EmployeeLeaveTypes
                .Where(leaveType => leaveType.Gender == selectedGender)
                .ToList();

            SetUserImageViewBag();

            ViewData["SelectedLeaveTypeId"] = new SelectList(leaveTypeList, "Id", "Name");
            ViewBag.LeaveTypeList = Enum.GetValues(typeof(GenderType))
                .Cast<GenderType>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                });

            return View();





        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,DateRequest,Gender,SelectedLeaveTypeId, NumberOfDaysOff")] EmployeeLeaveRequestVM employeeLeaveRequest)
        {

            var validate = _validator.Validate(employeeLeaveRequest);

            var user = _employeeLeaveRequestService.GetByIdEmployee(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var selectedGender = user.Gender;
            var leaveTypeList = _hrDb.EmployeeLeaveTypes
         .Where(leaveType => leaveType.Gender == selectedGender)
         //.Select(leaveType => leaveType.Id)
         .ToList();


            if (validate.IsValid)
            {

                var numberofdays = _employeeLeaveTypeService.GetByIdType(employeeLeaveRequest.NumberOfDaysOff);

                employeeLeaveRequest.RequestingId = User.FindFirstValue(ClaimTypes.NameIdentifier);
     

                var mappingQuery = _mapper.Map<EmployeeLeaveRequest>(employeeLeaveRequest);

                await _hrDb.AddAsync(mappingQuery);
                await _hrDb.SaveChangesAsync();
                ViewBag.UserImageUrl = user?.ImageUrl;
                ViewBag.UserFullName = user?.FullName;

                return RedirectToAction(nameof(Index));
            }



            ViewData["SelectedLeaveTypeId"] = new SelectList(leaveTypeList, "Id", "Name");

            ViewData["SelectedLeaveTypeId"] = new SelectList(_hrDb.EmployeeLeaveTypes, "Id", "Name", /*employeeLeaveRequest.EmployeeType.Name ,*/ employeeLeaveRequest.SelectedLeaveTypeId);
            ViewBag.LeaveTypeList = Enum.GetValues(typeof(GenderType))
              .Cast<GenderType>()
              .Select(c => new SelectListItem
              {
                  Value = c.ToString(),
                  Text = c.ToString()
              });

            SetUserImageViewBag();

            return View(employeeLeaveRequest);



        }

        public IActionResult Delete(int id)
        {

            var personel = _employeeLeaveRequestService.GetByIdRequest(id);

            var mapli = _mapper.Map<EmployeeLeaveRequestVM>(personel);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(EmployeeLeaveRequestVM typeVm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();

            _employeeLeaveRequestService.TDelete(_mapper.Map<EmployeeLeaveRequest>(typeVm));
            return RedirectToAction("Index");


        }
        public IActionResult WaitingForApprovalLeave()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _employeeLeaveRequestService.GetAllLeaveReq().Where(x => x.RequestingId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.LeaveTypeName = (_employeeLeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
            }

            foreach (var item in mappingQuery) _employeeLeaveRequestService.GetLeaveApprovelName(item);
            var waitingForApprovalLeave = mappingQuery.Where(item => item.Approved == null);

            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();
            return View(waitingForApprovalLeave);
        }

        public IActionResult ConfirmedApprovalLeave()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _employeeLeaveRequestService.GetAllLeaveReq().Where(x => x.RequestingId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.LeaveTypeName = (_employeeLeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
            }

            foreach (var item in mappingQuery) _employeeLeaveRequestService.GetLeaveApprovelName(item);


            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();
            var confirmedLeave = mappingQuery.Where(item => item.Approved == true);
            return View(confirmedLeave);

        }

        public IActionResult NotConfirmedApprovalLeave()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _employeeLeaveRequestService.GetAllLeaveReq().Where(x => x.RequestingId == userId);
            var mappingQuery = _mapper.Map<IEnumerable<EmployeeLeaveRequestVM>>(query);

            foreach (var item in mappingQuery)
            {
                item.LeaveTypeName = (_employeeLeaveTypeService.GetByIdType(item.SelectedLeaveTypeId)).Name;
            }
            foreach (var item in mappingQuery) _employeeLeaveRequestService.GetLeaveApprovelName(item);

            var notConfirmedLeave = mappingQuery.Where(item => item.Approved == false);
            var user = _hrDb.Personels.FirstOrDefault(u => u.Id == userId);
            SetUserImageViewBag();

            return View(notConfirmedLeave);

        }
    }
}

