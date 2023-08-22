using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract.InstitutionalAllowanceRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete.InstitutionalAllowanceConcreteRepos
{
    public class InstitutionalAllowanceTypeRepository : BaseRepository<InstitutionalAllowanceType>, IInstitutionalAllowanceTypeRepository
    {
        public InstitutionalAllowanceTypeRepository(HrDb hrDb) : base(hrDb)
        {
        }

        public IEnumerable<InstitutionalAllowanceType> GetAllInsAllType()
        {
            return _hrDb.InstitutionalAllowanceTypes.ToList();
        }

        public InstitutionalAllowanceType GetByIdInsAllType(int id)
        {
            return _hrDb.InstitutionalAllowanceTypes.Find(id);
        }
    }
}
