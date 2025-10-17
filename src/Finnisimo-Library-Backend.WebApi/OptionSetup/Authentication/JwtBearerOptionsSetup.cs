using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Finnisimo_Library_Backend.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Finnisimo_Library_Backend.WebApi.OptionSetup.Authentication;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
  private readonly JwtOptions _jwtOptions;

  public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
  {
    _jwtOptions = jwtOptions.Value;

    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
  }

  public void Configure(string? name,
                        JwtBearerOptions options) => ConfigureToken(options);

  public void Configure(JwtBearerOptions options) => ConfigureToken(options);

  private void ConfigureToken(JwtBearerOptions options)
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!)),
      ValidateIssuer = true,
      ValidIssuer = _jwtOptions.Issuer,
      ValidateAudience = true,
      ValidAudience = _jwtOptions.Audience,
      ValidateLifetime = true,
      NameClaimType = JwtRegisteredClaimNames.Sub
    };
    options.Events = new JwtBearerEvents
    {
      OnMessageReceived = ctx =>
      {
        if (ctx.Request.Cookies.TryGetValue("access_token", out var token))
          ctx.Token = token;
        return Task.CompletedTask;
      }
    };
  }
}
