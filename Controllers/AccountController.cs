using Microsoft.AspNetCore.Mvc;
using plain.Models;

namespace plain.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    /// POST: api/Account/Login
    [HttpPost("Login")]
    public ActionResult<LoginResponse> Login(LoginRequest request)
    {
        System.Diagnostics.Debug.WriteLine("Login request received");
        if (request == null)
        {
            return BadRequest(new LoginResponse("ERROR", "Invalid request"));
        }

        return Ok(new LoginResponse("OK", "Login successful"));
    }
}
