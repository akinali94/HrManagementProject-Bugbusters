using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg =>
    {
        // If you run from solution root, keep this:
        cfg.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
           .AddEnvironmentVariables();

        // If you prefer running from anywhere, uncomment the two lines below and
        // add CopyToOutput for appsettings.json in the .csproj:
        // cfg.SetBasePath(AppContext.BaseDirectory)
        //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    })
    .ConfigureServices((ctx, services) =>
    {
        var cs = ctx.Configuration.GetConnectionString("DBConnection")
                 ?? throw new InvalidOperationException("Missing ConnectionStrings:DBConnection.");
        services.AddDbContext<HrDb>(opt => opt.UseSqlServer(cs));

        // Hash passwords for Identity users we seed:
        services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();
    });

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<HrDb>();
var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<IdentityUser>>();

static string N(string s) => s.ToUpperInvariant();

Console.WriteLine("Applying migrations (if any)...");
await db.Database.MigrateAsync();

// ------------------------------------------------------
// 1) Companies (sample)
// ------------------------------------------------------
if (!db.Set<Companies>().Any())
{
    Console.WriteLine("Seeding Companies...");
    await db.Set<Companies>().AddRangeAsync(
        new Companies {
            CompanyName = "BugBusters",
            CompanyTitle = CompanyTitle.Limited,
            MersisNo = "1234567890123456",
            TaxNumber = "1234567890",
            Logo = "https://picsum.photos/seed/bugbusters/120",
            TelephoneNumber = "02120000000",
            Address = "Istanbul",
            Email = "info@bugbusters.test",
            EmployeeNumber = "100",
            FoundationYear = new DateTime(2015,1,1),
            ContractStartDate = DateTime.UtcNow.AddYears(-1),
            ContractEndDate = DateTime.UtcNow.AddYears(1),
            IsActive = true
        }
    );
    await db.SaveChangesAsync();
}

// ------------------------------------------------------
// 2) ExpenditureTypes / LeaveTypes / AllowanceTypes
// ------------------------------------------------------
if (!db.Set<ExpenditureType>().Any())
{
    Console.WriteLine("Seeding ExpenditureTypes...");
    await db.Set<ExpenditureType>().AddRangeAsync(
        new ExpenditureType { ExpenditureName = "Meal",      MinPrice = 0,   MaxPrice = 250 },
        new ExpenditureType { ExpenditureName = "Transport", MinPrice = 0,   MaxPrice = 500 },
        new ExpenditureType { ExpenditureName = "Hotel",     MinPrice = 0,   MaxPrice = 5000 }
    );
    await db.SaveChangesAsync();
}

if (!db.Set<EmployeeLeaveType>().Any())
{
    Console.WriteLine("Seeding EmployeeLeaveTypes...");
    await db.Set<EmployeeLeaveType>().AddRangeAsync(
        new EmployeeLeaveType { Name = "Annual Leave", DefaultDay = 14, Gender = GenderType.Man },
        new EmployeeLeaveType { Name = "Annual Leave", DefaultDay = 14, Gender = GenderType.Woman },
        new EmployeeLeaveType { Name = "Excuse Leave", DefaultDay = 3,  Gender = GenderType.Man }
    );
    await db.SaveChangesAsync();
}

if (!db.Set<InstitutionalAllowanceType>().Any())
{
    Console.WriteLine("Seeding InstitutionalAllowanceTypes...");
    await db.Set<InstitutionalAllowanceType>().AddRangeAsync(
        new InstitutionalAllowanceType { InstitutionalAllowanceTypeName = "Equipment", MinPrice = 0, MaxPrice = 10000 },
        new InstitutionalAllowanceType { InstitutionalAllowanceTypeName = "Training",  MinPrice = 0, MaxPrice = 5000  }
    );
    await db.SaveChangesAsync();
}

// ------------------------------------------------------
// 3) ROLES
//


var adminRole = db.Roles.FirstOrDefault(r => r.Name == "Admin");
var managerRole = db.Roles.FirstOrDefault(r => r.Name == "Manager");
var employeeRole = db.Roles.FirstOrDefault(r => r.Name == "Employee");

if (adminRole == null)
{
    adminRole = new IdentityRole
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Admin",
        NormalizedName = N("Admin"),
        ConcurrencyStamp = Guid.NewGuid().ToString()
    };
    await db.Roles.AddAsync(adminRole);
}

