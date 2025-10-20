using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Context;
using CleanArchitecture.Infrastructure.Options;
using CleanArchitecture.Infrastructure.Repositories;
using GenericRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Formats.Tar;

namespace CleanArchitecture.Infrastructure;
public static class InfrastructureRegistrar
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            opt =>
            {
                string connectionString = configuration.GetConnectionString("SqlServer")!;
                opt.UseSqlServer(connectionString);
            });

        // services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        // burda her bir scope icin yazmak yerine scan yaptiricak ve tek tek yazilmicak bir kutuphane cagirdim.
        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services
            .AddIdentity<AppUser , IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredUniqueChars = 0;

                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                opt.SignIn.RequireConfirmedEmail = true;

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();   // User Manager kullanmak icin yaziyoruz..

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtOptionsSetup>();

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddAuthorization();


        services.Scan(scan => scan
.FromAssemblies(typeof(InfrastructureRegistrar).Assembly)

// Sadece Repositories klasörü → SCOPED
.AddClasses(c => c.InNamespaces("CleanArchitecture.Infrastructure.Repositories"))
    .AsImplementedInterfaces()
    .WithScopedLifetime()

// Opsiyonel: Services klasörü → TRANSIENT
.AddClasses(c => c.InNamespaces("CleanArchitecture.Infrastructure.Services"))
    .AsImplementedInterfaces()
    .WithTransientLifetime()
);

        services.TryAddScoped<IEmployeeRepository, EmployeeRepository>();


        return services;
    }
}

