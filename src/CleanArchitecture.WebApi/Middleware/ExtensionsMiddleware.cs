using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace CleanArhictecture_2025.WebAPI;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(p => p.UserName == "admin"))
            {
                AppUser user = new()
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Eray",
                    LastName = "Yaman",
                    EmailConfirmed = true,
                    CreatedAt = DateTimeOffset.Now,
                };

                user.CreatedUserId = user.Id;

                userManager.CreateAsync(user, "123123").Wait();


            }
        }
    }
}