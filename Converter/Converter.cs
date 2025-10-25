using Aspose.Words.Saving;
using ConverterServices;
using log4net;
using Microsoft.Extensions.Configuration;

namespace PDFUABox.ConverterServices;

public class Converter
{
    private readonly ILog _logger = LogManager.GetLogger(typeof(Converter));

    private readonly JobPool _jobPool;

    public Converter(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var converterConfiguration = configuration.GetSection("Converter");

        this._workDirectory = converterConfiguration["PDFUABOX_WORK"] ??
            throw new ArgumentException("No configuration for work directory ", nameof(configuration));
        if (string.IsNullOrWhiteSpace(_workDirectory))
            throw new ArgumentException("Work directory is null oder leer.", nameof(configuration));

        this._destinationDirectory = converterConfiguration["PDFUABOX_DEST"] ??
            throw new ArgumentException("No configuration for destination directory ", nameof(configuration));
        if (string.IsNullOrWhiteSpace(_destinationDirectory))
            throw new ArgumentException("Target directory is null oder leer.", nameof(configuration));

        _jobPool = new JobPool();

        _logger.Debug($"Converter created with WorkDirectory: {_workDirectory}, TargetDirectory: {_destinationDirectory}");
    }


    public string WorkDirectory { get => _workDirectory; }
    public string TargetDirectory { get => _destinationDirectory; }

    private readonly string _workDirectory;
    private readonly string _destinationDirectory;

    /// <summary>
    /// Create a conversion job
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="saveOptions"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<Guid> CreateJob(string userId, string inputFile, Aspose.Words.Saving.PdfSaveOptions? saveOptions = null)
    {

        _logger.Debug($"CreateJob with inputfile: {inputFile}");

        string workFile = Path.Combine(_workDirectory!, Path.GetFileName(inputFile));
        File.Copy(inputFile, workFile, true);

        using Job job = new(userId,
                            workFile,
                            _destinationDirectory,
                            saveOptions ?? CreateDefaultSaveOptions());
        _jobPool.AddJob(job);
        return Task.FromResult(job.Id);
    }

    public async Task<JobStatus> GetJobStatusAsync(string jobId)
    {
        if (!string.IsNullOrEmpty(jobId))
            throw new ArgumentNullException(nameof(jobId));

        return await Task.Run(() =>
        {
            var job = _jobPool.Jobs.Find(j => j.Id.ToString() == jobId);
            if(job == null)
                throw new InvalidOperationException($"Job with Id {jobId} not found.");
            return Task.FromResult(job.Status);
        }).ConfigureAwait(false);
    }

    private static Aspose.Words.Saving.PdfSaveOptions CreateDefaultSaveOptions()
    {
        Aspose.Words.Saving.PdfSaveOptions saveOptions = new Aspose.Words.Saving.PdfSaveOptions();
        saveOptions.PageMode = PdfPageMode.UseOutlines;
        saveOptions.OutlineOptions.CreateMissingOutlineLevels = false;
        saveOptions.OutlineOptions.CreateOutlinesForHeadingsInTables = true;
        saveOptions.OutlineOptions.HeadingsOutlineLevels = 6;
        saveOptions.OutlineOptions.ExpandedOutlineLevels = 1;
        saveOptions.HeaderFooterBookmarksExportMode = HeaderFooterBookmarksExportMode.All;
        saveOptions.CreateNoteHyperlinks = true;
        saveOptions.ExportDocumentStructure = true;
        saveOptions.Compliance = PdfCompliance.PdfUa1;
        saveOptions.CustomPropertiesExport = PdfCustomPropertiesExport.Metadata;
        saveOptions.UpdateFields = false;
        return saveOptions;
    }

    public IList<Job> GetAllJobs(string userId) {
        return _jobPool.Jobs.Where(j => j.UserId == userId).ToList();
    }
}
