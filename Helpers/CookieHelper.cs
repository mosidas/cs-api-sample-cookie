using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using plain.Models;

namespace plain.Helpers;


public class CookieHelper
{
    private readonly IConfiguration _configuration;
    public CookieHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// GetAuthProperties
    /// </summary>
    /// <returns></returns>
    public AuthenticationProperties GetAuthProperties()
    {
        return new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
        };
    }

    /// <summary>
    /// GetClaimsPrincipal
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public ClaimsPrincipal GetClaimsPrincipal(LoginRequest request)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, request.Id.ToString()),
            new Claim(ClaimTypes.Name, $"name-{request.Id.ToString()}"),
            new Claim(ClaimTypes.Email, $"email-{request.Id.ToString()}@example.com"),
            new Claim("custom-claim", $"custom-claim-{request.Id.ToString()}"),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        return principal;
    }
}