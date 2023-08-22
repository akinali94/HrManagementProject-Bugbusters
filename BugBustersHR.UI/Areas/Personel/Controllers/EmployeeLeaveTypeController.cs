using AutoMapper;
using BugBustersHR.BLL.Services.Abstract.LeaveAbstractService;
using BugBustersHR.BLL.Validatons.LeaveValidations;
using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]
    public class EmployeeLeaveTypeController : Controller
    {
        private readonly IEmployeeLeaveTypeService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeLeaveTypeVM> _employeeLeaveType;

        public EmployeeLeaveTypeController(IEmployeeLeaveTypeService service, IMapper mapper, IValidator<EmployeeLeaveTypeVM> employeeLeaveType)
        {
            _service = service;
            _mapper = mapper;
            _employeeLeaveType = employeeLeaveType;
        }

        public async Task<IActionResult> Index()
        {
           



            var query =  _service.GetAllLeaveType();
            var mappingQuery = _mapper.Map<List<EmployeeLeaveTypeVM>>(query);

            return View(mappingQuery);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeLeaveTypeVM typeVm)
        {
            EmployeeLeaveTypeValidator validator = new EmployeeLeaveTypeValidator();
            var validationResult = validator.Validate(typeVm);

            if (validationResult.IsValid)
            {
                var leaveType = _mapper.Map<EmployeeLeaveType>(typeVm);
                leaveType.Gender = typeVm.Gender; // Gender değeri atandı
                await _service.TAddAsync(leaveType);
                return RedirectToAction("Index");
                //await _service.TAddAsync(_mapper.Map<EmployeeLeaveType>(typeVm));
                //return RedirectToAction("Index");
            }

            return View(typeVm);
        }



        public IActionResult Edit(int id)
        {

            var personel = _service.GetByIdType(id);

            var mapli = _mapper.Map<EmployeeLeaveTypeVM>(personel);

            return View(mapli);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeLeaveTypeVM typeVm)
        {
            EmployeeLeaveTypeValidator validator = new EmployeeLeaveTypeValidator();
            var validationResult = validator.Validate(typeVm);

            if (validationResult.IsValid)
            {
                _service.TUpdate(_mapper.Map<EmployeeLeaveType>(typeVm));
                return RedirectToAction("Index");
            }

            return View(typeVm);
        }


        public IActionResult Delete(int id)
        {

            var leaveType = _service.GetByIdType(id);

            var mapli = _mapper.Map<EmployeeLeaveTypeVM>(leaveType);

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(EmployeeLeaveTypeVM typeVm)
        {



            _service.TDelete(_mapper.Map<EmployeeLeaveType>(typeVm));
            return RedirectToAction("Index");


        }

    }
}
