using Microsoft.AspNetCore.Identity;
using SurvayBasket.API.Authentication;
using SurvayBasket.API.Contracts.Authentication;

namespace SurvayBasket.API.Service;

public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvidor jwtProvidor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvidor _jwtProvidor = jwtProvidor;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if(!IsValidPassword)
            return null;

       var (token, expiresIn) = _jwtProvidor.GenerateToken(user);

        return new AuthResponse(user.Id,user.FirstName,user.LastName,user.Email!,token,expiresIn);
    }
}
