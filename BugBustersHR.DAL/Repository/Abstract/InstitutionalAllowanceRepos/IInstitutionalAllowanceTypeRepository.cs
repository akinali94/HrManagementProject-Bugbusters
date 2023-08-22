using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.InstitutionalAllowanceRepos
{
    public interface IInstitutionalAllowanceTypeRepository : IBaseRepository<InstitutionalAllowanceType>
    {
        InstitutionalAllowanceType GetByIdInsAllType(int id);

        IEnumerable<InstitutionalAllowanceType> GetAllInsAllType();
    }
}
