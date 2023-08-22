using BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.DAL.Repository.Abstract.InstitutionalAllowanceRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete.InstitutionalAllowanceConcreteServices
{
    public class InstitutionalAllowanceService : Service<InstitutionalAllowance>, IInstitutionalAllowanceService
    {
        protected readonly IInstitutionalAllowanceRepository _allowanceRepository;



        public InstitutionalAllowanceService(IBaseRepository<InstitutionalAllowance> repository, IUnitOfWork unitOfWork, HrDb db, IInstitutionalAllowanceRepository allowanceRepository) : base(repository, unitOfWork, db)
        {
            _allowanceRepository = allowanceRepository;
        }

        public IEnumerable<InstitutionalAllowance> GetAllInstitutionalAllowances()
        {
            return _allowanceRepository.GetAllInsAllowance();
        }

        public InstitutionalAllowance GetByIdInstitutionalAllowance(int id)
        {
            return _allowanceRepository.GetByIdInsAllowance(id);
        }

        public async Task TChangeToFalseforAllowance(int id)
        {
            await _allowanceRepository.ChangeToFalseforAllowance(id);
        }

        public async Task TChangeToTrueforAllowance(int id)
        {
            await _allowanceRepository.ChangeToTrueforAllowance(id);
        }
    }
}
