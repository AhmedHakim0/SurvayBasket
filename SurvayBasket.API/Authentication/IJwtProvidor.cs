namespace SurvayBasket.API.Authentication;

public interface IJwtProvidor
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
}
