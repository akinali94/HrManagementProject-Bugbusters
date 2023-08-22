using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos
{
    public interface IEmployeeLeaveTypeRepository : IBaseRepository<EmployeeLeaveType>
    {
        EmployeeLeaveType GetByIdType(int id);
        IEnumerable<EmployeeLeaveType> GetAllLeaveType();


    }
}
