using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PDFUABox.WebApp.Areas.Identity.Data;
using PDFUABox.WebApp.Pages;

namespace PDFUABox.WebApp.Areas.Identity.Pages.Account
{
    // Top-level input model to avoid nesting CA1034
    public class RegisterInputModel
    {
        [Display(Name = "First Name")]
        public string Firstname { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Create a certificate?")]
        public bool CreateCertificate { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm certificate password")]
        [RequiredIf("CreateCertificate", true, ErrorMessage = "This field is required when create a certificate is checked")]
        public string CertificatePassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("CertificatePassword", ErrorMessage = "The certificate password and confirmation password do not match.")]
        [Display(Name = "Confirm certificate password")]
        [RequiredIf("CreateCertificate", true, ErrorMessage = "This field is required when create a certificate is checked")]
        public string ConfirmCertificatePassword { get; set; } = string.Empty;
    }

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<PDFUABoxUser> _signInManager;
        private readonly UserManager<PDFUABoxUser> _userManager;
        private readonly IUserStore<PDFUABoxUser> _user_store;
        private readonly IUserEmailStore<PDFUABoxUser> _email_store;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        // LoggerMessage delegate to address CA1848
        private static readonly Action<ILogger, Exception?> _userCreated =
            Microsoft.Extensions.Logging.LoggerMessage.Define(LogLevel.Information, new EventId(1, nameof(RegisterModel)), "User created a new account with password.");

        public RegisterModel(
            UserManager<PDFUABoxUser> userManager,
            IUserStore<PDFUABoxUser> userStore,
            SignInManager<PDFUABoxUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _user_store = userStore;
            _email_store = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new RegisterInputModel();

        // CA1056: Use System.Uri for URL properties
        public Uri ReturnUrl { get; set; } = new Uri("about:blank");

        // CA2227: make read-only; initialize and modify the collection instead of replacing it
        public IList<AuthenticationScheme> ExternalLogins { get; } = new List<AuthenticationScheme>();

        public async Task OnGetAsync(Uri? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"));
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false);
            ExternalLogins.Clear();
            foreach (var s in schemes) ExternalLogins.Add(s);
        }

        public async Task<IActionResult> OnPostAsync(Uri? returnUrl = null)
        {
            // Convert Uri to string for Url helpers / redirects
            var returnUrlString = returnUrl?.ToString() ?? Url.Content("~/");
            ReturnUrl = returnUrl ?? new Uri(Url.Content("~/"));

            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false);
            ExternalLogins.Clear();
            foreach (var s in schemes) ExternalLogins.Add(s);

            if (ModelState.IsValid)
            {
                var user = new PDFUABoxUser()
                {
                    FirstName = Input.Firstname,
                    LastName = Input.Lastname,
                    CertificatePassword = Input.CreateCertificate ? Input.CertificatePassword : "<no certificate>",
                    Certificate = Input.CreateCertificate ? SignHelpers.CreateCertificate(Input.Email, Input.CertificatePassword) : "<no certificate>"
                };

                await _user_store.SetUserNameAsync(user, Input.Email, CancellationToken.None).ConfigureAwait(false);
                await _email_store.SetEmailAsync(user, Input.Email, CancellationToken.None).ConfigureAwait(false);
                var result = await _userManager.CreateAsync(user, Input.Password).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    // use LoggerMessage delegate
                    _userCreated(_logger, null);

                    var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrlString },
                        protocol: Request.Scheme) ?? Url.Content("~/");

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.").ConfigureAwait(false);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrlString });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                        return LocalRedirect(returnUrlString);
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

        private IUserEmailStore<PDFUABoxUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<PDFUABoxUser>)_user_store;
        }
    }
}
