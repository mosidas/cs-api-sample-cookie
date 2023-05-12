using plain.Filters;
using plain.Services;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using plain.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSingleton<CookieHelper>();

// TODO: Singleton --> Scoped
builder.Services.AddSingleton<IIssueRepository, IssueRepository>();

// cookie
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "sampledscvkvmvmeopk";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //options.Cookie.IsEssential = true;
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.Events.OnRedirectToLogin = cxt =>
        {
            // リダイレクトせずにエラーを返す
            cxt.Response.StatusCode = StatusCodes.Status401Unauthorized;
            cxt.Response.Headers.Append("Content-Type","application/json; charset=utf-8");
            cxt.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("""
            {
                "status":"401",
                "message":"Unauthorized!!!"
            }
            """));
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = cxt =>
        {
            // リダイレクトせずにエラーを返す
            cxt.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToLogout = cxt => Task.CompletedTask;
    });


builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(CommonFilter));
});

// swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "sample", Version = "v1" });
    c.EnableAnnotations();
    // Set the XML comments file path for Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// ロギングの設定
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddHostedService<RegularIssuance>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
else
{
    //app.UseExceptionHandler("/error");
    app.UseExceptionHandler("/error-development");
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action?}/{id?}");

app.MapFallbackToFile("index.html");

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "plain v1");
});
app.Run();
