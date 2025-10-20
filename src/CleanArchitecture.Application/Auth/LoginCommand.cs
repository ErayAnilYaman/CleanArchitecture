using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace CleanArchitecture.Application.Auth;
public sealed record LoginCommand
    (
    string UserNameOrEmail,
    string Password
    ) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;



}

internal sealed class LoginCommandHandler(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager 
    , IJwtProvider jwtprovider) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == request.UserNameOrEmail || u.UserName == request.UserNameOrEmail);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanici Bulunamadi!");


        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
            if (timeSpan is not null)
                return (500, $"Şifrenizi 5 defa yanlış girdiğiniz için kullanıcı {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika süreyle bloke edilmiştir");
            else
                return (500, "Kullanıcınız 5 kez yanlış şifre girdiği için 5 dakika süreyle bloke edilmiştir");
        }

        if (signInResult.IsNotAllowed)
        {
            return (500, "Mail adresiniz onaylı değil , lutfen mail adresinizi onaylayiniz");
        }

        if (!signInResult.Succeeded)
        {
            return (500, "Şifreniz yanlış");
        }

        var jwtToken = await jwtprovider.GetTokenAsync(user, cancellationToken); 


        var response = new LoginCommandResponse { AccessToken = jwtToken };

        return response;


    }
}
