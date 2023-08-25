using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete.ExpenditureConcreteRepos
{
    public class ExpenditureRequestRepository : BaseRepository<ExpenditureRequest>, IExpenditureRequestRepository
    {
        protected readonly IExpenditureTypeRepository _expendituretypeRepository;
        public ExpenditureRequestRepository(HrDb hrDb, IExpenditureTypeRepository expendituretypeRepository) : base(hrDb)
        {
            _expendituretypeRepository = expendituretypeRepository;
        }

        public async Task ChangeToFalseforExpenditure(int id)
        {
            var values = await _hrDb.ExpenditureRequests.FindAsync(id);
            values.ApprovalStatus = false;
            _hrDb.Update(values);
            _hrDb.SaveChanges();
        }

        public async Task ChangeToTrueforExpenditure(int id)
        {
            var values = await _hrDb.ExpenditureRequests.FindAsync(id);
            values.ApprovalStatus = true;
            _hrDb.Update(values);
            _hrDb.SaveChanges();
        }

        public IEnumerable<ExpenditureRequest> GetAllExReq()
        {
            return _hrDb.ExpenditureRequests.ToList();
        }

        public ExpenditureRequest GetByIdExpenditureRequest(int id)
        {
            return _hrDb.ExpenditureRequests.Find(id);
        }

       
    }
}
