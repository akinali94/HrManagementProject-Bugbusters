using AutoMapper;
using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.Validatons.ExpanditureValidations;
using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]
    public class ExpenditureTypeController : Controller
    {

        private readonly IExpenditureTypeService _service;
        private readonly IMapper _mapper;
        private readonly HrDb _context;

        public ExpenditureTypeController(IExpenditureTypeService service, IMapper mapper, HrDb context)
        {
            _service = service;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
           var query = _service.GetAllExType();

            var mappingQuery = _mapper.Map<List<ExpenditureTypeVM>>(query);

            return View(mappingQuery);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExpenditureTypeCreateVM typeVm)
        {
            ExpanditureTypeValidator validator = new ExpanditureTypeValidator();
            var validationResult = validator.Validate(typeVm);

            if (validationResult.IsValid)
            {
                var query = _mapper.Map<ExpenditureType>(typeVm);
                await _service.TAddAsync(query);
                return RedirectToAction("Index");
            }

            return View(typeVm);
        }


        public IActionResult Delete(int id)
        {

            var type = _service.GetByIdExpenditureType(id);

            var mapli = _mapper.Map<ExpenditureTypeVM>(type);

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(ExpenditureTypeVM typeVm)
        {



            _service.TDelete(_mapper.Map<ExpenditureType>(typeVm));
            return RedirectToAction("Index");


        }


    }
}
