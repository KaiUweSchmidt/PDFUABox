namespace Converter;

public static class Sign
{
    public static void SignDocument(string fileToSign, string pfxFile, string password)
    {
        // The path to the documents directory
        
                             // Open PDF document
        using var document = new Aspose.Pdf.Document(fileToSign);
        // Instantiate PdfFileSignature object
        using var signature = new Aspose.Pdf.Facades.PdfFileSignature(document);
        // Create PKCS#7 object for sign
        var pkcs = new Aspose.Pdf.Forms.PKCS7(pfxFile, password);
        // Sign PDF file
        signature.Sign(1, true, new System.Drawing.Rectangle(300, 100, 400, 200), pkcs);
        // Save PDF document
        var pdfPath = Path.GetDirectoryName(fileToSign);
        if(pdfPath == null)
            throw new InvalidOperationException("pdfPath is null.");
        var signedFileName = Path.GetFileNameWithoutExtension(fileToSign) + "_signed.pdf";
        signature.Save(Path.Combine(pdfPath, signedFileName));
    }
}
