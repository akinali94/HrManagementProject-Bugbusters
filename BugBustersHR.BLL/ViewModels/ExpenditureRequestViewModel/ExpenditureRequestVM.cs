using BugBustersHR.BLL.ViewModels.BaseViewModel;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel
{
    public class ExpenditureRequestVM:BaseVM
    {
        public ExpenditureRequestVM()
        {
            ExpenditureTypeCreateVM = new ExpenditureTypeCreateVM();
            ImageModel = new ImageModel();
        }
        public string ImageUrl { get; set; } // Eklediğimiz özellik
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ImageModel ImageModel { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public Currency? Currency { get; set; }
        public bool? ApprovalStatus { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now.Date;
        public DateTime? ApprovalDate { get; set; }
        public string? TypeName { get; set; }
        public decimal AmountOfExpenditure { get; set; }
        public string? ApprovalStatusName { get; set; }



        //One to One Relationship
        //[Required(ErrorMessage = "Please select an Expenditure Type.")]
        public int ExpenditureTypeId { get; set; }
        public ExpenditureTypeCreateVM ExpenditureTypeCreateVM { get; set; }
        //Talepte bulunan kullanıcı
        public string EmployeeId { get; set; }
        public EmployeeVM Employee { get; set; }
        //Onaylayan kullanıcı
        public string? CompanyName { get; set; }
    }
}
