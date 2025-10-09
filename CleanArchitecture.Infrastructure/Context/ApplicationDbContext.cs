
namespace CleanArchitecture.Infrastructure.Context
{
    using CleanArchitecture.Domain.Employees;
    using CleanArchitecture.Infrastructure.Configuration;
    using GenericRepository;
    using Microsoft.EntityFrameworkCore;
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion
    internal sealed class ApplicationDbContext : DbContext , IUnitOfWork
    {

        public ApplicationDbContext(DbContextOptions options) :base(options)
        {
            
        }

        public DbSet<Employee>  Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
