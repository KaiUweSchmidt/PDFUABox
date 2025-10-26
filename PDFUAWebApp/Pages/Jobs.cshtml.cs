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
    private readonly List<Job> _jobs;

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
        _jobs = new List<Job>();
    }


    public IList<Job> Jobs
    {
        get
        {
            if(_jobs.Count == 0)
            {
                string userId = _userManager.GetUserIdSafe(User);
                var jobs = _converter.GetAllJobs(userId);
                if ((jobs != null))
                {
                    jobs.ToList().ForEach(j => _jobs!.Add(j));
                }
            }
            return _jobs;
        }
    }

    public void OnGet()
    {
        _logJobPageAccessed(_logger, "started", null);
    }

    public IActionResult OnGetJobStatus(string jobId)
    {
        var job = Jobs.FirstOrDefault(j => j.Id.ToString() == jobId);
        if (job != null)
        {
            return new JsonResult(new { status = job.Status.ToString() });
        }
        else
        {
            return new JsonResult(new { status = "Job missing !!!" });
        }
    }

    public IActionResult OnGetDownloadPDF(string fileName)
    {
        var userId = _userManager.GetUserIdSafe(User);
        if (_converter.GetAllJobs(userId).All(j => j.ResultFile != fileName))
        {
            // User does not have access to this file
            return NotFound();
        }
        var filePath = Path.Combine("Documents", fileName); // Adjust path as needed
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        var contentType = "application/pdf";
        return File(fileBytes, contentType, Path.GetFileName(fileName));
    }

}
