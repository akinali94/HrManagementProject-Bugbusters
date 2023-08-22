// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using BugBustersHR.DAL.Context;
using BugBustersHR.ENTITY.Concrete;
using BugBustersHR.ENTITY.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace BugBustersHR.UI.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HrDb _db;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            HrDb db)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _db = db;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 
        
            [Required]
            [RegularExpression("^[a-zA-ZğüşıöçĞÜŞİÖÇ]+$", ErrorMessage = "Name can only contain letters.")]
            public string Name { get; set; }
            public string? SecondName { get; set; }
            [Required]
            [RegularExpression("^[a-zA-ZğüşıöçĞÜŞİÖÇ]+$", ErrorMessage = "Surname can only contain letters.")]
            public string Surname { get; set; }
            public string? SecondSurname { get; set; }
            public string? BirthPlace { get; set; }
            [Required]
            [RegularExpression(@"^\d{11}$", ErrorMessage = "Please enter a valid TC identification number.")]
            public string TC { get; set; }
            [Required]

            private DateTime _birthDate;

            public DateTime BirthDate
            {
                get { return _birthDate.Date; }
                set { _birthDate = value.Date; }
            }

            public DateTime? HiredDate { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public string Section { get; set; }
            [Required]
            [RegularExpression(@"^05\d{9}$", ErrorMessage = "Please enter a valid phone number in the format 05XXXXXXXXX.")]
            public string TelephoneNumber { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string CompanyName { get; set; }
            [Required]
            public decimal Salary { get; set; }
            public GenderType Gender { get; set; }
            public decimal MaxAdvanceAmount
            {
                get
                {
                    return Salary * 3;
                }
            }


            public string Role { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email
            {
                get
                {
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

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public static class GenderTypeHelper
        {
            public static SelectList GetGenderSelectList()
            {
                var genderTypes = Enum.GetValues(typeof(GenderType))
                                      .Cast<GenderType>()
                                      .Select(gender => new SelectListItem
                                      {
                                          Value = gender.ToString(),
                                          Text = gender.ToString()
                                      });

                return new SelectList(genderTypes, "Value", "Text");
            }
        }

        public static ValidationResult ValidateTC(string tc)
        {
            if (tc.Length != 11 || !IsAllDigits(tc))
            {
                return new ValidationResult("Invalid TC identification number.");
            }

            int[] digits = tc.Select(c => int.Parse(c.ToString())).ToArray();

            if (digits[0] == 0 || digits.Take(10).All(d => d == digits[0]))
            {
                return new ValidationResult("Invalid TC identification number.");
            }

            int oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int evenSum = digits[1] + digits[3] + digits[5] + digits[7];
            int tenthDigit = (oddSum * 7 - evenSum) % 10;
            int eleventhDigit = digits.Take(10).Sum() % 10;

            if (digits[9] != tenthDigit || digits[10] != eleventhDigit)
            {
                return new ValidationResult("Invalid TC identification number.");
            }

            return ValidationResult.Success;
        }

        private static bool IsAllDigits(string str)
        {
            return str.All(char.IsDigit);
        }

        // GetGenderSelectList metodu burada
        public SelectList GetGenderSelectList()
        {
            return GenderTypeHelper.GetGenderSelectList();
        }

        public static SelectList GetRoles()
        {
            var roles = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = "Manager", Value = AppRoles.Role_Manager},
                new SelectListItem{Text = "Employee", Value = AppRoles.Role_Employee}
            }, "Value", "Text");

            return roles;
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // TC kimlik numarasını doğrula
                var tcValidationResult = ValidateTC(Input.TC);
                if (tcValidationResult != ValidationResult.Success)
                {
                    ModelState.AddModelError(string.Empty, "Please check your TC number.");
                    return Page();
                }

                if (Input.Salary < 11402) // Örneğin, asgari ücret 3000 olarak kabul edilirse
                {
                    ModelState.AddModelError(string.Empty, "Salary must be greater than or equal to the minimum salary.");
                    return Page();
                }

                var user = new Employee
                { 
                    UserName=Input.Email,
                    Email=Input.Email,
                    Name=Input.Name,
                    Surname=Input.Surname,
                    SecondName=Input.SecondName,
                    SecondSurname=Input.SecondSurname,
                    BirthDate = Input.BirthDate.Date, // Sadece tarihi alıyoruz
                    BirthPlace = Input.BirthPlace,
                    TC=Input.TC,
                    HiredDate = Input.HiredDate?.Date, // Sadece tarihi alıyoruz
                    Title = Input.Title,
                    Section=Input.Section,
                    TelephoneNumber=Input.TelephoneNumber,
                    Address=Input.Address,
                    CompanyName=Input.CompanyName,
                    Salary=Input.Salary,
                    Gender = Input.Gender,
                    Role = Input.Role,
                    MaxAdvanceAmount=Input.MaxAdvanceAmount
                  


                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if(!await _roleManager.RoleExistsAsync(AppRoles.Role_Employee) && Input.Role == AppRoles.Role_Employee)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Employee));
                    }

                    else if(!await _roleManager.RoleExistsAsync(AppRoles.Role_Manager) && Input.Role == AppRoles.Role_Manager)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Manager));
                    }

                    await _userManager.AddToRoleAsync(user, Input.Role);

                    //if (!await _roleManager.RoleExistsAsync(AppRoles.Role_Employee))
                    //{
                    //    await _roleManager.CreateAsync(new IdentityRole(AppRoles.Role_Employee));
                    //}

                    //await _userManager.AddToRoleAsync(user, AppRoles.Role_Employee);

                    var userId = await _userManager.GetUserIdAsync(user);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
