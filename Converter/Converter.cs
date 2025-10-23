using Aspose.Words.Saving;
using log4net;
using Microsoft.Extensions.Configuration;

namespace PDFUABox.ConverterServices;

public class Converter
{
    private readonly ILog _logger = LogManager.GetLogger(typeof(Converter));

    public Converter(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var converterConfiguration = configuration.GetSection("Converter");

        this._workDirectory = converterConfiguration["PDFUABOX_WORK"] ??
            throw new ArgumentException("No configuration for work directory ", nameof(configuration));
        if (string.IsNullOrWhiteSpace(_workDirectory))
            throw new ArgumentException("Work directory is null oder leer.", nameof(configuration));

        this._targetDirectory = converterConfiguration["PDFUABOX_TARGET"] ??
            throw new ArgumentException("No configuration for target directory ", nameof(configuration));
        if (string.IsNullOrWhiteSpace(_targetDirectory))
            throw new ArgumentException("Target directory is null oder leer.", nameof(configuration));

        _logger.Debug($"Converter created with WorkDirectory: {_workDirectory}, TargetDirectory: {_targetDirectory}");
    }


    public string WorkDirectory { get => _workDirectory; }
    public string TargetDirectory { get => _targetDirectory; }

    private readonly string _workDirectory;
    private readonly string _targetDirectory;

    /// <summary>
    /// Create a conversion job
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="saveOptions"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<Job> CreateJob(string inputFile, Aspose.Words.Saving.PdfSaveOptions? saveOptions = null)
    {

        _logger.Debug($"CreateJob with inputfile: {inputFile}");

        string workFile = Path.Combine(_workDirectory!, Path.GetFileName(inputFile));
        File.Copy(inputFile, workFile, true);

        return Task.Run(() =>
        {
            Job job = new(Path.GetFileNameWithoutExtension(inputFile),
                workFile, _targetDirectory, saveOptions ?? CreateDefaultSaveOptions());
            try
            {
                job.Run();
                return job;
            }
            catch
            {
                job.Dispose();
                throw;
            }
        });
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

}
