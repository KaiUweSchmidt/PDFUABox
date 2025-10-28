using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PDFUABox.WebApp.Data;

namespace PDFUABox.WebApp.Areas.Identity.Data;

public static class SeedUserData
{
    public static async void CreateUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationAppContext>();

        PDFUABoxUser adminUser = new PDFUABoxUser
        {
            FirstName = "System",
            LastName = "Administrator",
            Email = "administrator@pdfuabox.de",
            NormalizedEmail = "administrator@pdfuabox.de".ToUpper(),
            UserName = "administrator@pdfuabox.de",
            NormalizedUserName = "administrator@pdfuabox.de".ToUpper(),
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            CertificatePassword = "<none>",
            Certificate = "<none>"
        };

        CreateAdminUser(context, adminUser, "Administrator", "Admin123@PDFUABox");

        await context.SaveChangesAsync().ConfigureAwait(false);

        adminUser = new PDFUABoxUser
        {
            FirstName = "Kai-Uwe",
            LastName = "Schmidt",
            Email = "kai-uwe.schmidt@gmx.de",
            NormalizedEmail = "kai-uwe.schmidt@gmx.de".ToUpper(),
            UserName = "kai-uwe.schmidt@gmx.de",
            NormalizedUserName = "kai-uwe.schmidt@gmx.de".ToUpper(),
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            CertificatePassword = "Kus111!!",
            Certificate = ""
        };

        adminUser.Certificate = SignHelpers.CreateCertificate(adminUser.Email, adminUser.CertificatePassword);

        CreateAdminUser(context, adminUser, "Administrator", adminUser.CertificatePassword);

        await context.SaveChangesAsync().ConfigureAwait(false);

    }

    private static void CreateAdminUser(ApplicationAppContext context, PDFUABoxUser adminUser, string adminRoleName, string password)
    {
        EnsureRoleExists(context, adminRoleName);
        EnsureUserExists(context, adminUser, password);

        var user = context.Users.FirstOrDefault(u => u.UserName == adminUser.UserName)!;
        var role = context.Roles.FirstOrDefault(r => r.Name == adminRoleName)!;

        if (!context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
        {
            // Ensure the "admin" role exists

            var adminRoleEntity = context.Roles.First(r => r.Name == adminRoleName);
            var userRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = adminRoleEntity.Id
            };
            context.UserRoles.Add(userRole);
        }
    }

    private static async void EnsureRoleExists(ApplicationAppContext context, string adminRoleName)
    {
        if (!context.Roles.Any(r => r.Name == adminRoleName))
        {
            using var roleStore = new RoleStore<IdentityRole>(context);
            var adminRole = new IdentityRole(adminRoleName);
            await roleStore.CreateAsync(adminRole).ConfigureAwait(false);
        }
    }

    private static async void EnsureUserExists(ApplicationAppContext context, PDFUABoxUser adminUser, string adminUserPassword)
    {

        if (!context.Users.Any(u => u.UserName == adminUser.UserName))
        {

            var password = new PasswordHasher<PDFUABoxUser>();
            var hashed = password.HashPassword(adminUser, adminUserPassword);
            adminUser.PasswordHash = hashed;

            using var userStore = new UserStore<PDFUABoxUser>(context);
            await userStore.CreateAsync(adminUser).ConfigureAwait(false);
        }
    }
}
