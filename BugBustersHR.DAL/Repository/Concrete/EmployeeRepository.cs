using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.ENTITY.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HrDb context) : base(context)
        {

        }

        public Employee GetEmployeeById(string employeeId)
        {
            return _hrDb.Personels.FirstOrDefault(e => e.Id == employeeId);
        }

        public void UpdateEmployee(Employee employee)
        {
            _hrDb.Personels.Update(employee);
            _hrDb.SaveChanges();
        }
    }
}
