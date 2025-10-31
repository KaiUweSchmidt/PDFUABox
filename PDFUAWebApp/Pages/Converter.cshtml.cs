using Aspose.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PDFUABox.ConverterServices;
using PDFUABox.WebApp.Areas.Identity.Data;
using PDFUABox.WebApp.Extensions;

namespace PDFUABox.WebApp.Pages;


[Authorize]
internal partial class ConverterModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly Converter _converter;
    private readonly UserManager<PDFUABoxUser> _userManager;
    private readonly string _uploadFolder = string.Empty;
    private readonly string _outputFolder = string.Empty;

    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logConverterOnGet =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "{Message}");

    private static readonly Action<ILogger, string, Exception?> _logConverterOnPostAsync =
    LoggerMessage.Define<string>(
        LogLevel.Information,
        new EventId(0, nameof(OnPostAsync)),
        "{Message}");

    public ConverterModel(ILogger<IndexModel> logger, IWebHostEnvironment environment, Converter converter, UserManager<PDFUABoxUser> userManager)
    {
        _logger = logger;
        _environment = environment;
        _converter = converter;
        _userManager = userManager;
        _uploadFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "PDFUABoxFiles", "in");
        if (Directory.Exists(_uploadFolder) == false)
        {
            Directory.CreateDirectory(_uploadFolder);
        }
        _outputFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "PDFUABoxFiles", "out");
        if (Directory.Exists(_outputFolder) == false)
        {
            Directory.CreateDirectory(_outputFolder);
        }
        
    }

    [BindProperty]
    public required IFormFile Upload { get; set; }
    public IActionResult OnPostAsync()
    {
        _logConverterOnPostAsync(_logger, $"File upload started: {Upload.FileName}", null);

        var file = Path.Combine(_uploadFolder, Upload.FileName);
        var outputFile = Path.Combine(_outputFolder, Path.GetFileNameWithoutExtension(Upload.FileName) + "_converted.pdf");
        using var fileStream = new FileStream(file, FileMode.Create);

        Upload.CopyTo(fileStream); // TODO: make async
        var userTask = _userManager.GetUserAsync(User);
        userTask.Wait();
        var user = userTask.Result;
        if (user == null || user.Certificate == null || user.CertificatePassword == null)
        {
            // Fehlerbehandlung, z.B. Redirect oder Fehlermeldung anzeigen
            ModelState.AddModelError(string.Empty, "Benutzerzertifikat oder Passwort nicht gefunden.");
            return Page();
        }
        SignContext signContext = new SignContext(user.Certificate, user.CertificatePassword);
        var jobId = _converter.CreateJob(user.Id, signContext, file).ConfigureAwait(false); 
        return RedirectToPage("/Jobs");
    }

    public Converter Converter { get => _converter; }

    public void OnGet()
    {
        _logConverterOnGet(_logger, "Converter page accessed: started", null);
    }
}
