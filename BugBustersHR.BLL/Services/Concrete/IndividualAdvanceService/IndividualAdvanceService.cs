using BugBustersHR.BLL.Services.Abstract.IndividualAdvanceService;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.DAL.Repository.Abstract.IndividualAdvanceseRepos;
using BugBustersHR.DAL.Repository.Concrete;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete.IndividualAdvanceService
{
    public class IndividualAdvanceService : Service<IndividualAdvance>, IIndividualAdvanceService
    {
        protected readonly IIndividualAdvancesesRepository _repository;

        protected readonly IEmployeeRepository _employeeRepository;
        public IndividualAdvanceService(IBaseRepository<IndividualAdvance> repository, IIndividualAdvancesesRepository ındividualAdvancesesRepositoryrepository, IUnitOfWork unitOfWork, HrDb db, IEmployeeRepository employeeRepository) : base(repository, unitOfWork, db)
        {
            _repository = ındividualAdvancesesRepositoryrepository;
            _employeeRepository = employeeRepository;
        }

        public async Task GetAdvanceApprovelName(IEnumerable<IndividualAdvanceRequestVM> request)
        {
            foreach (var item in request) 
            
            {
                if (item.ApprovalStatus == null)
                {
                    item.ApprovalStatusName = "Waiting for Approval";
                }
                else if (item.ApprovalStatus == true)
                {
                    item.ApprovalStatusName = "Confirmed";
                }
                else
                {
                    item.ApprovalStatusName = "Not Confirmed";
                }
            }
        }

   

        public IEnumerable<IndividualAdvance> GetAllIndividualAdvanceReq()
        {
           return _repository.GetAllIndividualAdvanceReq();
        }

        public IndividualAdvance GetByIdIndividualAdvanceRequest(int id)
        {
            return _repository.GetByIdIndividualAdvanceRequest(id);
        }


        public void RemainCalculation(IndividualAdvance ındividual)
        {
            
        }


        public async Task TChangeToFalseforAdvance(int id)
        {
            await _repository.ChangeToFalseforAdvance(id);
        }

        public async Task TChangeToTrueforAdvance(int id)
        {
            await _repository.ChangeToTrueforAdvance(id);
        }
    }
    
}
