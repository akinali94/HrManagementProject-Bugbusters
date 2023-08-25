using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract.CompanyRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete.Company
{
    public class CompanyRepository : BaseRepository<Companies>, ICompanyRepository
    {
        public CompanyRepository(HrDb hrDb) : base(hrDb)
        {
        }

        public IEnumerable<Companies> GetAllCompany()
        {
            return _hrDb.Companies.ToList();
        }

        public Companies GetByIdCompany(int id)
        {
            return _hrDb.Companies.Find(id);
        }
    }
}
