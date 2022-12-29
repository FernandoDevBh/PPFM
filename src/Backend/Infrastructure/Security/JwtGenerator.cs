using System.Text;
using Domain.Identity;
using Application.Interfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security;

internal class JwtGenerator : IJwtGenerator
{
    private readonly SymmetricSecurityKey key;

    public JwtGenerator(IConfiguration config)
    {
        this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"] ?? string.Empty));
    }

    public string CreateToken(ApplicationUser user)
    {
        var userName = user.UserName ?? string.Empty;
        var claim = new List<Claim>
        { 
            new Claim(JwtRegisteredClaimNames.NameId, userName)
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claim),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
