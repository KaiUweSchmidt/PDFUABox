using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUAWebApp.Pages
{
    public class DownloadToolsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DownloadToolsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            _logger.LogInformation("Tools page accessed.");
        }
    }
}
