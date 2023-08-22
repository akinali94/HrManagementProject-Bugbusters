using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.InstitutionalAllowanceTypeViewModel
{
    public class InstitutionalAllowanceTypeVM
    {
        public int Id { get; set; }
        public string InstitutionalAllowanceTypeName { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; } = 0;
    }
}
