using PDFUAWebApp.Extensions;

namespace PDFUAWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Environment.ContentRootPath = @"C:\PDFUABoxFiles";
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

        Console.WriteLine("Starting Engine");

        Thread.Sleep(500);

        app.Run();
    }
}
