using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace PDFUABox.ConverterServices.Test;

public class UnitTestSign
{
    //need to find a solution for testing under linux, no Arial font out of the box there
    [Theory (Skip = "skip till Clients get into focus")]
    [InlineData(@"C:\PDFUABoxFiles\out\BFSG für Kommunen.pdf")]
    public void TestSigning(string pdfFile)
    {
        var pfxData = "";
        var signContext = new SignContext(pfxData, "Kus111!!");
        //pdfFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", pdfFile);

        Sign.SignDocument(pdfFile, signContext);
        //TODO: Verify signed PDF
    }
}
