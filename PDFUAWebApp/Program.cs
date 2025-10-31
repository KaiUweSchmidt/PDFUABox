using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PDFUABox.ConverterServices;
using PDFUABox.WebApp.Areas.Identity.Data;
using PDFUABox.WebApp.Data;
using PDFUABox.WebApp.Extensions;

namespace PDFUABox.WebApp;

internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("ApplicationAppContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationAppContextConnection' not found.");

        builder.Services.AddDbContext<ApplicationAppContext>(options => options.UseSqlServer(connectionString));

        builder.Services.AddDefaultIdentity<PDFUABoxUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationAppContext>();
        builder.Services.AddHealthCheck(builder.Configuration);

        builder.Logging.AddLog4Net();
        Console.WriteLine($"builder.Environment.ContentRootPath: {Path.GetFullPath(builder.Environment.ContentRootPath)}");
        // Add services to the container.
        builder.Services.AddRazorPages();
        
        builder.Services.AddWatcher(builder.Configuration.GetSection("Converter"));
        builder.Services.AddConverter(builder.Configuration);

        builder.Services.AddSingleton<JobWorker>();

        var app = builder.Build();

        app.UseHealthChecks("/health");

        var jobWorker = app.Services.GetRequiredService<JobWorker>();
        jobWorker.Start();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.Services.AddIdentitySeed();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseDeveloperExceptionPage();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

#pragma warning disable CA1303 // Literale nicht als lokalisierte Parameter �bergeben
        Console.WriteLine("Starting Engine");
#pragma warning restore CA1303 // Literale nicht als lokalisierte Parameter �bergeben

        Thread.Sleep(500);

        app.Run();
    }
}
