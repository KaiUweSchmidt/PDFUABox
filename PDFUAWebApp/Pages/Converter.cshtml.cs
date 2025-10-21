using Converter;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUAWebApp.Pages
{
    public class ConverterModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private Watcher _watcher;

        public ConverterModel(ILogger<IndexModel> logger, Watcher watcher)
        {
            _logger = logger;
            _watcher = watcher;
        }

        public Watcher Watcher { get => _watcher; }

        public void OnGet()
        {
            _logger.LogInformation("Converter page accessed.");
        }
    }
}
