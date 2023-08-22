using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.ManagerViewModel
{
    public class ManagerSummaryListVM
    {
        public ManagerSummaryListVM()
        {
            ImageModel = new ImageModel();
        }

        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Section { get; set; }
        public ImageModel ImageModel { get; set; }
    }
}
