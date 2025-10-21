using FluentAssertions;
using Microsoft.Extensions.Configuration;
namespace Converter_Test;

public class UnitTest_Converter
{
    public UnitTest_Converter()
    {
        var converter = Converter.Converter.Instance;
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
    [InlineData(@"testdata\2025_10_Rechnung_5619948855_bearbeitet.docx")]
    [InlineData(@"testdata\BFSG für Kommunen.docx")]
    [InlineData(@"testdata\Playbook Barrierefreiheit in Dokumenten.docx")]
    [InlineData(@"testdata\Barrierefreiheitspflichten für Gemeinden.docx")]
    
    public async Task Test_CreateJob(string inputFile)
    {
        var converter = Converter.Converter.Instance;
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
        await job;
        job.Status.Should().Be(TaskStatus.RanToCompletion);
        
    }
}