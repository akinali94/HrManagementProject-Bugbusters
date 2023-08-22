using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos
{
    public interface IExpenditureTypeRepository : IBaseRepository<ExpenditureType>
    {
        ExpenditureType GetByIdExpenditureType(int id);

        IEnumerable<ExpenditureType> GetAllExType();
    }
}
