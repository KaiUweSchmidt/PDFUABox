using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PDFUABox.ConverterServices;
using PDFUABox.WebApp.Areas.Identity.Data;
using PDFUABox.WebApp.Extensions;

namespace PDFUABox.WebApp.Pages;

internal sealed class JobsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Converter _converter;
    private readonly UserManager<PDFUABoxUser> _userManager;
    private IList<Job> _jobs;

    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logJobPageAccessed =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "Job page accessed: {Message}");

    public JobsModel(ILogger<IndexModel> logger, Converter converter, UserManager<PDFUABoxUser> userManager)
    {

        _logger = logger;
        _converter = converter;
        _userManager = userManager;
    }


    public IList<Job> Jobs
    {
        get
        {
            if(_jobs is null)
            {
                string userId = _userManager.GetUserIdSafe(User);
                var jobs = _converter.GetAllJobs(userId);
                _jobs = jobs.ToList();
            }
            return _jobs;
        }
    }

    public void OnGet()
    {
        _logJobPageAccessed(_logger, "started", null);
    }

}
