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

//var settingsSection = builder.Configuration.GetSection("WineCellarDatabase");
var settingsSection = configurationBuilder.GetSection("WineCellarDatabase");
//Console.WriteLine(settingsSection.Get<WineCellarDatabaseSettings>().ConnectionString);
//Console.WriteLine(s.Get<WineCellarDatabaseSettings>().ConnectionString);
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors(allowedSpecificOrigins);
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
