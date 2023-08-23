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
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using AutoMapper;

namespace BugBustersHR.BLL.Services.Concrete.LeaveConcreteService
{
    public class EmployeeLeaveRequestService : Service<EmployeeLeaveRequest>,IEmployeeLeaveRequestService
    {
        protected readonly IEmployeeLeaveRequestRepository _employeeLeaveRequestRepository;
        protected readonly IMapper _mapper;
        private readonly IEmployeeLeaveTypeService _employeeLeaveTypeService;
        public EmployeeLeaveRequestService(IBaseRepository<EmployeeLeaveRequest> repository, IUnitOfWork unitOfWork, HrDb db, IEmployeeLeaveRequestRepository employeeLeaveRequestRepository, IMapper mapper, IEmployeeLeaveTypeService employeeLeaveTypeService) : base(repository, unitOfWork, db)
        {
            _employeeLeaveRequestRepository = employeeLeaveRequestRepository;
            _mapper = mapper;
            _employeeLeaveTypeService = employeeLeaveTypeService;
        }

        public IEnumerable<EmployeeLeaveRequest> GetAllLeaveReq()
        {
            return _employeeLeaveRequestRepository.GetAllLeaveReq();
        }

        public EmployeeLeaveRequest GetByIdRequest(int id)
        {
            return _employeeLeaveRequestRepository.GetByIdRequest(id);
        }

        public async Task GetLeaveApprovelName(EmployeeLeaveRequestVM request)
        {
            if (request.Approved == null)
            {
                request.LeaveApprovalStatusName = "Waiting for Approval";
            }
            else if (request.Approved == true)
            {
                request.LeaveApprovalStatusName = "Confirmed";
            }
            else
            {
                request.LeaveApprovalStatusName = "Not Confirmed";
            }
        }

        public async Task GetLeaveTypeName(EmployeeLeaveRequestVM request)
        {
            request.LeaveTypeName = (_employeeLeaveTypeService.GetByIdType(request.SelectedLeaveTypeId)).Name;
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
