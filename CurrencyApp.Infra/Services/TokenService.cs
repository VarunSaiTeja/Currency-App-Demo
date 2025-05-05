using CurrencyApp.Application.Services;
using CurrencyApp.Data.Entities;
using CurrencyApp.Infra.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace CurrencyApp.Infra.Services;

public class TokenService(IOptionsSnapshot<JwtOptions> options) : ITokenService
{
    public virtual string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new("userId", user.Id.ToString()),
            new(ClaimTypes.Name, user.Id.ToString())
        };

        foreach (var claim in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, claim.ToString()));

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Secret));

        var securityToken = new JwtSecurityToken(
            issuer: options.Value.Issuer,
            expires: DateTime.UtcNow.AddMinutes(options.Value.AccessExpiryInMins.Value),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
