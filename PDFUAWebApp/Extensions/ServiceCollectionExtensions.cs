using PDFUABox.ConverterServices;
using PDFUABox.WebApp.Areas.Identity.Data;
namespace PDFUABox.WebApp.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWatcher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Watcher>(sp =>
        {
            var watcher = new Watcher(configuration);
            watcher.Start();
            return watcher;
        });
        return services;
    }

    public static IServiceCollection AddConverter(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Converter>(sp =>
        {
            var converter = new Converter(configuration);
            return converter;
        });
        return services;
    }

    public static IServiceCollection AddJobWorker(this IServiceCollection services)
    {
        services.AddSingleton<JobWorker>(sp =>
        {
            var converter = services.BuildServiceProvider().GetRequiredService<Converter>();
            var jobWorker = new JobWorker(converter);
            return jobWorker;
        });
        return services;
    }

    public static void AddIdentitySeed(this IServiceProvider serviceProvider)
    {
        try
            {   // Intentionally left blank to avoid blocking the main thread
            SeedUserData.CreateUsersAsync(serviceProvider);
        }
        catch (InvalidOperationException ex)
        {
            var msg = ex.Message;
            Console.WriteLine($"Fehler InvalidOperationException beim Anlegen der Standardbenutzer: {msg}");
        }
        catch (ArgumentNullException ex)
        {
            var msg = ex.Message;
            Console.WriteLine($"Fehler ArgumentNullException beim Anlegen der Standardbenutzer: {msg}");
        }
        // Weitere spezifische Ausnahmen können hier ergänzt werden, falls bekannt
    }
}
