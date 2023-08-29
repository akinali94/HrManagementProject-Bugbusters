using BugBustersHR.BLL.ViewModels.BaseViewModel;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.CompanyViewModel
{
    public class CompanyDetailsVM:BaseVM
    {
        public CompanyDetailsVM()
        {

            ImageModel = new ImageModel();
        }
        public string CompanyName { get; set; }
        public CompanyTitle CompanyTitle { get; set; }
        public ImageModel ImageModel { get; set; }
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

        public bool IsActive
        {
            get
            {
                if (ContractEndDate<=DateTime.Now)
                {
                    return true;
                }
                else
                { return false; }
            }
        }

    }
}
