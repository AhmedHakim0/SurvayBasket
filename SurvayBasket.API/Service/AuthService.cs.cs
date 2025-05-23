﻿using System.Linq.Expressions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using SurvayBasket.API.Abstractions;
using SurvayBasket.API.Authentication;
using SurvayBasket.API.Contracts.Authentication;
using SurvayBasket.API.Errors;

namespace SurvayBasket.API.Service;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvidor jwtProvidor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvidor _jwtProvidor = jwtProvidor;
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
           return  Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!IsValidPassword)
           return  Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, expiresIn) = _jwtProvidor.GenerateToken(user);
        var RefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = RefreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        await _userManager.UpdateAsync(user);

        var Response= new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName,  token, expiresIn, RefreshToken, refreshTokenExpiration); 
        return Result.Success(Response); 
    }



    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvidor.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
           Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive );

        if(userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

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

        var result= new AuthResponse(user.Id, user.FirstName, user.LastName, user.Email!, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
        
        return Result.Success(result);

    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