if (managerRole == null)
{
    managerRole = new IdentityRole
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Manager",
        NormalizedName = N("Manager"),
        ConcurrencyStamp = Guid.NewGuid().ToString()
    };
    await db.Roles.AddAsync(managerRole);
}

if (employeeRole == null)
{
    employeeRole = new IdentityRole
    {
        Id = Guid.NewGuid().ToString(),
        Name = "Employee",
        NormalizedName = N("Employee"),
        ConcurrencyStamp = Guid.NewGuid().ToString()
    };
    await db.Roles.AddAsync(employeeRole);
}

await db.SaveChangesAsync();

//
// ------------------------------------------------------
// 3) Seed Employee users (admin + 2 managers + 4 employees)
// ------------------------------------------------------
// everything here is Employee (inherits IdentityUser) -> we can set domain fields:
// Name, Surname, Title, Section, TelephoneNumber, CompanyName, Salary, Gender, etc. :contentReference[oaicite:2]{index=2}
static (Employee user, string role, string password)[] GetMockEmployees()
{
    static string N(string s) => s.ToUpperInvariant();

    return new[]
    {
        // ---- ADMIN ----
        (
            new Employee
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "admin.master@bugbusters.com",
                NormalizedUserName = N("admin.master@bugbusters.com"),
                Email = "admin.master@bugbusters.com",
                NormalizedEmail = N("admin.master@bugbusters.com"),
                EmailConfirmed = true,
                PhoneNumber = "05350000001",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),

                Name = "Kemal",
                Surname = "Sancar",
                Title = "Admin",
                Section = "System",
                TelephoneNumber = "05350000001",
                Address = "Ankara Cad. No:100",
                CompanyName = "BugBusters",
                Salary = 30000,
                Gender = GenderType.Man,
                BirthPlace = "Ankara",
                TC = "12345678901",
                BirthDate = new DateTime(1980, 5, 12),
                HiredDate = new DateTime(2020, 1, 15),
                ImageUrl = "https://picsum.photos/seed/admin/200",
                BackgroundImageUrl = "https://picsum.photos/1200/300",
                Role = "Admin",
                MaxAdvanceAmount = 20000,
                AdvanceAmount = 0
            }, "Admin", "Passw0rd!"
        ),

        // ---- MANAGERS ----
        (
            new Employee
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "manager.ayse@bugbusters.com",
                NormalizedUserName = N("manager.ayse@bugbusters.com"),
                Email = "manager.ayse@bugbusters.com",
                NormalizedEmail = N("manager.ayse@bugbusters.com"),
                EmailConfirmed = true,
                PhoneNumber = "05350000002",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),

                Name = "Ayşe",
                Surname = "Yıldız",
                Title = "Manager",
                Section = "Frontend",
                TelephoneNumber = "05350000002",
                Address = "İzmir Bulvarı No:23",
                CompanyName = "BugBusters",
                Salary = 24000,
                Gender = GenderType.Woman,
                BirthPlace = "İzmir",
                TC = "23456789012",
                BirthDate = new DateTime(1985, 3, 22),
                HiredDate = new DateTime(2021, 2, 1),
                ImageUrl = "https://picsum.photos/seed/manager1/200",
                BackgroundImageUrl = "https://picsum.photos/1200/300",
                Role = "Manager",
                MaxAdvanceAmount = 15000,
                AdvanceAmount = 0
            }, "Manager", "Passw0rd!"
        ),

        (
            new Employee
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "manager.omer@bugbusters.com",
                NormalizedUserName = N("manager.omer@bugbusters.com"),
                Email = "manager.omer@bugbusters.com",
                NormalizedEmail = N("manager.omer@bugbusters.com"),
                EmailConfirmed = true,
                PhoneNumber = "05350000003",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),

                Name = "Ömer",
                Surname = "Turan",
                Title = "Manager",
                Section = "Backend",
                TelephoneNumber = "05350000003",
                Address = "Kadıköy Mah. No:50",
                CompanyName = "BugBusters",
                Salary = 25000,
                Gender = GenderType.Man,
                BirthPlace = "Istanbul",
                TC = "34567890123",
                BirthDate = new DateTime(1983, 7, 8),
                HiredDate = new DateTime(2022, 3, 5),
                ImageUrl = "https://picsum.photos/seed/manager2/200",
                BackgroundImageUrl = "https://picsum.photos/1200/300",
                Role = "Manager",
                MaxAdvanceAmount = 18000,
                AdvanceAmount = 0
            }, "Manager", "Passw0rd!"
        ),

        // ---- EMPLOYEES ----
        (
            new Employee
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "employee.ali@bugbusters.com",
                NormalizedUserName = N("employee.ali@bugbusters.com"),
                Email = "employee.ali@bugbusters.com",
                NormalizedEmail = N("employee.ali@bugbusters.com"),
                EmailConfirmed = true,
                PhoneNumber = "05350000004",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),

                Name = "Ali",
                Surname = "Demir",
                Title = "Employee",
                Section = "Mobile",
                TelephoneNumber = "05350000004",
                Address = "Bahçelievler Sk. No:17",
                CompanyName = "BugBusters",
                Salary = 18000,
                Gender = GenderType.Man,
                BirthPlace = "Eskişehir",
                TC = "45678901234",
                BirthDate = new DateTime(1992, 11, 19),
                HiredDate = new DateTime(2023, 1, 10),
                ImageUrl = "https://picsum.photos/seed/employee1/200",
                BackgroundImageUrl = "https://picsum.photos/1200/300",
                Role = "Employee",
                MaxAdvanceAmount = 12000,
                AdvanceAmount = 0
            }, "Employee", "Passw0rd!"
        ),

        (
            new Employee
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "employee.elif@bugbusters.com",
                NormalizedUserName = N("employee.elif@bugbusters.com"),
                Email = "employee.elif@bugbusters.com",
                NormalizedEmail = N("employee.elif@bugbusters.com"),
                EmailConfirmed = true,
                PhoneNumber = "05350000005",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),

                Name = "Elif",
                Surname = "Kara",
                Title = "Employee",
                Section = "QA",
                TelephoneNumber = "05350000005",
                Address = "Ataşehir No:11",
                CompanyName = "BugBusters",
                Salary = 17500,
                Gender = GenderType.Woman,
                BirthPlace = "Adana",
                TC = "56789012345",
                BirthDate = new DateTime(1994, 9, 5),
                HiredDate = new DateTime(2023, 5, 20),
                ImageUrl = "https://picsum.photos/seed/employee2/200",
                BackgroundImageUrl = "https://picsum.photos/1200/300",
                Role = "Employee",
                MaxAdvanceAmount = 10000,
                AdvanceAmount = 0
            }, "Employee", "Passw0rd!"
        ),

        (
            new Employee
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "employee.ahmet@bugbusters.com",
                NormalizedUserName = N("employee.ahmet@bugbusters.com"),
                Email = "employee.ahmet@bugbusters.com",
                NormalizedEmail = N("employee.ahmet@bugbusters.com"),
                EmailConfirmed = true,
                PhoneNumber = "05350000006",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),

                Name = "Ahmet",
                Surname = "Koç",
                Title = "Employee",
                Section = "Support",
                TelephoneNumber = "05350000006",
                Address = "Beşiktaş Sk. No:30",
                CompanyName = "BugBusters",
                Salary = 16000,
                Gender = GenderType.Man,
                BirthPlace = "Bursa",
                TC = "67890123456",
                BirthDate = new DateTime(1993, 12, 15),
                HiredDate = new DateTime(2022, 6, 12),
                ImageUrl = "https://picsum.photos/seed/employee3/200",
                BackgroundImageUrl = "https://picsum.photos/1200/300",
                Role = "Employee",
                MaxAdvanceAmount = 10000,
                AdvanceAmount = 0
            }, "Employee", "Passw0rd!"
        )
    };
}

