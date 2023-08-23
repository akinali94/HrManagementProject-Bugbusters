using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices
{
    public interface IInstitutionalAllowanceService : IService<InstitutionalAllowance>
    {
        InstitutionalAllowance GetByIdInstitutionalAllowance(int id);

        IEnumerable<InstitutionalAllowance> GetAllInstitutionalAllowances();

        Task TChangeToTrueforAllowance(int id);
        Task TChangeToFalseforAllowance(int id);

        Task GetInstAllApprovelName(InstitutionalAllowanceVM request);
    }

}
