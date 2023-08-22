using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.DAL.Repository.Concrete.LeaveConcreteRepos;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete.ExpenditureConcreteServices
{
    public class ExpenditureRequestService : Service<ExpenditureRequest>, IExpenditureRequestService
    {
        protected readonly IExpenditureRequestRepository _requestrepository;
        public ExpenditureRequestService(IBaseRepository<ExpenditureRequest> repository, IUnitOfWork unitOfWork, HrDb db, IExpenditureRequestRepository requestrepository) : base(repository, unitOfWork, db)
        {
            _requestrepository = requestrepository;
        }

        public IEnumerable<ExpenditureRequest> GetAllExReq()
        {
            return _requestrepository.GetAllExReq();
        }

        public ExpenditureRequest GetByIdExpenditureRequest(int id)
        {
            return _requestrepository.GetByIdExpenditureRequest(id);
        }

        public async Task TChangeToFalseforExpenditure(int id)
        {
            await _requestrepository.ChangeToFalseforExpenditure(id);
        }

        public async Task TChangeToTrueforExpenditure(int id)
        {
            await _requestrepository.ChangeToTrueforExpenditure(id);
        }
    }
}
