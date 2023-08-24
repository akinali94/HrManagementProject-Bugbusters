using BugBustersHR.ENTITY.Abstract;
using BugBustersHR.ENTITY.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class Employee : IdentityUser
    {
        public string? ImageUrl { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string Surname { get; set; }
        public string? SecondSurname { get; set; }
        
      
        public string? Role { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                if (SecondName == null && SecondSurname == null)
                {
                    return $"{Name} {Surname}";
                }
                else if (SecondName == null && SecondSurname != null)
                {
                    return $"{Name} {Surname} {SecondSurname}";
                }
                else if (SecondName != null && SecondSurname == null)
                {
                    return $"{Name} {SecondName} {Surname}";
                }
                else
                {
                    return $"{Name} {SecondName} {Surname} {SecondSurname}";
                }


            }
            set
            {

            }
        }
        public decimal AdvanceAmount { get; set; }


        public string? BirthPlace { get; set; }

        public string TC { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime? HiredDate { get; set; }
        public DateTime? ResignationDate { get; set; }

        public bool IsActive
        {
            get
            {
                return HiredDate != default(DateTime) && ResignationDate == null;
            }
            set { }
        }
        public string Title { get; set; }
        public string Section { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }

        public decimal Salary { get; set; }
        public GenderType Gender { get; set; }
        public decimal MaxAdvanceAmount { get; set; }


    }
}
