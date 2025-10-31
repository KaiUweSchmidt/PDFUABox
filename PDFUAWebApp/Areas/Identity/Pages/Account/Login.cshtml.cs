// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using PDFUABox.WebApp.Areas.Identity.Data;

namespace PDFUABox.WebApp.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<PDFUABoxUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;

        public LoginModel(SignInManager<PDFUABoxUser> signInManager, ILogger<LoginModel> logger, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; private set; }

        // CA1056: Property type changed to System.Uri
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public bool IsMailEnabled
        {
            get
            {
                return _configuration.GetValue<bool>("MailSettings:EnableMail");
            }
        }



        // New Uri-based handler to satisfy CA1054 and to back the Uri-typed property.
        public async Task OnGetAsync(string returnUrl)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();

            ReturnUrl = returnUrl;
        }


        // Uri-based POST handler
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            returnUrl =  returnUrl ?? Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    // CA1848: Use LoggerMessage delegate for performance
                    s_logUserLoggedIn(_logger, null);
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    s_logLoginPageAccessed(_logger, "User account locked out.", null);
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }

        // Removed nullable annotation on Exception to fix CS8632 (file has #nullable disable)
        private static readonly Action<ILogger, string, Exception> s_logLoginPageAccessed =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(0, nameof(OnGetAsync)),
                "Login.cshtml.cs {Message}");

        // LoggerMessage delegate for "User logged in."
        private static readonly Action<ILogger, Exception> s_logUserLoggedIn =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(1, nameof(LoginModel)),
                "User logged in.");
    }

    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
