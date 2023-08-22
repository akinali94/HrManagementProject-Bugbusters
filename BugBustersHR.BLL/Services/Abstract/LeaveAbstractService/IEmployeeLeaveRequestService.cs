using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract.LeaveAbstractService
{
    public interface IEmployeeLeaveRequestService : IService<EmployeeLeaveRequest>
    {
        EmployeeLeaveRequest GetByIdRequest(int id);
        IEnumerable<EmployeeLeaveRequest> GetAllLeaveReq();

        Task TChangeToTrueforLeave(int id);
        Task TChangeToFalseforLeave(int id);

    }
}