var mockData = GetMockEmployees();

foreach (var (user, role, pwd) in mockData)
{
    var existing = db.Users.FirstOrDefault(x => x.UserName == user.UserName);
    if (existing == null)
    {
        user.PasswordHash = hasher.HashPassword(user, pwd);
        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();
        existing = user;
    }

    // attach to correct role
    var roleId = role switch
    {
        "Admin" => adminRole!.Id,
        "Manager" => managerRole!.Id,
        _ => employeeRole!.Id
    };
    bool linked = db.UserRoles.Any(ur => ur.UserId == existing.Id && ur.RoleId == roleId);
    if (!linked)
    {
        await db.UserRoles.AddAsync(new IdentityUserRole<string>
        {
            UserId = existing.Id,
            RoleId = roleId
        });
        await db.SaveChangesAsync();
    }
}

// Handy references (by username)
var manager1 = db.Users.First(x => x.UserName == "manager1@example.com");
var employee1 = db.Users.First(x => x.UserName == "employee1@example.com");

// ------------------------------------------------------
// 6) Sample business data tied to the new users
// ------------------------------------------------------
var meal   = db.Set<ExpenditureType>().FirstOrDefault(t => t.ExpenditureName == "Meal");
var transp = db.Set<ExpenditureType>().FirstOrDefault(t => t.ExpenditureName == "Transport");
var annWom = db.Set<EmployeeLeaveType>().FirstOrDefault(t => t.Name == "Annual Leave" && t.Gender == GenderType.Woman);

