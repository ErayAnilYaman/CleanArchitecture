using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CleanArchitecture.Infrastructure.Services;
public class JwtProvider(IOptions<JwtOptions> opt) : IJwtProvider
{
    public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        var expires = DateTime.Now.AddDays(1);
        List<Claim> claims = new()
        {
            new Claim("user-id" ,user.Id.ToString())
        };

        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(opt.Value.SecretKey));

        SigningCredentials credentials = new(symmetricSecurityKey , SecurityAlgorithms.HmacSha512);




        JwtSecurityToken securityToken = new
            (
            issuer : opt.Value.Issuer,
            audience : opt.Value.Audience,
            claims: claims,
            notBefore : DateTime.Now,
            expires:  expires,
            signingCredentials : credentials
            );
        JwtSecurityTokenHandler handler = new();

        string token = handler.WriteToken(securityToken);  

        return Task.FromResult(token);

    }
}

