using BugBustersHR.BLL.Mapping;
using BugBustersHR.BLL.Options;
using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.BLL.Services.Concrete;
using BugBustersHR.BLL.Validatons;
using BugBustersHR.BLL.ViewModels.EmployeeViewModel;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.DAL.Repository.Concrete;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Data.Common;
using Microsoft.AspNetCore.Identity.UI.Services;
using BugBustersHR.UI.Email;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.DAL.Repository.Abstract.ExpenditureAbstractRepos;
using BugBustersHR.DAL.Repository.Concrete.ExpenditureConcreteRepos;
using BugBustersHR.BLL.Services.Abstract.ExpenditureAbstractServices;
using BugBustersHR.BLL.Services.Concrete.ExpenditureConcreteServices;
using BugBustersHR.BLL.ViewModels.ExpenditureRequestViewModel;
using BugBustersHR.BLL.Validatons.ExpanditureValidations;
using BugBustersHR.BLL.Services.Abstract.LeaveAbstractService;
using BugBustersHR.BLL.Services.Concrete.LeaveConcreteService;
using BugBustersHR.DAL.Repository.Concrete.LeaveConcreteRepos;
using BugBustersHR.BLL.Validatons.LeaveValidations;
using BugBustersHR.BLL.ViewModels.LeaveRequestViewModel;
using BugBustersHR.BLL.ViewModels.LeaveTypeViewModel;
using BugBustersHR.BLL.ViewModels.ExpenditureTypeViewModel;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.BLL.Validatons.IndividualAdvanceValidator;
using BugBustersHR.DAL.Repository.Abstract.IndividualAdvanceseRepos;
using BugBustersHR.DAL.Repository.Concrete.IndividualAdvanceRepos;
using BugBustersHR.BLL.Services.Abstract.IndividualAdvanceService;
using BugBustersHR.BLL.Services.Concrete.IndividualAdvanceService;
using BugBustersHR.DAL.Repository.Abstract.InstitutionalAllowanceRepos;
using BugBustersHR.DAL.Repository.Concrete.InstitutionalAllowanceConcreteRepos;
using BugBustersHR.BLL.Services.Abstract.InstitutionalAllowanceAbstractServices;
using BugBustersHR.BLL.Services.Concrete.InstitutionalAllowanceConcreteServices;
using BugBustersHR.BLL.ViewModels.InstitutionalAllowanceViewModel;
using BugBustersHR.BLL.Validatons.InstitutionalAllowanceValidations;
using BugBustersHR.BLL.ViewModels.IndividualAdvanceViewModel;
using BugBustersHR.UI.Email.ServiceEmail;
using BugBustersHR.UI.OptionsModels;
using BugBustersHR.BLL.ViewModels.ManagerViewModel;
using BugBustersHR.BLL.Validatons.CreateEnployeeValidations;
using BugBustersHR.BLL.ViewModels.AdminViewModel;
using BugBustersHR.BLL.Validatons.CreateManagerValidation;
using BugBustersHR.DAL.Repository.Abstract.CompanyRepos;
using BugBustersHR.DAL.Repository.Concrete.Company;
using BugBustersHR.BLL.Services.Abstract.CompanyService;
using BugBustersHR.BLL.Services.Concrete.CompanyService;
using BugBustersHR.BLL.ViewModels.CompanyViewModel;
using BugBustersHR.BLL.Validatons.CompanyValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DBConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<HrDb>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<HrDb>();

builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddRazorPages();


builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

//EmailSettings

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();


//Expenditure
builder.Services.AddScoped<IExpenditureRequestRepository,ExpenditureRequestRepository>();
builder.Services.AddScoped<IExpenditureRequestService,ExpenditureRequestService>();
builder.Services.AddScoped<IExpenditureTypeRepository,ExpenditureTypeRepository>();
builder.Services.AddScoped<IExpenditureTypeService,ExpenditureTyoeService>();

//Leave
builder.Services.AddScoped<IEmployeeLeaveTypeRepository, EmployeeLeaveTypeRepository>();
builder.Services.AddScoped<IEmployeeLeaveTypeService, EmployeeLeaveTypeService>();
builder.Services.AddScoped<IEmployeeLeaveRequestRepository, EmployeeLeaveRequestRepository>();
builder.Services.AddScoped<IEmployeeLeaveRequestService, EmployeeLeaveRequestService>();


//IndividualAdvance
builder.Services.AddScoped<IIndividualAdvancesesRepository, IndividualAdvanceRepository>();
builder.Services.AddScoped<IIndividualAdvanceService, IndividualAdvanceService>();

//allowance

builder.Services.AddScoped<IInstitutionalAllowanceRepository, InstitutionalAllowanceRepository>();
builder.Services.AddScoped<IInstitutionalAllowanceTypeRepository, InstitutionalAllowanceTypeRepository>();
builder.Services.AddScoped<IInstitutionalAllowanceTypeService, InstitutionalAllowanceTypeService>();
builder.Services.AddScoped<IInstitutionalAllowanceService, InstitutionalAllowanceService>();
// Company
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();






builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped<AzureOptions>();  evet onu denicektim
builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection("Azure"));
builder.Services.AddDbContext<HrDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddScoped<EmployeeLeaveRequestVM>();
builder.Services.AddScoped<EmployeeLeaveTypeCreateVM>();
builder.Services.AddScoped<EmployeeLeaveRequestCreateVM>();
builder.Services.AddScoped<EmployeeLeaveRequestListVM>();
builder.Services.AddScoped<EmployeeLeaveTypeVM>();
builder.Services.AddScoped<IndividualAdvanceRequestVM>();
builder.Services.AddScoped<InstitutionalAllowanceVM>();
builder.Services.AddScoped<CompanyVM>();

builder.Services.AddScoped<CreateEmployeeFromManagerVM>();
builder.Services.AddScoped<CreateManagerFromAdminVM>();
builder.Services.AddScoped<GetManagerListVM>();
builder.Services.AddScoped<AdminUpdateVM>();
builder.Services.AddScoped<AdminSummaryListVM>();
builder.Services.AddScoped<AdminListWithoutSalaryVM>();
builder.Services.AddScoped<GetManagerListVM>();

builder.Services.AddScoped<EmployeeVM>();



builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>()); // Main sınıfı (Program) kullan�l�yor

builder.Services.AddScoped<IValidator<EmployeeUpdateVM>, EmployeeValidator>();
builder.Services.AddScoped<IValidator<ExpenditureRequestVM>, ExpenditureRequestValidator>();
builder.Services.AddScoped<IValidator<EmployeeLeaveTypeVM>, EmployeeLeaveTypeValidator>();
builder.Services.AddScoped<IValidator<EmployeeLeaveRequestVM>, EmployeeLeaveRequestValidator>();
builder.Services.AddScoped<IValidator<IndividualAdvanceRequestVM>, IndividualAdvanceValidator>();
builder.Services.AddScoped<IValidator<InstitutionalAllowanceVM>, InstitutionalAllowanceValidator>();
builder.Services.AddScoped<IValidator<CreateEmployeeFromManagerVM>, CreateEmployeeValidator>();
builder.Services.AddScoped<IValidator<CreateManagerFromAdminVM>, CreateManagerValidator>();
builder.Services.AddScoped<IValidator<AdminUpdateVM>, AdminValidator>();
builder.Services.AddScoped<IValidator<CompanyVM>, CompanyValidator>();



builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
 pattern: "{area=Bos}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
