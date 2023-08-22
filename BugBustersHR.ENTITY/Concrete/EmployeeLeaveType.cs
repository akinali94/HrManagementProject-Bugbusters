using BugBustersHR.ENTITY.Abstract;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class EmployeeLeaveType:BaseEntity
    {
        public string Name { get; set; }
        public int DefaultDay { get; set; }
        public GenderType Gender { get; set; }
    }
}
