using BugBustersHR.BLL.Services.Abstract.LeaveAbstractService;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete.LeaveConcreteService
{
    public class EmployeeLeaveRequestService : Service<EmployeeLeaveRequest>, IEmployeeLeaveRequestService
    {
        protected readonly IEmployeeLeaveRequestRepository _employeeLeaveRequestRepository;
        public EmployeeLeaveRequestService(IBaseRepository<EmployeeLeaveRequest> repository, IUnitOfWork unitOfWork, HrDb db, IEmployeeLeaveRequestRepository employeeLeaveRequestRepository) : base(repository, unitOfWork, db)
        {
            _employeeLeaveRequestRepository = employeeLeaveRequestRepository;
        }

        public IEnumerable<EmployeeLeaveRequest> GetAllLeaveReq()
        {
            return _employeeLeaveRequestRepository.GetAllLeaveReq();
        }

        public EmployeeLeaveRequest GetByIdRequest(int id)
        {
            return _employeeLeaveRequestRepository.GetByIdRequest(id);
        }

        public async Task TChangeToFalseforLeave(int id)
        {
            await _employeeLeaveRequestRepository.ChangeToFalseforLeave(id);

        }

        public async Task TChangeToTrueforLeave(int id)
        {
            await _employeeLeaveRequestRepository.ChangeToTrueforLeave(id);
        }
    }
}
