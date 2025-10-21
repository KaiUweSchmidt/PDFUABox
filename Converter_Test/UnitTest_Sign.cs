using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Converter.Test;

public class UnitTestSign
{
    [Theory]
    [InlineData(@"testdata\BFSG für Kommunen.pdf")]
    public void TestSigning(string pdfFile)
    {
        pdfFile = Path.GetFullPath(pdfFile);
        Sign.SignDocument(pdfFile);
    }
}
