using System.Runtime.InteropServices;
using Aspose.Pdf;
using Aspose.Words.Fields;
using Converter.ComplianceReportModels;

namespace Converter;

public class Job : IDisposable
{
    public string Name { get; set; } = string.Empty;
    public string InputFile { get; set; }
    public string TargetDirectory { get; set; }
    
    public string? ResultFile { get; set; }

    public Stream OutputStream { get; set; }
    Aspose.Words.Saving.PdfSaveOptions SaveOptions { get; set; }

    JobStatus Status { get; set; } = JobStatus.Pending;

    public Job(string name, string inputFile, string targetDirectory, Aspose.Words.Saving.PdfSaveOptions saveOptions)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is null or empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(inputFile)) throw new ArgumentException("InputFile is null or empty.", nameof(inputFile));
        if (string.IsNullOrWhiteSpace(targetDirectory)) throw new ArgumentException("TargetDirectory is null or empty.", nameof(targetDirectory));
        if (saveOptions == null) throw new ArgumentNullException(nameof(saveOptions), "SaveOptions is null.");
        Name = name;
        InputFile = inputFile;
        TargetDirectory = targetDirectory;
        OutputStream = new MemoryStream();
        SaveOptions = saveOptions;
    }



    public void Run()
    {
        Status = JobStatus.InProgress;

        Aspose.Words.Document doc = new Aspose.Words.Document(InputFile);
        foreach (Field field in doc.Range.Fields)
        {
            if (field.Type == FieldType.FieldHyperlink)
            {
                FieldHyperlink hyperlink = (FieldHyperlink)field;

                if (hyperlink.SubAddress != null)
                    continue;
                if(hyperlink.ScreenTip == null)
                {
                    hyperlink.ScreenTip = "Test";
                }
                
            }
        }


        doc.Save(OutputStream, SaveOptions);

        string validatedFile = Path.Combine(TargetDirectory!, Path.GetFileNameWithoutExtension(InputFile) + "_validated.xml");
        if(OutputStream == null)
            throw new InvalidOperationException("OutputStream is null.");
        if(SaveOptions == null)
            throw new InvalidOperationException("SaveOptions is null.");
        
        using var pdfDocument = new Aspose.Pdf.Document(OutputStream);
        ResultFile = Path.Combine(TargetDirectory!, Path.GetFileNameWithoutExtension(InputFile) + ".pdf");

        pdfDocument.Save(ResultFile);

        SetDocumentPrivileges(ResultFile);
        
        
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
            Status = JobStatus.Completed;

        }
        else {
            Status = JobStatus.Failed;
        }

        ResetOutputStream();
    }

    private static void SetDocumentPrivileges(string pdfFileName)
    {
        if (string.IsNullOrEmpty(pdfFileName))
            throw new ArgumentNullException(nameof(pdfFileName), "pdfFileName is null or empty.");

        var pdfFilePath = Path.GetDirectoryName(pdfFileName);
        if (pdfFilePath == null)
            throw new InvalidOperationException("pdfFilePath is null.");


        using var fileSecurity = new Aspose.Pdf.Facades.PdfFileSecurity();

        pdfFileName = Path.Combine(pdfFilePath, "Encrypt Test.pdf");
        fileSecurity.BindPdf(pdfFileName);

        fileSecurity.EncryptFile("User_P@ssw0rd", "OwnerP@ssw0rd", Aspose.Pdf.Facades.DocumentPrivilege.Print, Aspose.Pdf.Facades.KeySize.x256,
    Aspose.Pdf.Facades.Algorithm.AES);

        string encryptedFileName = Path.Combine(Path.GetDirectoryName(pdfFileName) ?? string.Empty,
            Path.GetFileNameWithoutExtension(pdfFileName) + "_secured" + Path.GetExtension(pdfFileName));

        fileSecurity.Save(encryptedFileName);
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
