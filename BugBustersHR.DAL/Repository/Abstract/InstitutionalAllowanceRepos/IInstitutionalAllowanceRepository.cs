using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.InstitutionalAllowanceRepos
{
    public interface IInstitutionalAllowanceRepository : IBaseRepository<InstitutionalAllowance>
    {
        InstitutionalAllowance GetByIdInsAllowance(int id);

        IEnumerable<InstitutionalAllowance> GetAllInsAllowance();

        Task ChangeToTrueforAllowance(int id);
        Task ChangeToFalseforAllowance(int id);

    }
}
