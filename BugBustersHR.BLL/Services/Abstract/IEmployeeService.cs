using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract
{
    public interface IEmployeeService : IService<Employee>
    {
        string GenerateRandomPassword(PasswordOptions opts);
       
    }
}
