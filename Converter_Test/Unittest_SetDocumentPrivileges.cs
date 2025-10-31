using Aspose.Words.Saving;
using FluentAssertions;

namespace PDFUABox.ConverterServices.Test;

public class UnittestSetDocumentPrivileges
{
    private readonly Aspose.Pdf.License _pdfLicense = new Aspose.Pdf.License();
    private readonly Aspose.Words.License _wordsLicense = new Aspose.Words.License();

    public UnittestSetDocumentPrivileges()
    {
        _pdfLicense.SetLicense("Aspose.Total.NET.lic");
        _wordsLicense.SetLicense("Aspose.Total.NET.lic");
    }

    [Theory]
    [InlineData(@"testdata\BFSG für Kommunen.docx", "User@SecureRandomPassword123!", "Owner@SecureRandomPassword123!", "_tempWithUserAndOwner.pdf")]
    [InlineData(@"testdata\Passwortgeschützte Dokumente.docx", "", "Owner@SecureRandomPassword123!", "_tempJustOwner.pdf")]
    public void TestSetDocumentPrivilegesWithCreatingPDF(string docxFile, string userPassword, string ownerPassword, string outputPostfix)
    {
        var tempPath = Path.GetFullPath(Path.GetDirectoryName(docxFile)!) ?? string.Empty;
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(docxFile);
        Aspose.Words.Document doc = new Aspose.Words.Document(docxFile);
        var saveOptions = CreateDefaultSaveOptions(userPassword, ownerPassword);
        string tempPdfFile = Path.Combine(tempPath, fileNameWithoutExtension + outputPostfix);
        tempPdfFile.Should().NotBeNullOrEmpty();
        if (File.Exists(tempPdfFile)) File.Delete(tempPdfFile);
        doc.Save(tempPdfFile, saveOptions);
        using var pdfDocument = new Aspose.Pdf.Document(tempPdfFile, userPassword);
        pdfDocument.Permissions.Should().Be((int)PdfPermissions.ContentCopyForAccessibility);

        if (File.Exists(tempPdfFile)) File.Delete(tempPdfFile);
    }


    private static Aspose.Words.Saving.PdfSaveOptions CreateDefaultSaveOptions(string userPassword, string ownerPassword)
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
        saveOptions.EncryptionDetails = new Aspose.Words.Saving.PdfEncryptionDetails(userPassword, ownerPassword, PdfPermissions.ContentCopyForAccessibility);
        return saveOptions;
    }

}
