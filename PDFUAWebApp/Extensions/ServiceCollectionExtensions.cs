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
            var watcher = new Converter(configuration);
            return watcher;
        });
        return services;
    }

    public static void AddIdentitySeed(this IServiceProvider serviceProvider)
    {
        try
        {             // Intentionally left blank to avoid blocking the main thread
            SeedUserData.CreateUsersAsync(serviceProvider);
        }
        catch (Exception ex)
        {
            var msg = ex.Message;
        }
    }
}
