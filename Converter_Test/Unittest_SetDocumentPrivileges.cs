using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Words.Saving;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace PDFUABox.ConverterServices.Test;

public class UnittestSetDocumentPrivileges
{
    public UnittestSetDocumentPrivileges()
    {
        //TODO : Load license from file
    }

    [Theory]
    [InlineData(@"testdata\BFSG für Kommunen.docx")]
    public void TestSetDocumentPrivilegesWithCreatingPDF(string docxFile)
    {
        var tempPath = Path.GetFullPath(Path.GetDirectoryName(docxFile)!) ?? string.Empty;
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(docxFile);
        Aspose.Words.Document doc = new Aspose.Words.Document(docxFile);
        var saveOptions = CreateDefaultSaveOptions("User@SecureRandomPassword123!", "Owner@SecureRandomPassword123!");
        string tempPdfFile = Path.Combine(tempPath, fileNameWithoutExtension + "_tempWithUserAndOwner.pdf");
        if(File.Exists(tempPdfFile)) File.Delete(tempPdfFile);
        doc.Save(tempPdfFile, saveOptions);
        //doc.ProtectionType.Should().Be(Aspose.Words.ProtectionType.ReadOnly);
        
        File.Copy(tempPdfFile, Path.Combine(@"C:\PDFUABoxFiles\stmas",Path.GetFileName(tempPdfFile)) , true);

        doc = new Aspose.Words.Document(docxFile);
        saveOptions = CreateDefaultSaveOptions("", "Owner@SecureRandomPassword123!");
        tempPdfFile = tempPdfFile = Path.Combine(tempPath, fileNameWithoutExtension + "_tempJustOwner.pdf");
        if (File.Exists(tempPdfFile)) File.Delete(tempPdfFile);
        doc.Save(tempPdfFile, saveOptions);
        File.Copy(tempPdfFile, Path.Combine(@"C:\PDFUABoxFiles\stmas", Path.GetFileName(tempPdfFile)), true);
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
