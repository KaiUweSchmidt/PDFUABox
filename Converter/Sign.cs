namespace Converter;

public static class Sign
{
    public static void SignDocument(string fileToSign)
    {
        // The path to the documents directory
#pragma warning disable S1075 // URIs should not be hardcoded
        string pfxFile= @"C:\Git\PDFUABox\PDFSignContextHandler\pfx\PDFUABoxTestUser.pfx";
#pragma warning restore S1075 // URIs should not be hardcoded
                             // Open PDF document
        using var document = new Aspose.Pdf.Document(fileToSign);
        // Instantiate PdfFileSignature object
        using var signature = new Aspose.Pdf.Facades.PdfFileSignature(document);
        // Create PKCS#7 object for sign
        var pkcs = new Aspose.Pdf.Forms.PKCS7(pfxFile, "password123");
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
