using CleanArchitecture.WebApi.Modules.Employees;

namespace CleanArchitecture.WebApi.Modules
{
    public static class RouteRegistrar
    {

        public static void RegisterRoutes(this IEndpointRouteBuilder builder)
        {
            builder.RegisterEmployeeRoutes();
        }
    }
}

