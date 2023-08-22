﻿using BugBustersHR.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class InstitutionalAllowance
    {
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
        public InstitutionalAllowanceType InstitutionalAllowanceType { get; set; }
        //Talepte bulunan kullanıcı
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        
        
        //Onaylayan kullanıcı

        public Admin? Admin { get; set; }
        public string? AdminId { get; set; }


    }
}
