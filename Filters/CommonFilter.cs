using Microsoft.AspNetCore.Mvc.Filters;

namespace plain.Filters;

public class CommonFilter : ActionFilterAttribute
{
    private readonly ILogger<CommonFilter> _logger;

    public CommonFilter(ILogger<CommonFilter> logger)
    {
        _logger = logger;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"OnActionExecuting: Path: {context.HttpContext.Request.Path} Method: {context.HttpContext.Request.Method}");
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"OnActionExecuted: Path: {context.HttpContext.Request.Path} Method: {context.HttpContext.Request.Method}");
    }
}