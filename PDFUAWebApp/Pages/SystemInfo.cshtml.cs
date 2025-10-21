using Converter;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUAWebApp.Pages;

internal sealed class SystemInfoModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private Watcher _watcher;

    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logSystemInfoPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "System info page accessed: {Message}");
    public SystemInfoModel(ILogger<IndexModel> logger, Watcher watcher)
    {
        _logger = logger;
        _watcher = watcher;
    }

    public Watcher Watcher { get => _watcher; }

    public void OnGet()
    {
        _logSystemInfoPageAccessed(_logger, "started", null);
    }
}
