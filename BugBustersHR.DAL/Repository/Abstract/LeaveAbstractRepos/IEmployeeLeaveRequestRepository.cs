using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos
{
    public interface IEmployeeLeaveRequestRepository:IBaseRepository<EmployeeLeaveRequest>
    {
        EmployeeLeaveRequest GetByIdRequest(int id);
        IEnumerable<EmployeeLeaveRequest> GetAllLeaveReq();

        Task ChangeToTrueforLeave(int id);
        Task ChangeToFalseforLeave(int id);

        //Task GetLeaveApprovelName(EmployeeLeaveRequest request);

    }
}
