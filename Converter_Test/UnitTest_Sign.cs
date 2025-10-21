using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Converter.Test;

public class UnitTestSign
{
    [Theory]
    [InlineData(@"SignTest.pdf")]
    public void TestSigning(string pdfFile)
    {
        pdfFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", pdfFile);
        string pfxFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "PDFUABoxTestUser.pfx");
        Sign.SignDocument(pdfFile,pfxFile, "password123");
        //TODO: Verify signed PDF
    }
}
