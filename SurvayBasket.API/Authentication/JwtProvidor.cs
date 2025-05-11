using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SurvayBasket.API.Authentication;

public class JwtProvidor : IJwtProvidor
{
    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("$|%XF^sN)0c-?X}@eU>iLsr|Ju)7Pc.}Qk+cb:7p0Sy3r7W0nv:~r]i23jf2@/P"));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var ExpiresIn = 30 * 60;

        var token =new JwtSecurityToken(
            issuer: "SurvayBasket",
            audience: "SurvayBasket users",
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(ExpiresIn),
            signingCredentials: signingCredentials
        );

        return (
            token: new JwtSecurityTokenHandler().WriteToken(token),
            expiresIn: ExpiresIn
        );
    }
}
