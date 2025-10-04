using Aspose.Pdf;
using Aspose.Words.Layout;
using System.Xml.Serialization;

namespace Converter;

public class Job
{
    public string Name { get; set; } = string.Empty;
    public string InputFile { get; set; }
    public string TargetDirectory { get; set; }
    
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
        doc.Save(OutputStream, SaveOptions);

        string validatedFile = Path.Combine(TargetDirectory!, Path.GetFileNameWithoutExtension(InputFile) + "_validated.xml");
        if(OutputStream == null)
            throw new InvalidOperationException("OutputStream is null.");
        if(SaveOptions == null)
            throw new InvalidOperationException("SaveOptions is null.");
        
        var pdfDocument = new Aspose.Pdf.Document(OutputStream);
        var isPDFUA = pdfDocument.Validate(validatedFile, PdfFormat.PDF_UA_1);
        if (!isPDFUA)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Compliance));
            using TextReader reader = new StreamReader(validatedFile);
            Compliance? compliance = (Compliance?)serializer.Deserialize(reader);
            if (compliance != null && compliance.File != null)
            {
                foreach (var problem in compliance.File.General)
                {
                    Console.WriteLine($"Error Code: {problem.Code}, Code: {problem.Code} Description {problem.Value}");
                }
            }
            Status = JobStatus.Completed;
        } else {
            Status = JobStatus.Failed;
        }

        OutputStream.Seek(0, SeekOrigin.Begin);
        OutputStream.Position = 0;
    }
}
