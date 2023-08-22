using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.LeaveRequestViewModel
{
    public class EmployeeLeaveRequestVM
    {
        public int Id { get; set; }
        public ImageModel ImageModel { get; set; }
        public string ImageUrl { get; set; } // Eklediğimiz özellik
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string? CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateRequest { get; set; } = DateTime.Now;
        public bool? Approved { get; set; }
        public DateTime? LeaveApprovalDate { get; set; }
        public string? LeaveTypeName { get; set; }
        public string? LeaveApprovalStatusName { get; set; }

        public int NumberOfDaysOff
        {
            get
            {
                return (EndDate - StartDate).Days + 1;
            }
        }

        public GenderType Gender { get; set; }

        //Diğer tablolarla ilişkiler
        public int SelectedLeaveTypeId { get; set; }
        public EmployeeLeaveTypeVM EmployeeLeaveRequest { get; set; }


        public string RequestingId { get; set; }

        public EmployeeVM EmployeeType { get; set; }

    }
}
