namespace PDFUABox.ConverterServices;

public static class Sign
{
    public static void SignDocument(string fileToSign, SignContext signContext)
    {
        ArgumentNullException.ThrowIfNull(fileToSign);
        ArgumentNullException.ThrowIfNull(signContext);
        // Open PDF document
        using var document = new Aspose.Pdf.Document(fileToSign);
        // Instantiate PdfFileSignature object
        using var signature = new Aspose.Pdf.Facades.PdfFileSignature(document);
        // Create PKCS#7 object for sign
        var pkcs = new Aspose.Pdf.Forms.PKCS7(signContext.GetPfxStream(), signContext.PfxPassword);
        // Sign PDF file
        // this looks ugly, we need to find a better way to position the signature
        signature.Sign(1, true, new System.Drawing.Rectangle(300, 100, 400, 200), pkcs);
        // Save PDF document
        var pdfPath = Path.GetDirectoryName(fileToSign);
        if(pdfPath == null)
            throw new InvalidOperationException("pdfPath is null.");
        signature.Save(fileToSign);
    }
}
