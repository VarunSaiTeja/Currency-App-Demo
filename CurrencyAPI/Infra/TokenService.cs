using CurrencyAPI.DAL.Entities;
using CurrencyAPI.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace CurrencyAPI.Infra;

public class TokenService(IOptionsSnapshot<JwtOptions> options)
{
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new("userId", user.Id.ToString())
        };

        if (!string.IsNullOrEmpty(user.Name))
            claims.Add(new("name", user.Name));

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
