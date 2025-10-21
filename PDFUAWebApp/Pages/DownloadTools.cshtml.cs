using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace PDFUAWebApp.Pages
{
    internal partial class DownloadToolsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // LoggerMessage-Delegate für bessere Performance
        private static readonly Action<ILogger, string, Exception?> _toolsPageAccessed =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(0, nameof(OnGet)),
                "Tools page accessed: {Message}"
            );

        public DownloadToolsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _toolsPageAccessed(_logger, "started", null);
        }
    }
}
