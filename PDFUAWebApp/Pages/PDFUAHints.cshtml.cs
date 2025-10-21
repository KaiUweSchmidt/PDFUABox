using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PDFUAWebApp.Pages
{
    public class PDFUAHintsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PDFUAHintsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            _logger.LogInformation("PDFUAHints page accessed.");
        }
    }
}
