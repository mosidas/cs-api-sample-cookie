# .NET 6.0 - JWT Authentication Tutorial with Example API

このチュートリアルでは、C#で.NET 6.0 APIにカスタムJWT（JSON Web Token）認証を実装する方法を簡単に説明します。

(略)

## .NET Custom Authorize Attribute

- カスタムのauthorize属性は、ユーザーの認証が必要なコントローラのアクションメソッドに追加されます。
- 認証はOnAuthorizationメソッドで行われ、現在のリクエストに認証済みユーザーがアタッチされているかどうかをチェックします（context.HttpContext.Items["User"])。
- リクエストに有効なJWTアクセストークンが含まれていれば、カスタムjwtミドルウェアによって認証されたユーザーがアタッチされます。
- 認証に成功した場合、アクションは実行されず、リクエストはコントローラのアクションメソッドに渡されます。
- 認証に失敗した場合、401 Unauthorizedレスポンスが返されます。

AuthorizeAttribute.cs
```csharp
namespace WebApi.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Entities;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = (User)context.HttpContext.Items["User"];
        if (user == null)
        {
            // not logged in
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}
```

## .NET Custom JWT Middleware

- カスタムJWTミドルウェアは、リクエストのAuthorizationヘッダーにトークンがあるかどうかをチェックし、ある場合は以下を実行する:
    1. トークンの有効性確認
    2. トークンからユーザーIDを抽出する
    3. 認証されたユーザーを現在の HttpContext.Items コレクションにアタッチし、現在のリクエストの範囲内でアクセスできるようにする
- リクエストヘッダにトークンがない場合、または上記の手順のいずれかに失敗した場合、httpコンテキストにユーザーがアタッチされていないため、リクエストはパブリックルートにのみアクセスすることができます。
- 認証はカスタムのauthorize属性によって行われ、ユーザーがhttpコンテキストにアタッチされているかどうかをチェックします。認証に失敗すると、401 Unauthorizedレスポンスが返されます。

```csharp
namespace WebApi.Helpers;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            attachUserToContext(context, userService, token);

        await _next(context);
    }

    private void attachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            // attach user to context on successful jwt validation
            context.Items["User"] = userService.GetById(userId);
        }
        catch
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}
```