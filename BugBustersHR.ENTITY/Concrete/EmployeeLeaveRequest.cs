using BugBustersHR.ENTITY.Abstract;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class EmployeeLeaveRequest:BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateRequest { get; set; }

        public bool? Approved { get; set; }
        public DateTime? LeaveApprovalDate { get; set; }

        public string? LeaveTypeName { get; set; }
        [NotMapped]
        public string? LeaveApprovalStatusName { get; set; }

        //public bool Canceled{get;set;}
        public GenderType Gender { get; set; }
        public string RequestingId { get; set; }
        [ForeignKey("RequestingId")]
        public Employee Requesting { get; set; }





        public int SelectedLeaveTypeId { get; set; }
        public int NumberOfDaysOff
        {
            get
            {
                return (EndDate - StartDate).Days + 1;
            }
        }
        public EmployeeLeaveType SelectedLeaveType { get; set; }


       
    }
}
