using BugBustersHR.ENTITY.Abstract;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class IndividualAdvance:BaseEntity
    {
       
        public Currency? Currency { get; set; }
        public string Explanation { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string? ApprovalStatusName { get; set; }
        public bool? ApprovalStatus { get; set; }

        

        public decimal Amount { get; set; }


        public decimal Remain { get; set; }

    
        public string EmployeeRequestingId { get; set; }
        public Employee Employee { get; set; }

    }
}
