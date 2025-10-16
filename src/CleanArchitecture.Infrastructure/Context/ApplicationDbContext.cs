
namespace CleanArchitecture.Infrastructure.Context;
using CleanArchitecture.Domain.Abstraction;
using CleanArchitecture.Domain.Employees;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion
internal sealed class ApplicationDbContext : DbContext, IUnitOfWork
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
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