using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUAWebApp.Pages;

internal sealed class PDFUAHintsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logPDFUAHintsPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "PDFUAHints page accessed: {Message}");
    public PDFUAHintsModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    public void OnGet()
    {
        _logPDFUAHintsPageAccessed(_logger, "started", null);
    }
}
