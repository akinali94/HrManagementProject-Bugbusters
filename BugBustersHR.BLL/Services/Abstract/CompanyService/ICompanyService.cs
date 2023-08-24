using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract.CompanyService
{
    public interface ICompanyService:IService<Companies>
    {
        Companies GetByIdCompany(int id);
        IEnumerable<Companies> GetAllCompany();
    }
}
