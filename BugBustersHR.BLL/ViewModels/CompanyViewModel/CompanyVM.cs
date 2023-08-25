using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.CompanyViewModel
{
    public class CompanyVM
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public CompanyTitle CompanyTitle { get; set; }
        public string MersisNo { get; set; }
        public string TaxNumber { get; set; }
        public string Logo { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime FoundationYear { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }

        public bool IsActive { get; set; } 

    }
}
