using CleanArchitecture.Application.Auth;
using MediatR;
using TS.Result;

namespace CleanArchitecture.WebApi.Modules.Auth;
public static class AuthModule
{


    public static void RegisterAuthRoutes(this IEndpointRouteBuilder app)
    {


        RouteGroupBuilder group = app.MapGroup("/auth").WithTags("Auth");

        group.MapPost("login",
            async (ISender sender, LoginCommand request, CancellationToken ct) =>
            {
                var response = await sender.Send(request, ct);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);

            }).Produces<Result<LoginCommandResponse>>();


    }
}

