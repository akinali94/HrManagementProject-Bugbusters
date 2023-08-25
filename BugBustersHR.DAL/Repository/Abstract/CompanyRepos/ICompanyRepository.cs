using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.CompanyRepos
{
    public interface ICompanyRepository:IBaseRepository<Companies>
    {
        Companies GetByIdCompany(int id);
        IEnumerable<Companies> GetAllCompany();
    }
}
