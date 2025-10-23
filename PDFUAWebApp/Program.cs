using PDFUABox.WebApp.Extensions;

namespace PDFUABox.WebApp;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddLog4Net();
        Console.WriteLine($"builder.Environment.ContentRootPath: {Path.GetFullPath(builder.Environment.ContentRootPath)}");
        // Add services to the container.
        builder.Services.AddRazorPages();
        
        builder.Services.AddWatcher(builder.Configuration.GetSection("Converter"));
        
        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseDeveloperExceptionPage();
        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

#pragma warning disable CA1303 // Literale nicht als lokalisierte Parameter übergeben
        Console.WriteLine("Starting Engine");
#pragma warning restore CA1303 // Literale nicht als lokalisierte Parameter übergeben

        Thread.Sleep(500);

        app.Run();
    }
}
