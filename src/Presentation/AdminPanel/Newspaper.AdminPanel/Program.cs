using Microsoft.AspNetCore.Authentication.Cookies;
using Maggsoft.Framework.Middleware;
using Maggsoft.Framework.HttpClientApi;
using Newspaper.AdminPanel.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add Authentication (Cookie Authentication)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        opt.LoginPath = "/Users/Login";
        opt.AccessDeniedPath = "/Users/Login";
        opt.ExpireTimeSpan = TimeSpan.FromDays(1); // Set your desired expiration time
        opt.Cookie.HttpOnly = true;
        opt.Cookie.IsEssential = true;
        opt.SlidingExpiration = true;
        opt.Events.OnSignedIn = context =>
        {
            //@TODO:buras� incelenecek
            //https://stackoverflow.com/questions/54202190/get-authenticationproperties-in-current-httprequest-after-httpcontext-signinasyn
            var httpContext = context.HttpContext;
            httpContext.Items["Properties"] = context.Properties;
            httpContext.Features.Set(context.Properties);
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

// HTTP Client for API calls
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IMaggsoftHttpClient, CustomHttpClient>();

// Memory Cache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
