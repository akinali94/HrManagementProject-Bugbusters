﻿using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.ViewModels.AdminViewModel
{
    public class CreateManagerFromAdminVM
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
        public DateTime StartedDate { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public decimal Salary { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public ImageModel ImageModel { get; set; }

        public Companies Company { get; set; }    
        public string Email
        {
            get
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Surname))
                {
                    return string.Empty;
                }
                string sanitizedFirstName = Name.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();
                string sanitizedLastName = Surname.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();




                if (SecondSurname == null && SecondName == null)
                {
                    return $"{sanitizedFirstName}.{sanitizedLastName}@bilgeadam.com";
                }
                else if (SecondSurname == null && SecondName != null)
                {
                    string? sanitizedSecondName = SecondName.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();


                    return $"{sanitizedFirstName}{sanitizedSecondName}.{sanitizedLastName}@bilgeadam.com";
                }
                else if (SecondSurname != null && SecondName == null)
                {
                    string? sanitizedSecondLastName = SecondSurname.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();
                    return $"{sanitizedFirstName}{sanitizedLastName}.{sanitizedSecondLastName}@bilgeadam.com";
                }
                else
                {
                    string? sanitizedSecondName1 = SecondName.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();
                    string? sanitizedSecondLastName2 = SecondSurname.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();
                    return $"{sanitizedFirstName}{sanitizedSecondName1}.{sanitizedLastName}{sanitizedSecondLastName2}@bilgeadam.com";
                }
            }
            set
            {

            }
        }

        public string Password { get; set; }
        public decimal MaxAdvanceAmount
        {
            get
            { return Salary * 3; }

            set { }

        }




    }
}
