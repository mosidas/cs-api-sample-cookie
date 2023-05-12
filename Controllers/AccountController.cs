using plain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using plain.Helpers;

namespace plain.Controllers;


[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly CookieHelper _cookieHelper;

    public AccountController(ILogger<AccountController> logger, CookieHelper cookieHelper)
    {
        _logger = logger;
        _cookieHelper = cookieHelper;
    }

    /// <summary>
    /// POST: api/account/register
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">成功</response>
    /// <response code="400">失敗</response>
    /// <response code="500">失敗</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<RegisterResponse> Register(RegisterRequest request)
    {

        if (request == null)
        {
            var bad = new RegisterResponse();
            _logger.LogError("ERROR: Request body is null.");
            return BadRequest(bad);
        }

        var ok = new RegisterResponse();
        _logger.LogInformation("SUCCESS: Registered.");

        return Ok(ok);
    }

    /// <summary>
    /// POST: api/account/login
    /// </summary>
    /// <param name="request">リクエストボディ</param>
    /// <returns>レスポンス</returns>
    /// <response code="200">成功</response>
    /// <response code="400">失敗</response>
    /// <response code="500">失敗</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request)
    {
        if (request == null)
        {
            return BadRequest(new {message = "Request body is null."});
        }

        // TODO: validate request

        // make claims
        var claims = _cookieHelper.GetClaimsPrincipal(request);

        // make auth properties
        var authProperties = _cookieHelper.GetAuthProperties();

        // sign in
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claims,
            authProperties);

        return Ok(new LoginResponse("success!"));
    }
}