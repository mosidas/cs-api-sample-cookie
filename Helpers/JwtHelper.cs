using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace plain.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;
    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// JWTトークンを生成する。
    /// </summary>
    /// <param name="userId">id(guid)</param>
    /// <param name="userName">name(unique)</param>
    /// <returns></returns>
    public string GenerateJwtToken(string userId, string userName)
    {
        var secretKey = _configuration.GetSection("Jwt:SecretKey").Value??throw new ArgumentNullException("JWT secretKey is null.");
        var issuer = _configuration.GetSection("Jwt:Issuer").Value??throw new ArgumentNullException("JWT issuer is null.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: null,// TODO: 
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
