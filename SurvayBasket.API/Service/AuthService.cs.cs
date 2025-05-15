using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using SurvayBasket.API.Authentication;
using SurvayBasket.API.Contracts.Authentication;

namespace SurvayBasket.API.Service;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvidor jwtProvidor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvidor _jwtProvidor = jwtProvidor;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!IsValidPassword)
            return null;

        var (token, expiresIn) = _jwtProvidor.GenerateToken(user);
        var RefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = RefreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);
        return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, token, expiresIn, RefreshToken, refreshTokenExpiration);
    }



    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvidor.ValidateToken(token);

        if (userId is null)
            return null;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return null;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive );

        if(userRefreshToken is null)
            return null; 

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn) = _jwtProvidor.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);

        return new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
