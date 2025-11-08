

using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Domain.Abstraction;
public abstract class Entity
{

    public Entity()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    #region AuditLog
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreatedUserId { get; set; } = default!;
    public string CreatedUserName => GetCreatedUserName();


    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? UpdatedUserId { get; set; }
    public string? UpdatedUserName => GetUpdatedUserName();


    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? DeletedUserId { get; set; }
    public bool IsDeleted { get; set; }



    private string GetCreatedUserName()
    {

        HttpContextAccessor httpContext = new();

        var userManager = httpContext
            .HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();

        AppUser appUser = userManager.Users.First(u => u.Id == CreatedUserId);

        return $"{appUser.FirstName} {appUser.LastName} ({appUser.Email})";

    }
    private string? GetUpdatedUserName()
    {

        if(UpdatedUserId is null) return null;

        HttpContextAccessor httpContext = new();

        var userManager = httpContext
            .HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();


        AppUser? appUser = userManager.Users.First(u => u.Id == UpdatedUserId);

        return $"{appUser.FirstName} {appUser.LastName} ({appUser.Email})";


    }
    #endregion

}
