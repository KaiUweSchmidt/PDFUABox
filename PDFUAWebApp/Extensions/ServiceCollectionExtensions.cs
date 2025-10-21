namespace PDFUAWebApp.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWatcher(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<Converter.Watcher>(sp =>
        {
            var watcher = new Converter.Watcher(configuration);
            watcher.Start();
            return watcher;
        });
        return services;
    }
}
