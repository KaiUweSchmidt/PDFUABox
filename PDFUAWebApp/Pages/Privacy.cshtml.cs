using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUABox.WebApp.Pages;

internal sealed class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logPrivacyPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "Privacy page accessed: {Message}");
    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        _logPrivacyPageAccessed(_logger, "started", null);
    }
}
