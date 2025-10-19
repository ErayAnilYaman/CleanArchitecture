

using CleanArchitecture.Domain.Abstraction;
using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
#region Usings

using System;
using System.Threading;
using System.Threading.Tasks;

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

        foreach (var entity in entries)
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property(p => p.CreatedAt).CurrentValue = DateTimeOffset.Now;
            }

            if (entity.State == EntityState.Modified)
            {
                if (entity.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entity.Property(p => p.DeletedAt).CurrentValue = DateTimeOffset.Now;
                }
                else
                {
                    entity.Property(p => p.UpdatedAt).CurrentValue = DateTimeOffset.Now;
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