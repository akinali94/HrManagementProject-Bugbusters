using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices
{
    public interface IExpenditureRequestService : IService<ExpenditureRequest>
    {
        ExpenditureRequest GetByIdExpenditureRequest(int id);
        IEnumerable<ExpenditureRequest> GetAllExReq();

        Task TChangeToTrueforExpenditure(int id);
        Task TChangeToFalseforExpenditure(int id);

        Task GetExpenditureApprovelName(ExpenditureRequestVM request);
    }
}
