using PDFSignContextHandler;

namespace PDFSignContextHandler_Test;

public class UnitTest_PDFSignContextHandler
{
    [Theory]
    [InlineData(@"testdata\BFSG f�r Kommunen.pdf")]
    public void Test_Signing(string pdfFile)
    {
        Sign.SignDocument(pdfFile);
    }
}