using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    public TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string GenerateJWT(Models.User user, DateTime expireDate)
    {
        var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

        var _claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, user.Id.ToString())
        };

        if (user.IsAdmin)
        {
            _claims.Add(new Claim(ClaimTypes.Role, Constants.Role.Admin));
        }
        else
        {
            _claims.Add(new Claim(ClaimTypes.Role, Constants.Role.Customer));
        }


        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            _claims,
            expires: expireDate,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}
