using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Infrastructure.Context;
using CleanArchitecture.Infrastructure.Repositories;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CleanArchitecture.Infrastructure
{
    public static class InfrastructureRegistrar
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services , IConfiguration configuration)
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


            //services.Scan(opt => opt
            //    .FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
            //    .AddClasses(publicOnly: false)
            //    .UsingRegistrationStrategy(Scrutor.RegistrationStrategy.Skip)  // implemente olanlari tekrarlama
            //    .AsImplementedInterfaces()
            //    .WithSingletonLifetime()
            //);

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
}
