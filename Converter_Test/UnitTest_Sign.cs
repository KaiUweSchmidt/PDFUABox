using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace PDFUABox.ConverterServices.Test;

public class UnitTestSign
{
    //need to find a solution for testing under linux, no Arial font out of the box there
    [Theory (Skip = "skip till Clients get into focus")]
    [InlineData(@"SignTest.pdf")]
    public void TestSigning(string pdfFile)
    {
        pdfFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", pdfFile);
        string pfxFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "PDFUABoxTestUser.pfx");
        Sign.SignDocument(pdfFile,pfxFile, "password123");
        //TODO: Verify signed PDF
    }
}
