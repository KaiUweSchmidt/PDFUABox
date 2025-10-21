using Aspose.Words.Saving;
using Microsoft.Extensions.Configuration;

namespace Converter;

public class ConverterRenameMe
{
    private static ConverterRenameMe? _instance;
    private static readonly object _lock = new object();
    private bool isInitialized;
#pragma warning disable CS8618 // Sigelnton
    private ConverterRenameMe() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Fügen Sie ggf. den „erforderlichen“ Modifizierer hinzu, oder deklarieren Sie den Modifizierer als NULL-Werte zulassend.

    public static ConverterRenameMe Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new ConverterRenameMe();
                }
                return _instance;
            }
        }
    }

    public bool IsInitialized { get => isInitialized; }
    public string WorkDirectory { get => workDirectory; }
    public string TargetDirectory { get => targetDirectory; }

    private string workDirectory;
    private string targetDirectory;

    /// <summary>
    /// Init needs to be called before any jobs can be added for convertion
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Init(IConfiguration configuration)
    {
        if (isInitialized)
        {
            throw new InvalidOperationException("Converter is already initialized.");
        }

        ArgumentNullException.ThrowIfNull(configuration);

        this.workDirectory = configuration["PDFUABOX_WORK"] ??
            throw new ArgumentException("No configuration for work directory ", nameof(configuration));
        if (string.IsNullOrWhiteSpace(workDirectory))
            throw new ArgumentException("Work directory is null oder leer.", nameof(configuration));

        this.targetDirectory = configuration["PDFUABOX_TARGET"] ??
            throw new ArgumentException("No configuration for target directory ", nameof(configuration));
        if (string.IsNullOrWhiteSpace(targetDirectory))
            throw new ArgumentException("Target directory is null oder leer.", nameof(configuration));

        isInitialized = true;
    }

    public Task<Job> CreateJob(string inputFile, Aspose.Words.Saving.PdfSaveOptions? saveOptions = null)
    {
        if (!isInitialized)
            throw new InvalidOperationException("Converter is not initialized. Call Init() first.");

        string workFile = Path.Combine(workDirectory!, Path.GetFileName(inputFile));
        File.Copy(inputFile, workFile, true);

        return Task.Run(() =>
        {
            Job job = new(Path.GetFileNameWithoutExtension(inputFile),
                workFile, targetDirectory, saveOptions ?? CreateDefaultSaveOptions());
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
