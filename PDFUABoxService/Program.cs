using Converter;

using Microsoft.Extensions.Configuration;

#pragma warning disable CA1303 // Literale nicht als lokalisierte Parameter übergeben
Console.WriteLine("Starting Engine");
#pragma warning restore CA1303 // Literale nicht als lokalisierte Parameter übergeben

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Console.WriteLine($"Environment: {environmentName}");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    .Build()
    .GetSection("Converter");

var converter = Converter.ConverterRenameMe.Instance;
converter.Init(configuration);

using Watcher watcher = new Watcher(configuration);
watcher.Start();

#pragma warning disable CA1303 // Literale nicht als lokalisierte Parameter übergeben
Console.WriteLine("Press 'q' to quit.");
#pragma warning restore CA1303 // Literale nicht als lokalisierte Parameter übergeben
while (true)
{
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
    {
        break;
    }
    Thread.Sleep(1000);
}
