using BaseProject.Application.Common.ServiceLifeTime;
using Domain.Concrete;
using Microsoft.Extensions.Configuration;

namespace BaseProject.Application.Identity.Tokens;

public interface ITokenService 
{
    TokenInfo CreateToken(User user, JwtSettings jwtSettings);
}