if (meal != null && transp != null && !db.Set<ExpenditureRequest>().Any())
{
    Console.WriteLine("Seeding ExpenditureRequests...");
    await db.Set<ExpenditureRequest>().AddRangeAsync(
        new ExpenditureRequest {
            Title = "Team lunch",
            Currency = Currency.TL,
            AmountOfExpenditure = 180,
            ExpenditureTypeId = meal.Id,
            EmployeeId = manager1.Id,
            RequestDate = DateTime.UtcNow.AddDays(-7),
            ApprovalStatus = true,
            ApprovalDate = DateTime.UtcNow.AddDays(-6),
            ImageUrl = "https://picsum.photos/seed/receipt1/400"
        },
        new ExpenditureRequest {
            Title = "Client visit transport",
            Currency = Currency.TL,
            AmountOfExpenditure = 90,
            ExpenditureTypeId = transp.Id,
            EmployeeId = manager1.Id,
            RequestDate = DateTime.UtcNow.AddDays(-3),
            ApprovalStatus = null,
            ImageUrl = "https://picsum.photos/seed/receipt2/400"
        }
    );
    await db.SaveChangesAsync();
}

if (annWom != null && !db.Set<EmployeeLeaveRequest>().Any())
{
    Console.WriteLine("Seeding EmployeeLeaveRequests...");
    await db.Set<EmployeeLeaveRequest>().AddAsync(
        new EmployeeLeaveRequest {
            StartDate = DateTime.UtcNow.Date.AddDays(5),
            EndDate   = DateTime.UtcNow.Date.AddDays(9),
            DateRequest = DateTime.UtcNow.Date,
            Approved = null,
            LeaveTypeName = "Annual Leave",
            Gender = GenderType.Woman,
            RequestingId = employee1.Id,
            SelectedLeaveTypeId = annWom.Id
        }
    );
    await db.SaveChangesAsync();
}

if (!db.Set<IndividualAdvance>().Any())
{
    Console.WriteLine("Seeding IndividualAdvances...");
    await db.Set<IndividualAdvance>().AddAsync(
        new IndividualAdvance {
            Currency = Currency.TL,
            Explanation = "Laptop advance",
            RequestDate = DateTime.UtcNow.AddDays(-10),
            ApprovalDate = DateTime.UtcNow.AddDays(-9),
            ApprovalStatus = true,
            ApprovalStatusName = "Approved",
            Amount = 5000,
            Remain = 2000,
            EmployeeRequestingId = manager1.Id
        }
    );
    await db.SaveChangesAsync();
}

var equip = db.Set<InstitutionalAllowanceType>().FirstOrDefault(t => t.InstitutionalAllowanceTypeName == "Equipment");
if (equip != null && !db.Set<InstitutionalAllowance>().Any())
{
    Console.WriteLine("Seeding InstitutionalAllowances...");
    await db.Set<InstitutionalAllowance>().AddAsync(
        new InstitutionalAllowance {
            Title = "New monitor",
            Currency = Currency.TL,
            AmountOfAllowance = 2500,
            RequestDate = DateTime.UtcNow.AddDays(-12),
            ApprovalStatus = null,
            InstitutionalAllowanceTypeId = equip.Id,
            EmployeeId = manager1.Id,
            ImageUrl = "https://picsum.photos/seed/allowance1/400"
        }
    );
    await db.SaveChangesAsync();
}

Console.WriteLine("Seeding completed.");
