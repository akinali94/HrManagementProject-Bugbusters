using AutoMapper;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceTypeViewModel;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using BugBustersHR.BLL.ViewModels.ManagerViewModel;
using BugBustersHR.DAL.Repository.Abstract.InstitutionalAllowanceRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BugBustersHR.BLL.ViewModels.AdminViewModel;
using BugBustersHR.BLL.ViewModels.CompanyViewModel;

namespace BugBustersHR.BLL.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<EmployeeListWithoutSalaryVM, Employee>().ReverseMap();
            CreateMap<EmployeeVM, Employee>().ReverseMap();
            CreateMap<EmployeeSummaryListVM, Employee>().ReverseMap();
            CreateMap<EmployeeUpdateVM, Employee>().ReverseMap();
            CreateMap<ExpenditureRequest,ExpenditureRequestCreateVM>().ReverseMap();
            CreateMap<ExpenditureRequest,ExpenditureRequestVM>().ReverseMap();
            CreateMap<ExpenditureType, ExpenditureTypeVM>().ReverseMap();
            CreateMap<ExpenditureType, ExpenditureTypeCreateVM>().ReverseMap();
            CreateMap<ExpenditureType, ExpenditureRequestListVM>().ReverseMap();
            CreateMap<EmployeeLeaveTypeVM, EmployeeLeaveType>().ReverseMap();
            CreateMap<EmployeeLeaveTypeCreateVM, EmployeeLeaveType>().ReverseMap();
            CreateMap<EmployeeLeaveRequestVM, EmployeeLeaveRequest>().ReverseMap();
            CreateMap<EmployeeLeaveRequestCreateVM, EmployeeLeaveType>().ReverseMap();
            CreateMap<IndividualAdvanceRequestVM, IndividualAdvance>().ReverseMap();
            CreateMap<ViewModels.ExpenditureTypeViewModel.EmployeeLeaveRequestListVM, EmployeeLeaveType>().ReverseMap();

            CreateMap<ManagerVM, Employee>().ReverseMap();
            CreateMap<ManagerUpdateVM, Employee>().ReverseMap();
            CreateMap<ManagerSummaryListVM, Employee>().ReverseMap();
            CreateMap<ManagerListWithoutSalaryVM, Employee>().ReverseMap();
            CreateMap<CreateEmployeeFromManagerVM, Employee>().ReverseMap();

            CreateMap<GetEmployeeListVM, Employee>().ReverseMap();
            CreateMap<InstitutionalAllowance, InstitutionalAllowanceVM>().ReverseMap();
            CreateMap<InstitutionalAllowanceType, InstitutionalAllowanceTypeVM>().ReverseMap();

            CreateMap<AdminVM, Employee>().ReverseMap();
            CreateMap<AdminUpdateVM, Employee>().ReverseMap();
            CreateMap<AdminSummaryListVM, Employee>().ReverseMap();    
            CreateMap<AdminListWithoutSalaryVM, Employee>().ReverseMap();
            CreateMap<CreateManagerFromAdminVM, Employee>().ReverseMap();
            CreateMap<GetManagerListVM, Employee>().ReverseMap();

            CreateMap<GetManagerListVM, Employee>().ReverseMap();

            CreateMap<CompanyVM,Companies>().ReverseMap();    
            CreateMap<CompanyDetailsVM,Companies>().ReverseMap();    

        }
    }
}
