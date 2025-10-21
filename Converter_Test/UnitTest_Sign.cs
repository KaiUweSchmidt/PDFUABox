using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Converter.Test;

public class UnitTestSign
{
    [Theory]
    [InlineData(@"BFSG für Kommunen.pdf")]
    public void TestSigning(string pdfFile)
    {
        pdfFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", pdfFile);
        Sign.SignDocument(pdfFile);
    }
}
