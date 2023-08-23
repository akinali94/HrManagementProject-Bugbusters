using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceTypeViewModel;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel
{
    public class InstitutionalAllowanceVM
    {
        public InstitutionalAllowanceVM()
        {
            InstitutionalAllowanceTypeVM = new InstitutionalAllowanceTypeVM();
        }
        public ImageModel ImageModel { get; set; }
        public string? FullName { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public Currency? Currency { get; set; }
        public bool? ApprovalStatus { get; set; } = null;
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? ApprovalDate { get; set; }
        [NotMapped]
        public string? TypeName { get; set; }
        [NotMapped]
        public string? ApprovalStatusName { get; set; }
        public decimal AmountOfAllowance { get; set; }

        public string? ImageUrl { get; set; }

        //One to One Relationship
        public int InstitutionalAllowanceTypeId { get; set; }
        public InstitutionalAllowanceTypeVM InstitutionalAllowanceTypeVM { get; set; }
        //Talepte bulunan kullanıcı
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        //Onaylayan kullanıcı
        public string? CompanyName { get; set; }
        public Admin? Admin { get; set; }
        public string? AdminId { get; set; }
    }
}
