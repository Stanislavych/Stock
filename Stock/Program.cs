using Stock.Models;
using Microsoft.EntityFrameworkCore;
using Stock.BusinessLogic.Services;
using Stock.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Stock.Common.Dto;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(hostingContext.Configuration)
    .Enrich.FromLogContext()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.File(Path.Combine("logs", "log.txt"), rollingInterval: RollingInterval.Day).Filter.ByExcluding(e => e.Level == LogEventLevel.Error &&
        e.Properties.ContainsKey("Category") &&
        e.Properties["Category"].ToString().Contains("DataProtection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
    .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

//создание первоначального админа
//var authService = app.Services.CreateScope().ServiceProvider.GetRequiredService<IAuthService>();
//var admin = new UserDto { Username = "admin", Password = "admin" };
//await authService.RegisterAdminAsync(admin);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.Use(async (context, next) =>
{
    var jwt = context.Request.Cookies["jwt"];
    if (!string.IsNullOrEmpty(jwt))
    {
        context.Request.Headers.Add("Authorization", $"Bearer {jwt}");
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Item}/{action=Index}/{id?}");

app.Run();
