using plain.Models;
using plain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace plain.Controllers;


[ApiController]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error() => Problem();
}