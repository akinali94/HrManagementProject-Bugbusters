using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete.ExpenditureConcreteServices
{
    public class ExpenditureTyoeService : Service<ExpenditureType>, IExpenditureTypeService
    {
        protected readonly IExpenditureTypeRepository _typeRepository;
        public ExpenditureTyoeService(IBaseRepository<ExpenditureType> repository, IUnitOfWork unitOfWork, HrDb db, IExpenditureTypeRepository typeRepository) : base(repository, unitOfWork, db)
        {
            _typeRepository = typeRepository;
        }

        public IEnumerable<ExpenditureType> GetAllExType()
        {
            return _typeRepository.GetAllExType();
        }

        public ExpenditureType GetByIdExpenditureType(int id)
        {
            return _typeRepository.GetByIdExpenditureType(id);
        }
    }
}
