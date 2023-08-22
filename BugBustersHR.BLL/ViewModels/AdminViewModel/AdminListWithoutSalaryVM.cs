using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.AdminViewModel
{
    public class AdminListWithoutSalaryVM
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string Surname { get; set; }
        public string? SecondSurname { get; set; }
        public string FullName { get; set; }
        public string? BirthPlace { get; set; }
        public string TC { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HiredDate { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public ImageModel ImageModel { get; set; }
    }
}
