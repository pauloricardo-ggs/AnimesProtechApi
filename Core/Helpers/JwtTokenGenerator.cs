using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Core.Helpers;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public async Task<Jwt> Generate(User user, UserManager<User> userManager)
    {
        var claims = new ClaimsIdentity(
        [
            new(CustomClaimTypes.UserId, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ]);

        var roles = await userManager.GetRolesAsync(user);
        claims.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationEnvironment.GetEnvironmentVariable("JWT_KEY")));
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claims,
            Issuer = ConfigurationEnvironment.GetEnvironmentVariable("JWT_ISSUER"),
            Audience = ConfigurationEnvironment.GetEnvironmentVariable("JWT_AUDIENCE"),
            NotBefore = DateTime.Now,
            Expires = DateTime.Now.AddMinutes(int.Parse(ConfigurationEnvironment.GetEnvironmentVariable("JWT_DURACAO_MINUTOS"))),
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return new Jwt
        {
            Token = tokenHandler.WriteToken(securityToken),
            ExpiresIn = securityToken.ValidTo.ToString()
        };
    }
}