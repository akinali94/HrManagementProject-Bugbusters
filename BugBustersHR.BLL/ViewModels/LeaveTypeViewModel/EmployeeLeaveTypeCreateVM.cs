using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.LeaveTypeViewModel
{
    public class EmployeeLeaveTypeCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DefaultDay { get; set; }
        public GenderType Gender { get; set; }
    }
}
