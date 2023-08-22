using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract.LeaveAbstractService
{
    public interface IEmployeeLeaveTypeService : IService<EmployeeLeaveType>
    {
        EmployeeLeaveType GetByIdType(int id);

        IEnumerable<EmployeeLeaveType> GetAllLeaveType();
    }
}
