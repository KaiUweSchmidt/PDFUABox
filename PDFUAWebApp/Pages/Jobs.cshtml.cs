using Converter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUAWebApp.Pages
{
    public class JobsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private Watcher _watcher;

        public JobsModel(ILogger<IndexModel> logger, Watcher watcher)
        {
            _logger = logger;
            _watcher = watcher;
        }

        public Watcher Watcher { get => _watcher; }

        public void OnGet()
        {
            _logger.LogInformation("Jobs page accessed.");
        }

    }
}
