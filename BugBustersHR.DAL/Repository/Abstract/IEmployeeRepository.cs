using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Employee GetEmployeeById(string employeeId);
        void UpdateEmployee(Employee employee);
    }

   
}
