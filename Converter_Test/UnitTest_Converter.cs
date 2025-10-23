using FluentAssertions;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PDFUABox.WebApp;
namespace ConverterTest;

public class UnitTestConverter
{
    private readonly ITestOutputHelper _output;
    private readonly IConfiguration _configuration;

    public UnitTestConverter(ITestOutputHelper output)
    {
        _output = output;


        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        //var converterConfiguration = configuration.GetSection("Converter");

    }

    [Theory]
    [InlineData(@"2025_10_Rechnung_5619948855_bearbeitet.docx")]
    //[InlineData(@"BFSG für Kommunen.docx")]
    [InlineData(@"Playbook Barrierefreiheit in Dokumenten.docx")]
    public async Task TestCreateJob(string inputFile)
    {

        await Task.Run(
            () =>
            {
                inputFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", inputFile);
                File.Exists(inputFile).Should().BeTrue("Input file should exist for the test.");
            }, TestContext.Current.CancellationToken
            );

        //        using var factory = new WebApplicationFactory<Program>()
        //.WithWebHostBuilder(builder =>
        //{
        //    builder.ConfigureLogging(logging =>
        //    {
        //        // Add xUnit logger provider
        //        logging.Services.AddSingleton<ILoggerProvider>(
        //        new XUnitLoggerProvider(_testOutputHelper, appendScope: false));
        //    });
        //});

        //        inputFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", inputFile);
        //        var converter = new Converter.ConverterRenameMe.Instance;
        //        if (converter == null)
        //            throw new InvalidOperationException("Converter instance is null.");

        //        if(!Directory.Exists(converter.WorkDirectory)) 
        //            Directory.CreateDirectory(converter.WorkDirectory);
        //        if(!Directory.Exists(converter.TargetDirectory))
        //            Directory.CreateDirectory(converter.TargetDirectory);

        //        inputFile = Path.GetFullPath(inputFile);

        //        string workFile = Path.Combine(converter.TargetDirectory, Path.GetFileName(inputFile));
        //        File.Copy(inputFile, workFile, true);


        //        var job = converter.CreateJob(workFile, null);
        //        Assert.NotNull(job);
        //        await job.ConfigureAwait(true);
        //        job.Status.Should().Be(TaskStatus.RanToCompletion);

    }
}
