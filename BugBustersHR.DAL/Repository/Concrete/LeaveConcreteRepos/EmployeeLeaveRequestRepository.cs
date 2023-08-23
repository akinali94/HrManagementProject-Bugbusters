using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.ENTITY.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete.LeaveConcreteRepos
{
    public class EmployeeLeaveRequestRepository : BaseRepository<EmployeeLeaveRequest>, IEmployeeLeaveRequestRepository
    {
        public EmployeeLeaveRequestRepository(HrDb hrDb) : base(hrDb)
        {
        }

        public async Task ChangeToFalseforLeave(int id)
        {
            var values = await _hrDb.EmployeeLeaveRequests.FindAsync(id);
            values.Approved = false;
            _hrDb.Update(values);
            _hrDb.SaveChanges();

        }

        public async Task ChangeToTrueforLeave(int id)
        {
            var values = await _hrDb.EmployeeLeaveRequests.FindAsync(id);
            values.Approved = true;
            _hrDb.Update(values);
            _hrDb.SaveChanges();

        }

        public IEnumerable<EmployeeLeaveRequest> GetAllLeaveReq()
        {
            return _hrDb.EmployeeLeaveRequests.ToList();

        }

        public EmployeeLeaveRequest GetByIdRequest(int id)
        {
            return _hrDb.EmployeeLeaveRequests.Find(id);
        }

        //public async Task GetLeaveApprovelName(EmployeeLeaveRequest request)
        //{
        //    //var request = await _hrDb.EmployeeLeaveRequests.FindAsync(id);

        //    if (request.Approved == null)
        //    {
        //        request.LeaveApprovalStatusName = "Waiting for Approval";
        //    }
        //    else if (request.Approved == true)
        //    {
        //        request.LeaveApprovalStatusName = "Confirmed";
        //    }
        //    else
        //    {
        //        request.LeaveApprovalStatusName = "Not Confirmed";
        //    }
        //}
    }
}
