using plain.Models;
using plain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace plain.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly JwtHelper _jwtHelper;

    public AccountController(ILogger<AccountController> logger, JwtHelper jwtHelper)
    {
        _logger = logger;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    /// <summary>
    /// POST: api/account/register
    /// </summary>
    /// <param name="request">リクエストボディ</param>
    /// <returns>レスポンス</returns>
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
    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        if (request == null)
        {
            return BadRequest(new {message = "Request body is null."});
        }
        // TODO: ユーザー認証
        throw new ArgumentException("hoge");

        // var userId = Guid.NewGuid().ToString();
        // var userName = request.Id.ToString();

        // var token = _jwtHelper.GenerateJwtToken(userId.ToString(), userName);

        // return Ok(new LoginResponse(token));
    }
}