using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract.IndividualAdvanceseRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete.IndividualAdvanceRepos
{
    public class IndividualAdvanceRepository : BaseRepository<IndividualAdvance>, IIndividualAdvancesesRepository
    {
        public IndividualAdvanceRepository(HrDb hrDb) : base(hrDb)
        {
        }

        public async Task ChangeToFalseforAdvance(int id)
        {
            var values = await _hrDb.IndividualAdvances.FindAsync(id);
            values.ApprovalStatus = false;
            _hrDb.Update(values);
            _hrDb.SaveChanges();
        }

        public async Task ChangeToTrueforAdvance(int id)
        {
            var values = await _hrDb.IndividualAdvances.FindAsync(id);
            values.ApprovalStatus = true;
            _hrDb.Update(values);
            _hrDb.SaveChanges();
        }

        public IEnumerable<IndividualAdvance> GetAllIndividualAdvanceReq()
        {
            return _hrDb.IndividualAdvances.ToList();
        }

        public IndividualAdvance GetByIdIndividualAdvanceRequest(int id)
        {
            return _hrDb.IndividualAdvances.Find(id);
        }

        public void RepositoryCalculation()
        {
           
        }
    }
}
