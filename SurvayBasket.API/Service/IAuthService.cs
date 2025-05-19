using SurvayBasket.API.Abstractions;
using SurvayBasket.API.Contracts.Authentication;

namespace SurvayBasket.API.Service;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password,CancellationToken cancellationToken=default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

}
