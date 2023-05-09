using plain.Models;
using plain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace plain.Controllers;


[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly JwtHelper _jwtHelper;

    public AccountController(ILogger<AccountController> logger, JwtHelper jwtHelper)
    {
        _logger = logger;
        _jwtHelper = jwtHelper;
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
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        if (request == null)
        {
            return BadRequest(new {message = "Request body is null."});
        }
        // TODO: ユーザー認証
        // throw new ArgumentException("hoge");

        var userId = Guid.NewGuid().ToString();
        var userName = request.Id.ToString();

        var token = _jwtHelper.GenerateJwtToken(userId.ToString(), userName);

        return Ok(new LoginResponse(token));
    }
}