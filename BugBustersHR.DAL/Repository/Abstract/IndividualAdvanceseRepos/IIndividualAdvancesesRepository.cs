using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract.IndividualAdvanceseRepos
{
    public interface IIndividualAdvancesesRepository:IBaseRepository<IndividualAdvance>
    {
        IndividualAdvance GetByIdIndividualAdvanceRequest(int id);

        IEnumerable<IndividualAdvance> GetAllIndividualAdvanceReq();

        void RepositoryCalculation();


        Task ChangeToTrueforAdvance(int id);
        Task ChangeToFalseforAdvance(int id);

    }
}
