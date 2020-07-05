using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using SensorManagementSystem.Models.Entities;
using SensorManagementSystem.Services.Contract;

namespace SensorManagementSystem.App.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<UserEntity> _signInManager;
		private readonly UserManager<UserEntity> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailService _emailService;
		private const int EmailActivationExpireTime = 2;

		public RegisterModel(
			UserManager<UserEntity> userManager,
			SignInManager<UserEntity> signInManager,
			ILogger<RegisterModel> logger,
			IEmailService emailService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_emailService = emailService;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }
		}

		public void OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Content("~/");

			if (ModelState.IsValid)
			{
				UserEntity existingUser = await _userManager.FindByEmailAsync(Input.Email);
				if (existingUser != null && !existingUser.EmailConfirmed)
				{
					if (existingUser.CreatedOn.Value.AddMinutes(EmailActivationExpireTime) < DateTime.UtcNow)
					{
						// Continue registration
						await _userManager.DeleteAsync(existingUser);
					}
					else
					{
						ModelState.AddModelError("EmailToBeConfirmed", "This email is alredy taken. If it is yours, please go and activate your account from there!");

						return Page();
					}
				}

				var user = new UserEntity { UserName = Input.Email, Email = Input.Email };

				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					_logger.LogInformation("User created a new account with password.");

					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { userId = user.Id, code },
						protocol: Request.Scheme);

					await _emailService.SendAsync(user.Email, "Successfull registration", $"Confirm you account by clicking on this link:\n\n{HtmlEncoder.Default.Encode(callbackUrl)}\n\nLink will be active only for {EmailActivationExpireTime} minutes.");

					return LocalRedirect(returnUrl);
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
	}
}
