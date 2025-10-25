using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PDFUABox.ConverterServices;

namespace PDFUABox.WebApp.Pages;


[Authorize]
internal partial class ConverterModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private IWebHostEnvironment _environment;
    private Converter _converter;
    private readonly string _uploadFolder = string.Empty;
    private readonly string _outputFolder = string.Empty;

    // LoggerMessage-Delegate für die LogInformation-Nachricht
    private static readonly Action<ILogger, string, Exception?> _logConverterOnGet =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(0, nameof(OnGet)),
            "{Message}");

    private static readonly Action<ILogger, string, Exception?> _logConverterOnPostAsync =
    LoggerMessage.Define<string>(
        LogLevel.Information,
        new EventId(0, nameof(OnPostAsync)),
        "{Message}");

    public ConverterModel(ILogger<IndexModel> logger, IWebHostEnvironment environment, Converter converter)
    {
        _logger = logger;
        _environment = environment;
        _converter = converter;
        _uploadFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "PDFUABoxFiles", "in");
        if (Directory.Exists(_uploadFolder) == false)
        {
            Directory.CreateDirectory(_uploadFolder);
        }
        _outputFolder = Path.Combine(_environment.ContentRootPath, "wwwroot", "PDFUABoxFiles", "out");
        if (Directory.Exists(_outputFolder) == false)
        {
            Directory.CreateDirectory(_outputFolder);
        }
    }

    [BindProperty]
    public required IFormFile Upload { get; set; }
    public async Task<FileContentResult> OnPostAsync()
    {
        _logConverterOnPostAsync(_logger, $"File upload started: {Upload.FileName}", null);

        var file = Path.Combine(_uploadFolder, Upload.FileName);


        var outputFile = Path.Combine(_outputFolder, Path.GetFileNameWithoutExtension(Upload.FileName) + "_converted.pdf");
        
        using var fileStream = new FileStream(file, FileMode.Create);
        await Upload.CopyToAsync(fileStream).ConfigureAwait(false);
        var job = await _converter.CreateJob(file).ConfigureAwait(false);
        if(job.OutputStream != null)
        {
            
            
            System.IO.File.Move(job.ResultFile, outputFile,true); //TODO: Review if this is necessary
            using var outputFileStream = new FileStream(outputFile, FileMode.Create);
            job.OutputStream.Position = 0;
            await job.OutputStream.CopyToAsync(outputFileStream).ConfigureAwait(false);
            _logConverterOnPostAsync(_logger, $"File conversion completed: {outputFile}", null);
            outputFileStream.Close();
            return File(
                await System.IO.File.ReadAllBytesAsync(outputFile).ConfigureAwait(false)
                , "application/pdf",
                Path.GetFileName(outputFile));
        }
        return null!;
    }

    public Converter Converter { get => _converter; }

    public void OnGet()
    {
        _logConverterOnGet(_logger, "Converter page accessed: started", null);
    }
}
