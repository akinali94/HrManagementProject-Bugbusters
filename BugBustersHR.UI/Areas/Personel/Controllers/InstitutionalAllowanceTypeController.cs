using AutoMapper;
using BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices;
using BugBustersHR.BLL.Validatons.ExpanditureValidations;
using BugBustersHR.BLL.Validatons.InstitutionalAllowanceValidations;
using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceTypeViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BugBustersHR.UI.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Authorize(Roles = AppRoles.Role_Employee)]
    public class InstitutionalAllowanceTypeController : Controller
    {
     
        private readonly IInstitutionalAllowanceTypeService _institutionalAllowanceTypeService;
        private readonly IMapper _mapper;
        private readonly HrDb _context;

        public InstitutionalAllowanceTypeController(IMapper mapper, HrDb context, IInstitutionalAllowanceTypeService institutionalAllowanceTypeService)
        {

            _mapper = mapper;
            _context = context;
            _institutionalAllowanceTypeService = institutionalAllowanceTypeService;
        }

        public IActionResult Index()
        {
            var query = _institutionalAllowanceTypeService.GetAllInstitutionalAllowanceTypes();
            var mappingQuery = _mapper.Map<List<InstitutionalAllowanceTypeVM>>(query);
            return View(mappingQuery);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(InstitutionalAllowanceTypeVM type)
        {
            //ExpanditureTypeValidator validator = new ExpanditureTypeValidator();

            InstitutionalAllowanceTypeValidator validator = new InstitutionalAllowanceTypeValidator();
            var validationResult = validator.Validate(type);

            if (validationResult.IsValid)
            {
                var query = _mapper.Map<InstitutionalAllowanceType>(type);
                await _institutionalAllowanceTypeService.TAddAsync(query);
                return RedirectToAction("Index");
            }

            return View(type);
        }


        public IActionResult Delete(int id)
        {

            var type = _institutionalAllowanceTypeService.GetByIdInstitutionalAllowanceType(id);

            var mapli = _mapper.Map<InstitutionalAllowanceTypeVM>(type);

            return View(mapli);
        }

        [HttpPost]
        public IActionResult Delete(InstitutionalAllowanceTypeVM typeVm)
        {



            _institutionalAllowanceTypeService.TDelete(_mapper.Map<InstitutionalAllowanceType>(typeVm));
            return RedirectToAction("Index");


        }












    }
}
