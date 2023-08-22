using AutoMapper;
using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel
{
    public class ExpenditureRequestListVM
    {
     

        public int Id { get; set; }
        public string Title { get; set; }
        public string? Currency { get; set; }
        public bool? ApprovalStatus { get; set; }
        public string? ApprovalStatusName { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? ApprovalDate { get; set; }

        public string? TypeName { get; set; }
        public decimal AmountOfExpenditure { get; set; }


        //One to One Relationship
        public int ExpenditureTypeId { get; set; }
        public ExpenditureTypeVM ExpenditureType { get; set; }

        //public string? TypeName { get; set; }

        //Talepte bulunan kullanıcı
        public string EmployeeId { get; set; }
        public EmployeeVM Employee { get; set; }

        public string? CompanyName { get; set; }


    }
}
