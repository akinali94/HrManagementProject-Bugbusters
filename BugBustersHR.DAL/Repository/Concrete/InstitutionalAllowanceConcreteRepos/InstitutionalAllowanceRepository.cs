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
    public class InstitutionalAllowanceRepository : BaseRepository<InstitutionalAllowance>, IInstitutionalAllowanceRepository
    {
        public InstitutionalAllowanceRepository(HrDb hrDb) : base(hrDb)
        {
        }

        public async Task ChangeToFalseforAllowance(int id)
        {
            var values = await _hrDb.InstitutionalAllowances.FindAsync(id);
            values.ApprovalStatus = false;
            _hrDb.Update(values);
            _hrDb.SaveChanges();
        }

        public async Task ChangeToTrueforAllowance(int id)
        {
            var values = await _hrDb.InstitutionalAllowances.FindAsync(id);
            values.ApprovalStatus = true;
            _hrDb.Update(values);
            _hrDb.SaveChanges();
        }

        public IEnumerable<InstitutionalAllowance> GetAllInsAllowance()
        {
            return _hrDb.InstitutionalAllowances.ToList();
        }

        public InstitutionalAllowance GetByIdInsAllowance(int id)
        {
            return _hrDb.InstitutionalAllowances.Find(id);
        }
    }
}
