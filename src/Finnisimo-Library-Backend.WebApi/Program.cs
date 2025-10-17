using Finnisimo_Library_Backend.Application;
using Finnisimo_Library_Backend.Application.Abstractions.Authentication;
using Finnisimo_Library_Backend.Application.Abstractions.Gateways;
using Finnisimo_Library_Backend.Infrastructure;
using Finnisimo_Library_Backend.Infrastructure.Authentication;
using Finnisimo_Library_Backend.WebApi.Extensions;
using Finnisimo_Library_Backend.WebApi.Gateways;
using Finnisimo_Library_Backend.WebApi.Hubs;
using Finnisimo_Library_Backend.WebApi.OptionSetup.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseConfiguredCors();

app.MapHub<NotificationHub>("/notificationHub");
app.MapControllers();

app.Run();
