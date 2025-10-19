using CleanArchitecture.Domain.Abstraction;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Users;
public sealed class AppUser : IdentityUser<Guid> 
{


    public AppUser()
    {
        Id = Guid.CreateVersion7();
    }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName  => $"{FirstName} {LastName}"; // computed property

    #region AuditLog

    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreatedUserId { get; set; } = default!;


    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid? UpdatedUserId { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? DeletedUserId { get; set; }
    public bool IsDeleted { get; set; }

    #endregion


}