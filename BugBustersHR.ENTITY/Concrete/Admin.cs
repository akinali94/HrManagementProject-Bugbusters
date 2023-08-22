using BugBustersHR.ENTITY.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.ENTITY.Concrete
{
    public class Admin : BaseEntity
    {
        public string? ImageUrl { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string Surname { get; set; }
        public string? SecondSurname { get; set; }

        [NotMapped]
        public string Role { get; set; }

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
    }
}
