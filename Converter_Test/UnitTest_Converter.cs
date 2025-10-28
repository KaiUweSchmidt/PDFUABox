using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using PDFUABox.ConverterServices;

namespace PDFUABox.ConverterServices.ConverterTest;

public class UnitTestConverter
{
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _configuration;

    private readonly SignContext _signContext;

    public UnitTestConverter(ITestOutputHelper output)
    {
        _output = output;
        _signContext = new SignContext("TODO", "TODO:Password");
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    [Theory]
    [InlineData(@"2025_10_Rechnung_5619948855_bearbeitet.docx")]
    //[InlineData(@"BFSG für Kommunen.docx")]
    [InlineData(@"Playbook Barrierefreiheit in Dokumenten.docx")]
    public async Task TestCreateJob(string inputFile)
    {
        inputFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", inputFile);
        _output.WriteLine($"TestCreateJob=> Input file: {inputFile}");
        var converter = new Converter(_configuration);

        if (!Directory.Exists(converter.WorkDirectory))
            Directory.CreateDirectory(converter.WorkDirectory);
        if (!Directory.Exists(converter.TargetDirectory))
            Directory.CreateDirectory(converter.TargetDirectory);

        inputFile = Path.GetFullPath(inputFile);
        //TODO: get real user id
        string userId = "TODO_GetPrincipal";

        var job = converter.CreateJob(userId, _signContext, inputFile, null);
        Assert.NotNull(job);
        await job.ConfigureAwait(true);
        job.Status.Should().Be(TaskStatus.RanToCompletion);
    }
}

