using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete.ExpenditureConcreteRepos
{
    public class ExpenditureTypeRepository : BaseRepository<ExpenditureType>, IExpenditureTypeRepository
    {
        public ExpenditureTypeRepository(HrDb hrDb) : base(hrDb)
        {
        }

        public IEnumerable<ExpenditureType> GetAllExType()
        {
            return _hrDb.ExpenditureTypes.ToList();
        }

        public ExpenditureType GetByIdExpenditureType(int id)
        {
           return _hrDb.ExpenditureTypes.Find(id);
        }
    }
}
