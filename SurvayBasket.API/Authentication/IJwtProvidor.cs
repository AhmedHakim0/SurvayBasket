using Microsoft.Extensions.Options;

namespace SurvayBasket.API.Authentication;

public interface IJwtProvidor
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
    string? ValidateToken(string token);
}
