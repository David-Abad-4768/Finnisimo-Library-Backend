using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Finnisimo_Library_Backend.Application.Abstractions.Authentication;
using Finnisimo_Library_Backend.Domain.Entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Finnisimo_Library_Backend.Infrastructure.Authentication;

public sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
  private readonly JwtOptions _options = options.Value;

  public Task<string> Generate(UserEntity user)
  {
    var claims =
        new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()) };
    var creds = new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey!)),
        SecurityAlgorithms.HmacSha256);
    var jwt = new JwtSecurityToken(
        issuer: _options.Issuer, audience: _options.Audience, claims: claims,
        expires: DateTime.UtcNow.AddDays(1), signingCredentials: creds);
    return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
  }
}
