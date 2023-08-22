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
    public class EmployeeLeaveTypeRepository : BaseRepository<EmployeeLeaveType>, IEmployeeLeaveTypeRepository
    {
        public EmployeeLeaveTypeRepository(HrDb hrDb) : base(hrDb)
        {
            
        }
        public IEnumerable<EmployeeLeaveType> GetAllLeaveType()
        {
            return _hrDb.EmployeeLeaveTypes.ToList();
        }

        public EmployeeLeaveType GetByIdType(int id)
        {
            return _hrDb.EmployeeLeaveTypes.Find(id);
        }
    }
}
