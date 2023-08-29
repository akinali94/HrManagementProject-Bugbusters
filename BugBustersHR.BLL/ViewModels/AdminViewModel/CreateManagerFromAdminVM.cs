﻿using BugBustersHR.BLL.ViewModels.CompanyViewModel;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
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
        public DateTime? HiredDate { get; set; }
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public string Section { get; set; }
        public decimal Salary { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        
        public ImageModel ImageModel { get; set; }
        public GenderType Gender { get; set; }
        public CompanyVM Company{ get; set; }

        public string CompanyName { get; set; }
        public List<string> CompanyNames { get; set; }
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
                    return $"{sanitizedFirstName}.{sanitizedLastName}@bilgeadamboost.com";
                }
                else if (SecondSurname == null && SecondName != null)
                {
                    string? sanitizedSecondName = SecondName.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();


                    return $"{sanitizedFirstName}{sanitizedSecondName}.{sanitizedLastName}@bilgeadamboost.com";
                }
                else if (SecondSurname != null && SecondName == null)
                {
                    string? sanitizedSecondLastName = SecondSurname.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();
                    return $"{sanitizedFirstName}{sanitizedLastName}.{sanitizedSecondLastName}@bilgeadamboost.com";
                }
                else
                {
                    string? sanitizedSecondName1 = SecondName.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();
                    string? sanitizedSecondLastName2 = SecondSurname.Replace(" ", "").Replace("ı", "i").Replace("ö", "o").Replace("ü", "u").Replace("ğ", "g").Replace("ç", "c").Replace("ş", "s").ToLower();
                    return $"{sanitizedFirstName}{sanitizedSecondName1}.{sanitizedLastName}{sanitizedSecondLastName2}@bilgeadamboost.com";
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
