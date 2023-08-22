using BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.DAL.Repository.Abstract.InstitutionalAllowanceRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete.InstitutionalAllowanceConcreteServices
{
    public class InstitutionalAllowanceTypeService : Service<InstitutionalAllowanceType>,IInstitutionalAllowanceTypeService
    {
        protected readonly IInstitutionalAllowanceTypeRepository _typeRepository;
        public InstitutionalAllowanceTypeService(IBaseRepository<InstitutionalAllowanceType> repository, IUnitOfWork unitOfWork, HrDb db, IInstitutionalAllowanceTypeRepository typeRepository) : base(repository, unitOfWork, db)
        {
            _typeRepository = typeRepository;
        }

        public IEnumerable<InstitutionalAllowanceType> GetAllInstitutionalAllowanceTypes()
        {
            return _typeRepository.GetAllInsAllType();
        }

        public InstitutionalAllowanceType GetByIdInstitutionalAllowanceType(int id)
        {
            return _typeRepository.GetByIdInsAllType(id);
        }
    }
}
