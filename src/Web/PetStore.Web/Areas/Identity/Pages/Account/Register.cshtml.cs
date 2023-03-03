// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
namespace PetStore.Web.Areas.Identity.Pages.Account
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Configuration;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.UI.Services;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.RazorPages;
	using Microsoft.AspNetCore.WebUtilities;
	using Microsoft.CodeAnalysis.VisualBasic.Syntax;
	using Microsoft.Extensions.Logging;

	using PetStore.Data.Models;
	using PetStore.Services.Data;
	using PetStore.Services.Data.Contracts;
	using PetStore.Web.Infrastructures;

	public class RegisterModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserStore<ApplicationUser> _userStore;
		private readonly IUserEmailStore<ApplicationUser> _emailStore;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;
		private readonly IClientCardService clientCardService;
        private readonly IAddressService addressService;
		private ApplicationUser user;

        public RegisterModel(
			UserManager<ApplicationUser> userManager,
			IUserStore<ApplicationUser> userStore,
			SignInManager<ApplicationUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender,
			IClientCardService clientCardService,
            IAddressService addressService)
		{
            this._userManager = userManager;
            this._userStore = userStore;
            this._emailStore = this.GetEmailStore();
            this._signInManager = signInManager;
            this._logger = logger;
            this._emailSender = emailSender;
            this.clientCardService = clientCardService;
            this.addressService = addressService;
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
			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long!", MinimumLength = 3)]
			[Display(Name = "First Name")]
			public string FirstName { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long!", MinimumLength = 3)]
			[Display(Name = "Last Name")]
			public string LastName { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

            [Required]
            [RegularExpression(@"^(\+)?(\d){7,12}$", ErrorMessage = "The phone number must contains only digits and must be min 7 and max 12!")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
			
            [Required]
            [RegularExpression(@"^(.){3,},(.){5,}$", ErrorMessage = "The address must in format City, Street!")]
            [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long!", MinimumLength = 3)]
            [Display(Name = "Delivery address")]
            public string DeliveryAddress { get; set; }

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

		public async Task OnGetAsync(string returnUrl = null)
		{
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= this.Url.Content("~/");
            this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (this.ModelState.IsValid)
			{
				this.user = this.CreateUser();
                await this.PopulateUser();

                await this._userStore.SetUserNameAsync(this.user, this.Input.Email, CancellationToken.None);
				await this._emailStore.SetEmailAsync(this.user, this.Input.Email, CancellationToken.None);
				var result = await this._userManager.CreateAsync(this.user, this.Input.Password);

				if (result.Succeeded)
				{
                    this._logger.LogInformation("User created a new account with password.");

					var userId = await this._userManager.GetUserIdAsync(this.user);
					var code = await this._userManager.GenerateEmailConfirmationTokenAsync(this.user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					var callbackUrl = this.Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
						protocol: this.Request.Scheme);

					await this._emailSender.SendEmailAsync(Input.Email, "Confirm your email", "Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					if (this._userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return this.RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
					}
					else
					{
						await this._signInManager.SignInAsync(user, isPersistent: false);
						return this.LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors)
				{
                    this.ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return this.Page();
		}

        private async Task PopulateUser()
        {
            this.user.FirstName = this.Input.FirstName;
            this.user.LastName = this.Input.LastName;
            this.user.UserName = this.Input.Email;
            this.user.PhoneNumber = this.Input.PhoneNumber;
            string clientCardId = Guid.NewGuid().ToString();
            string addressId = Guid.NewGuid().ToString();
            await this.addressService.CreateNewAddress(addressId, this.user.Id, this.Input.DeliveryAddress);
            await this.clientCardService.CreateNewCard(clientCardId, this.user.Id);
            this.user.ClientCardId = clientCardId;
            this.user.AddressId = addressId;
        }

        private ApplicationUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<ApplicationUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
					$"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<ApplicationUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<ApplicationUser>)_userStore;
		}
	}
}
