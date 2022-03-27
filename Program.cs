using System.Text;
using api_design_assignment.Models;
using api_design_assignment.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("Properties/appsettings.json")
    .AddJsonFile($"Properties/appsettings.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables()
    .Build();

var services = builder.Services;

var settingsSection = configurationBuilder.GetSection("WineCellarDatabase");
var connect = settingsSection.Get<WineCellarDatabaseSettings>().ConnectionString;

services.Configure<WineCellarDatabaseSettings>(settingsSection);

var settings = settingsSection.Get<WineCellarDatabaseSettings>();

var key = Encoding.ASCII.GetBytes(settings.JwtKey);

services.AddSingleton<WinesService>();
services.AddSingleton<UserService>();
services.AddSingleton<WebhookService>();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;

        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

    });

services.AddControllers()
  .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "Welcome to this winecellar api!");
app.Run();
