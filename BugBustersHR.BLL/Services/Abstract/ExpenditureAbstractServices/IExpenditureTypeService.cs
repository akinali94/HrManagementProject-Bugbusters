using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices
{
    public interface IExpenditureTypeService : IService<ExpenditureType>
    {
        ExpenditureType GetByIdExpenditureType(int id);
        IEnumerable<ExpenditureType> GetAllExType();
    }
}
