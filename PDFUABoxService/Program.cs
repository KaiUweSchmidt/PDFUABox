using Converter;

using Microsoft.Extensions.Configuration;

namespace PDFUABoxService;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting Engine");

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.WriteLine($"Environment: {environmentName}");

        var sourceFileDirectory = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .Build()
            .GetSection("Converter");

        var converter = Converter.Converter.Instance;
        converter.Init(configuration);

        Watcher watcher = new Watcher(configuration);
        watcher.Start();
        while (true)
        {
            Thread.Sleep(1000);
        }


    }
}
