using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices
{
    public interface IInstitutionalAllowanceTypeService : IService<InstitutionalAllowanceType>
    {
        InstitutionalAllowanceType GetByIdInstitutionalAllowanceType(int id);

        IEnumerable<InstitutionalAllowanceType> GetAllInstitutionalAllowanceTypes();
    }
}
