using FluentAssertions;
using Microsoft.Extensions.Configuration;
namespace Converter_Test;

public class UnitTest_Sign
{
    [Theory]
    [InlineData(@"testdata\BFSG für Kommunen.pdf")]
    public void Test_Signing(string pdfFile)
    {
        pdfFile = Path.GetFullPath(pdfFile);
        Converter.Sign.SignDocument(pdfFile);
    }
}