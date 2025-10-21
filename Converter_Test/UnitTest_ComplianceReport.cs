using Converter;
using Converter.ComplianceReportModels;
using FluentAssertions;

namespace ConverterTest;

public class UnitTestComplianceReport
{

    public UnitTestComplianceReport()
    {

    }

    [Theory]
    [InlineData(@"testdata\ComlianceReport_Full.xml")]
    //[InlineData(@"testdata\ComlianceReport1.xml")]
    public void TestReadComplianceReport(string complianceReportFile)
    {
        var complianceReport = ComplianceReportSerializer.Deserialize(complianceReportFile);
        Assert.NotNull(complianceReport);
        complianceReport!.Name.Should().Be("PDFUA-1 Validation Report");
        //TODO : more tests
    }
}
