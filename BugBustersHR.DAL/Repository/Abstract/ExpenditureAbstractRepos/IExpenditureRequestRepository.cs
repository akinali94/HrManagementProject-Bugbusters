using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos
{
    public interface IExpenditureRequestRepository : IBaseRepository<ExpenditureRequest>
    {
        ExpenditureRequest GetByIdExpenditureRequest(int id);

        IEnumerable<ExpenditureRequest> GetAllExReq();

        Task ChangeToTrueforExpenditure(int id);
        Task ChangeToFalseforExpenditure(int id);
        
    }
}
