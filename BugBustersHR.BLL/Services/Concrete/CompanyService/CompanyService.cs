using BugBustersHR.BLL.Services.Abstract.CompanyService;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.DAL.Repository.Abstract.CompanyRepos;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.ENTITY.Concrete;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete.CompanyService
{
    public class CompanyService : Service<Companies>, ICompanyService
    {
        protected readonly ICompanyRepository _companyRepository;
        public CompanyService(IBaseRepository<Companies> repository, IUnitOfWork unitOfWork, HrDb db, ICompanyRepository companyRepository) : base(repository, unitOfWork, db)
        {
            _companyRepository = companyRepository;
        }

        public IEnumerable<Companies> GetAllCompany()
        {
            return _companyRepository.GetAllCompany();
        }

        public Companies GetByIdCompany(int id)
        {
            return _companyRepository.GetByIdCompany(id); 
               
        }
    }
}
