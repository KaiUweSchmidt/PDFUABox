using FluentAssertions;
using Microsoft.Extensions.Configuration;
namespace ConverterTest;

public class UnitTestConverter
{
    public UnitTestConverter()
    {
        var converter = Converter.ConverterRenameMe.Instance;
        if(converter == null)
            throw new InvalidOperationException("Converter instance is null.");

        if (converter.IsInitialized)
            return;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var converterConfiguration = configuration.GetSection("Converter");

        converter.Init(converterConfiguration);
    }

    [Theory]
    [InlineData(@"2025_10_Rechnung_5619948855_bearbeitet.docx")]
    [InlineData(@"BFSG für Kommunen.docx")]
    [InlineData(@"Playbook Barrierefreiheit in Dokumenten.docx")]
    public async Task TestCreateJob(string inputFile)
    {
        inputFile = Path.Combine(Directory.GetCurrentDirectory(), "TestData", inputFile);
        var converter = Converter.ConverterRenameMe.Instance;
        if (converter == null)
            throw new InvalidOperationException("Converter instance is null.");

        if(!Directory.Exists(converter.WorkDirectory)) 
            Directory.CreateDirectory(converter.WorkDirectory);
        if(!Directory.Exists(converter.TargetDirectory))
            Directory.CreateDirectory(converter.TargetDirectory);

        inputFile = Path.GetFullPath(inputFile);

        string workFile = Path.Combine(converter.TargetDirectory, Path.GetFileName(inputFile));
        File.Copy(inputFile, workFile, true);
        
        
        var job = converter.CreateJob(workFile, null);
        Assert.NotNull(job);
        await job.ConfigureAwait(true);
        job.Status.Should().Be(TaskStatus.RanToCompletion);
        
    }
}
