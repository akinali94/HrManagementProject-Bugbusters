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
    public class EmployeeLeaveTypeService : Service<EmployeeLeaveType>, IEmployeeLeaveTypeService
    {
        protected readonly IEmployeeLeaveTypeRepository _employeeLeaveTypeRepository;
        public EmployeeLeaveTypeService(IBaseRepository<EmployeeLeaveType> repository, IUnitOfWork unitOfWork, HrDb db, IEmployeeLeaveTypeRepository employeeLeaveTypeRepository) : base(repository, unitOfWork, db)
        {
            _employeeLeaveTypeRepository = employeeLeaveTypeRepository;
        }

        public IEnumerable<EmployeeLeaveType> GetAllLeaveType()
        {
            return _employeeLeaveTypeRepository.GetAllLeaveType();
        }

        public EmployeeLeaveType GetByIdType(int id)
        {
            return _employeeLeaveTypeRepository.GetByIdType(id);
        }
    }
}

