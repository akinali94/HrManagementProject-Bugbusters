using BugBustersHR.BLL.ViewModels.BaseViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.ManagerViewModel
{
    public class GetEmployeeListVM
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string Section { get; set; }
        public string Title { get; set; }
    }
}
