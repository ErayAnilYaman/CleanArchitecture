using CleanArchitecture.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Configuration
{
    internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {

            builder.OwnsOne(p => p.Information, builder =>
            {
                builder.Property(p => p.TCNo).HasColumnName("TCNo");
                builder.Property(p => p.Phone1).HasColumnName("Phone1");
                builder.Property(p => p.Email).HasColumnName("Email");
                builder.Property(p => p.Phone2).HasColumnName("Phone2");

            });

            builder.OwnsOne(p => p.Address, builder => 
            {
                builder.Property(p => p.Town).HasColumnName("Town");
                builder.Property(p => p.City).HasColumnName("City");
                builder.Property(p => p.Country).HasColumnName("Country");
                builder.Property(p => p.FullAddress).HasColumnName("FullAddress");
            });
            builder.Property(p => p.Salary).HasColumnType("money");


        }
    }
}
