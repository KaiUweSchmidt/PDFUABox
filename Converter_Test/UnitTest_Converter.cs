using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using FluentAssertions;
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
    [InlineData(@"testdata\BFSG für Kommunen.docx")]
    [InlineData(@"testdata\Playbook Barrierefreiheit in Dokumenten.docx")]
    public async Task TestCreateJob(string inputFile)
    {
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