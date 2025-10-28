using System.Runtime.InteropServices;
using Aspose.Pdf;
using Aspose.Words.Fields;
using log4net;
using PDFUABox.ConverterServices.ComplianceReportModels;

namespace PDFUABox.ConverterServices;

public class Job : IDisposable
{
    private readonly ILog _logger = LogManager.GetLogger(typeof(Job));
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserId { get; } = string.Empty;
    public string InputFile { get; set; }
    public string InputFileDisplayName
    {
        get
        {
            return Path.GetFileName(InputFile);
        }
    }

    public string TargetDirectory { get; set; }

    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    public string? ResultFile { get; set; }

    public Stream OutputStream { get; set; }
    Aspose.Words.Saving.PdfSaveOptions SaveOptions { get; set; }

    public JobStatus Status { get; set; } = JobStatus.Pending;

    private readonly SignContext _signContext;

    public Job(string userId, SignContext signContext, string inputFile, string targetDirectory, Aspose.Words.Saving.PdfSaveOptions saveOptions)
    {
        if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("userId is null or empty.", nameof(userId));
        if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentException("InputFile is null or empty.", nameof(inputFile));
        if (string.IsNullOrWhiteSpace(targetDirectory)) throw new ArgumentException("TargetDirectory is null or empty.", nameof(targetDirectory));
        if (saveOptions == null) throw new ArgumentNullException(nameof(saveOptions), "SaveOptions is null.");

        _logger.Debug($"Job created for userId: {userId}, inputFile: {inputFile}, targetDirectory: {targetDirectory}");

        Id = Guid.NewGuid();
        UserId = userId;
        InputFile = inputFile;
        TargetDirectory = targetDirectory;
        OutputStream = new MemoryStream();
        SaveOptions = saveOptions;
        ResultFile = Path.Combine(TargetDirectory!, Path.GetFileNameWithoutExtension(InputFile) + ".pdf");
        _signContext = signContext;
    }

    public void Run()
    {
        if (Status != JobStatus.Pending)
        {
            _logger.Debug($"Job already");
            return;
        }
        Status = JobStatus.Failed;
        if (OutputStream == null)
            throw new InvalidOperationException("OutputStream is null.");
        if (SaveOptions == null)
            throw new InvalidOperationException("SaveOptions is null.");
#pragma warning disable S2139 // Exceptions should be either logged or rethrown but not both
        try
        {
            _logger.Info($"Job for UserId {UserId} and Filename {Path.GetFileName(InputFile)} started.");
            Status = JobStatus.InProgress;
            Aspose.Words.Document doc = new Aspose.Words.Document(InputFile);
            foreach (Field field in doc.Range.Fields)
            {
                if (field.Type == FieldType.FieldHyperlink)
                {
                    FieldHyperlink hyperlink = (FieldHyperlink)field;

                    if (hyperlink.SubAddress != null)
                        continue;
                    if (hyperlink.ScreenTip == null)
                    {
                        hyperlink.ScreenTip = hyperlink.DisplayResult;
                    }

                }
            }

            doc.Save(OutputStream, SaveOptions);

            string validatedFile = Path.Combine(TargetDirectory!, Path.GetFileNameWithoutExtension(InputFile) + "_validated.xml");

            using var pdfDocument = new Aspose.Pdf.Document(OutputStream);
            

            pdfDocument.Save(ResultFile);

            Sign.SignDocument(ResultFile!, _signContext);

#pragma warning disable S125, S1135 // Sections of code should not be commented out
            SetDocumentPrivileges(ResultFile!);
#pragma warning restore S125, S1135 // Sections of code should not be commented out

            var isPDFUA = pdfDocument.Validate(validatedFile, PdfFormat.PDF_UA_1);
            if (!isPDFUA)
            {
                Compliance? compliance = ComplianceReportSerializer.Deserialize(validatedFile);
                if (compliance != null && compliance.File != null
                    && compliance.File.General != null
                    && compliance.File.General.AllProblems() != null)
                {
                    foreach (var problem in compliance.File.General.AllProblems())
                    {
                        Console.WriteLine($"Error Code: {problem.Code}, Code: {problem.Code} Description {problem.Description}");
                    }
                }
                Status = JobStatus.CompletedWithWarnings;

            }
            else
            {
                Status = JobStatus.Completed;
            }
            ResetOutputStream();
        }
        catch (Exception ex)
        {
            _logger.Error($"Error processing job for UserId {UserId} and Filename {Path.GetFileName(InputFile)}: {ex.Message}", ex);
            throw;
        }
        finally
        {
            _logger.Info($"Job for UserId {UserId} and Filename {Path.GetFileName(InputFile)} completed with status {Status}.");
        }
#pragma warning restore S2139 // Exceptions should be either logged or rethrown but not both
    }

#pragma warning disable S1144 // Unused private types or members should be removed
    private static void SetDocumentPrivileges(string pdfFileName)
#pragma warning restore S1144 // Unused private types or members should be removed
    {
        if (string.IsNullOrEmpty(pdfFileName))
            throw new ArgumentNullException(nameof(pdfFileName), "pdfFileName is null or empty.");

        var pdfFilePath = Path.GetDirectoryName(pdfFileName);
        if (pdfFilePath == null)
            throw new InvalidOperationException("pdfFilePath is null.");

        string tempFileName = Path.Combine(Path.GetDirectoryName(pdfFileName) ?? string.Empty,
    Path.GetFileNameWithoutExtension(pdfFileName) + "_secured" + Path.GetExtension(pdfFileName));

        File.Move(pdfFileName, tempFileName, true);

        using var fileSecurity = new Aspose.Pdf.Facades.PdfFileSecurity();
        var privilege = Aspose.Pdf.Facades.DocumentPrivilege.ForbidAll;
        privilege.ChangeAllowLevel = 1;
        privilege.AllowPrint = true;
        privilege.AllowScreenReaders = true;
        privilege.AllowFillIn = true;
        privilege.AllowAssembly = true;

        fileSecurity.BindPdf(tempFileName);

        fileSecurity.SetPrivilege(privilege);

        fileSecurity.Save(pdfFileName);
    }

    private void ResetOutputStream()
    {
        if (OutputStream != null)
        {
            OutputStream.Seek(0, SeekOrigin.Begin);
            OutputStream.Position = 0;
        }
    }

    private void DisposeOutputStream()
    {
        if (OutputStream != null)
        {
            OutputStream.Dispose();
            OutputStream = null!;
        }
    }

    private bool isDisposed;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing)
        {
            // free managed resources
            DisposeOutputStream();
        }

        isDisposed = true;
    }
}
