using Converter;
using Converter.ComplianceReportModels;

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
        //TODO : more tests
    }
}
