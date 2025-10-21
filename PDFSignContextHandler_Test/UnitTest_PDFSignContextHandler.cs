using PDFSignContextHandler;

namespace PDFSignContextHandler_Test;

public class UnitTest_PDFSignContextHandler
{
    [Theory]
    [InlineData(@"testdata\BFSG für Kommunen.pdf")]
    public void Test_Signing(string pdfFile)
    {
        Sign.SignDocument(pdfFile);
    }
}