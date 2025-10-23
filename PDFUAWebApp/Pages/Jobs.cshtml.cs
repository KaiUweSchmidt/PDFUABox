using PDFUABox.ConverterServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUABox.WebApp.Pages;

internal sealed class JobsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private Watcher _watcher;

    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logJobPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "Job page accessed: {Message}");
    public JobsModel(ILogger<IndexModel> logger, Watcher watcher)
    {
        _logger = logger;
        _watcher = watcher;
    }

    public Watcher Watcher { get => _watcher; }

    public void OnGet()
    {
        _logJobPageAccessed(_logger, "started", null);
    }

}
