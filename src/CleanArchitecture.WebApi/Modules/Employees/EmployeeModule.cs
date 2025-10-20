using MediatR;
using TS.Result;
namespace CleanArchitecture.WebApi.Modules.Employees;
public static class EmployeeModule
{


    public static void RegisterEmployeeRoutes(this IEndpointRouteBuilder app)
    {


        RouteGroupBuilder group = app.MapGroup("/employees").WithTags("Employees").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, EmployeeCreateCommand request, CancellationToken ct) =>
            {
                var response = await sender.Send(request, ct);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

            }).Produces<Result<string>>();


    }
}
