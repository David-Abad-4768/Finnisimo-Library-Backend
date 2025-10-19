using Azure.Identity;
using Finnisimo_Library_Backend.Application;
using Finnisimo_Library_Backend.Application.Abstractions.Authentication;
using Finnisimo_Library_Backend.Application.Abstractions.Gateways;
using Finnisimo_Library_Backend.Infrastructure;
using Finnisimo_Library_Backend.Infrastructure.Authentication;
using Finnisimo_Library_Backend.Infrastructure.Persistence;
using Finnisimo_Library_Backend.WebApi.Extensions;
using Finnisimo_Library_Backend.WebApi.Gateways;
using Finnisimo_Library_Backend.WebApi.Hubs;
using Finnisimo_Library_Backend.WebApi.OptionSetup.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
  string? keyVaultName = builder.Configuration["AzureKeyVaultName"];

  if (string.IsNullOrEmpty(keyVaultName))
  {
    throw new InvalidOperationException(
        "AzureKeyVaultName no está configurado en appsettings.json.");
  }

  var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

  builder.Configuration.AddAzureKeyVault(keyVaultUri,
                                         new DefaultAzureCredential());
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddTransient<IJwtProvider, JwtProvider>();

builder.Services.AddConfiguredCors(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddSignalR();

builder.Services.AddScoped<INotificationGateway, SignalRNotificationGateway>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var dbContext =
      scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
  dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference(opt =>
  {
    opt.Title = "Finnisimo Apps";
    opt.DarkMode = true;
    opt.Theme = ScalarTheme.BluePlanet;
    opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
  });
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseConfiguredCors();

app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();

app.Run();
