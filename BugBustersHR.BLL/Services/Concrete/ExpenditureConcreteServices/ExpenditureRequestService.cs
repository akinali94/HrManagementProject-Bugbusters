using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
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
        protected readonly IExpenditureTypeService _typeservice;
        public ExpenditureRequestService(IBaseRepository<ExpenditureRequest> repository, IUnitOfWork unitOfWork, HrDb db, IExpenditureRequestRepository requestrepository, IExpenditureTypeService typeservice) : base(repository, unitOfWork, db)
        {
            _requestrepository = requestrepository;
            _typeservice = typeservice;
        }

        public IEnumerable<ExpenditureRequest> GetAllExReq()
        {
            return _requestrepository.GetAllExReq();
        }

        public ExpenditureRequest GetByIdExpenditureRequest(int id)
        {
            return _requestrepository.GetByIdExpenditureRequest(id);
        }

        public async Task GetExpenditureApprovelName(IEnumerable<ExpenditureRequestVM> request)
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

        public async Task GetExpenditureTypeName(IEnumerable<ExpenditureRequestVM> request)
        {
            foreach (var item in request)
            {
                item.TypeName = (_typeservice.GetByIdExpenditureType(item.ExpenditureTypeId)).ExpenditureName;
            }
           
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
