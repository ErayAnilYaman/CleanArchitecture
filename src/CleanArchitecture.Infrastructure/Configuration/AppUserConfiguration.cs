using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configuration;



internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{

    public void Configure(EntityTypeBuilder<AppUser> builder)
    {


        builder.HasIndex(b=>b.UserName).IsUnique();
        builder.Property(b=>b.FirstName).HasColumnType("varchar(50)");
        builder.Property(b=>b.LastName).HasColumnType("varchar(50)");
        builder.Property(b=>b.UserName).HasColumnType("varchar(100)");
        builder.Property(b=>b.Email).HasColumnType("varchar(MAX)");

            



    }
}
