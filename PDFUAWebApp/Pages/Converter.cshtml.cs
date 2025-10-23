using PDFUABox.ConverterServices;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PDFUABox.WebApp.Pages;

internal partial class ConverterModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private Watcher _watcher;

    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logConverterPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "Converter page accessed: {Message}");

    public ConverterModel(ILogger<IndexModel> logger, Watcher watcher)
    {
        _logger = logger;
        _watcher = watcher;
    }

    public Watcher Watcher { get => _watcher; }

    public void OnGet()
    {
        _logConverterPageAccessed(_logger, "started", null);
    }
}
