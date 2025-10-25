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

        var adminUserName = "administrator@pdfuabox.de";
        var adminRoleName = "Administrator";

        EnsureRoleExists(context, adminRoleName);
        EnsureUserExists(context, adminUserName, "Admin123@PDFUABox");

        var user = context.Users.FirstOrDefault(u => u.UserName == adminUserName)!;
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

        await context.SaveChangesAsync().ConfigureAwait(false);

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

    private static async void EnsureUserExists(ApplicationAppContext context, string adminUserName, string adminUserPassword)
    {

        if (!context.Users.Any(u => u.UserName == adminUserName))
        {
            
            var adminUser = new PDFUABoxUser
            {
                Email = adminUserName,
                NormalizedEmail = adminUserName.ToUpper(),
                UserName = adminUserName,
                NormalizedUserName = adminUserName.ToUpper(),
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var password = new PasswordHasher<PDFUABoxUser>();
            var hashed = password.HashPassword(adminUser, adminUserPassword);
            adminUser.PasswordHash = hashed;

            using var userStore = new UserStore<PDFUABoxUser>(context);
            await userStore.CreateAsync(adminUser).ConfigureAwait(false);
        }
    }
}
