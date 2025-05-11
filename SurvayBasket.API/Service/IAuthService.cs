using SurvayBasket.API.Contracts.Authentication;

namespace SurvayBasket.API.Service;

public interface IAuthService
{
    Task<AuthResponse?> GetTokenAsync(string email, string password,CancellationToken cancellationToken=default);
}
