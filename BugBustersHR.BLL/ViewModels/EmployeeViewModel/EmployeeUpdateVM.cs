using BugBustersHR.BLL.ViewModels.BaseViewModel;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.EmployeeViewModel
{
    public class EmployeeUpdateVM : BaseVM
    {
        public EmployeeUpdateVM()
        {
            ImageModel = new ImageModel();
        }

        public string ImageUrl { get; set; }
        public ImageModel BackgroundImageModel { get; set; }

        public string TelephoneNumber { get; set; }

        public string Address { get; set; }
        public string FullName { get; set; }

        public ImageModel ImageModel { get; set; }

    }
}
