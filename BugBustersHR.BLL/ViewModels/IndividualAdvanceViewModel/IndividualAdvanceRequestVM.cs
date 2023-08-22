using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel
{
    public class IndividualAdvanceRequestVM
    {
       

        public int Id { get; set; }
        public string RequestingId { get; set; }

        public EmployeeVM EmployeeType { get; set; }
        public string? FullName { get; set; }
        public Currency? Currency { get; set; }
        public string Explanation { get; set; }
        public string ApprovalStatusName { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime ApprovalDate { get; set; }

        public bool? ApprovalStatus { get; set; }

        public string EmployeeRequestingId { get; set; }

        public Employee Employee { get; set; }
        public string? ImgLink { get; set; }




        public decimal Amount { get; set; }


     


        public decimal Remain { get; set; }


        public EmployeeVM EmployeeVM { get; set; }

        public string? CompanyName { get; set; }

    }
}
