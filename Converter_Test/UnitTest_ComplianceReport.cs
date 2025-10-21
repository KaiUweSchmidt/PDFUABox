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
    [InlineData(@"ComlianceReport_Full.xml")]
    [InlineData(@"ComlianceReport1.xml")]
    public void TestReadComplianceReport(string complianceReportFile)
    {
        complianceReportFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", complianceReportFile);
        var complianceReport = ComplianceReportSerializer.Deserialize(complianceReportFile);
        Assert.NotNull(complianceReport);
        complianceReport!.Name.Should().Be("Log");
        //TODO : more tests
    }
}
