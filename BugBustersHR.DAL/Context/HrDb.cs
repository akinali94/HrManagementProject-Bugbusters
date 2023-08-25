using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
//asdasd
namespace BugBustersHR.DAL.Context
{
    public class HrDb : IdentityDbContext
    {
        public HrDb(DbContextOptions<HrDb> options)
       : base(options)
        {
        }

        public DbSet<Employee> Personels { get; set; }
        public DbSet<ExpenditureType> ExpenditureTypes { get; set; }
        public DbSet<ExpenditureRequest> ExpenditureRequests { get; set; }
        public DbSet<EmployeeLeaveType> EmployeeLeaveTypes { get; set; }
        public DbSet<EmployeeLeaveRequest> EmployeeLeaveRequests { get; set; }
        public DbSet<IndividualAdvance> IndividualAdvances { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<InstitutionalAllowanceType> InstitutionalAllowanceTypes { get; set; }
        public DbSet<InstitutionalAllowance> InstitutionalAllowances { get; set; }
        public DbSet<Companies> Companies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                Name = "Ersin",
                Id = "1",
                SecondName = "",
                Surname = "Bahar",
                SecondSurname = "",
                BirthPlace = "Aydın",
                TC = "54111447858",
                BirthDate = new DateTime(1990, 1, 1),
                HiredDate = new DateTime(2020, 1, 1),
                Title = "Manager",
                Section = "DevOps",
                Salary = 2000,
                TelephoneNumber = "05354578958",
                Address = "Cicek sokak no:14",
                CompanyName = "Bilge Adam",
                Gender = GenderType.Man,
                

                ImageUrl = "https://bugbustersstorage.blob.core.windows.net/contentupload/765d1a1-7027-47f1-94f6-f293fadebf31.png",
                BackgroundImageUrl = "https://images.unsplash.com/photo-1531512073830-ba890ca4eba2?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=1920&q=80"
            });

            //BU GEREKLİ Mİ EMİN OLAMADIM AMA DENEYİP ÇIKARILABİLİR. AMACI otomatik artan (auto-increment) anahtar sütunlar veya bazı varsayılan değerler gibi senaryolarda kullanılır. 
            modelBuilder.Entity<EmployeeLeaveRequest>()
            .Property(r => r.RequestingId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<InstitutionalAllowance>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);


        }
    }
}
