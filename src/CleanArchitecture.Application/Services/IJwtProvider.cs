using CleanArchitecture.Domain.Users;

namespace CleanArchitecture.Application.Services;
public interface IJwtProvider
{
    public Task<string> GetTokenAsync(AppUser user, CancellationToken cancellationToken = default);

}
