using Finnisimo_Library_Backend.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Finnisimo_Library_Backend.WebApi.OptionSetup.Authentication;

public class JwtOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<JwtOptions>
{
  private const string SectionName = "Jwt";
  private readonly IConfiguration _configuration = configuration;

  public void Configure(JwtOptions options)
  {
    _configuration.GetSection(SectionName).Bind(options);
  }
}
