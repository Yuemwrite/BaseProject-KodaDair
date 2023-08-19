using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BaseProject.Application.Identity.Tokens;
using Domain.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BaseProject.Infrastructure.Identity;

public class TokenService : ITokenService
{
    public TokenInfo CreateToken(User user, JwtSettings jwtSettings)
    {
        TokenInfo token = new();
        
        
        
        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        token.ExpirationTime = DateTime.Now.AddMinutes(Convert.ToInt16(jwtSettings.TokenExpirationInMinutes));

        JwtSecurityToken jwtSecurityToken = new(
            issuer: jwtSettings.ValidIssuer,
            audience: jwtSettings.ValidAudience,
            GetClaims(user),
            expires: token.ExpirationTime,
            notBefore: DateTime.Now,
            signingCredentials: credentials);

        JwtSecurityTokenHandler tokenHandler = new();

        token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);

        byte[] numbers = new byte[32];
        using RandomNumberGenerator random = RandomNumberGenerator.Create();
        random.GetBytes(numbers);
        token.RefreshToken = Convert.ToBase64String(numbers);


        return token;
    }
    
    private IEnumerable<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

        };
            
        return claims;
    }
}