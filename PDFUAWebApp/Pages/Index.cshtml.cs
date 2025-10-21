using Converter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUAWebApp.Pages;

internal sealed class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private Converter.Watcher _watcher;

    // LoggerMessage-Delegate f�r die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logIndexPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "Index page accessed: {Message}");

    public IndexModel(ILogger<IndexModel> logger, Converter.Watcher watcher)
    {
        _logger = logger;
        _watcher = watcher;
    }

    public Watcher Watcher { get => _watcher;}

    public void OnGet()
    {
        _logIndexPageAccessed(_logger, "started", null);
    }
}
