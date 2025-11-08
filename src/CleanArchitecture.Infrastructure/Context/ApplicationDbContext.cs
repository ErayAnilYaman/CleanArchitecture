
#region Usings


using CleanArchitecture.Domain.Abstraction;
using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
#endregion

namespace CleanArchitecture.Infrastructure.Context;
internal sealed class ApplicationDbContext : IdentityDbContext<AppUser , IdentityRole<Guid> , Guid>, IUnitOfWork
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
        modelBuilder.Ignore<IdentityRole<Guid>>();

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        var entries = ChangeTracker.Entries<Entity>();
        HttpContextAccessor httpContext = new();

        string userIdString = httpContext.HttpContext!.User.Claims.First(f => f.Type == "user-id").Value;
        Guid userId = Guid.Parse(userIdString);


        foreach (var entity in entries)
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;

                entity.Property(p => p.CreatedUserId).CurrentValue = userId;
            }

            if (entity.State == EntityState.Modified)
            {
                if (entity.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entity.Property(p => p.DeletedAt).CurrentValue = DateTimeOffset.Now;
                    entity.Property(p => p.DeletedUserId).CurrentValue = userId;
                }
                else
                {
                    entity.Property(p => p.UpdatedAt).CurrentValue = DateTimeOffset.Now;
                    entity.Property(p => p.UpdatedUserId).CurrentValue = userId;
                }
            }
            if (entity.State == EntityState.Deleted)
            {
                throw new ArgumentException("Veritabani uzerinden direkt silme islemi yapamazsiniz !");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}