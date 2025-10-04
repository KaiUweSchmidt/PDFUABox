using Converter;

namespace Converter_Test;

public class UnitTest_ComplianceReport
{

    public UnitTest_ComplianceReport()
    {

    }

    [Theory]
    [InlineData(@"testdata\ComlianceReport_Full.xml")]
    [InlineData(@"testdata\ComlianceReport1.xml")]
    public void Test_ReadComplianceReport(string complianceReportFile)
    {
        var complianceReport = ComplianceReportSerializer.Deserialize(complianceReportFile);
        Assert.NotNull(complianceReport);
        //TODO : more tests
    }
}
