using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAssignment.Options;

namespace MyAssignment.Services.Auth
{
    /// <summary>
    /// Generates signed JWT tokens for authenticated Identity users using the
    /// configured JWT settings.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SigningCredentials _credentials;

        private static readonly JwtSecurityTokenHandler TokenHandler = new();

        public JwtTokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;

            SymmetricSecurityKey signingKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            _credentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Builds a JWT containing the user's identifier, email, unique token
        /// identifier (JTI), and assigned role claims.
        /// </summary>
        public string GenerateToken(Models.User user, IList<string> roles)
        {
            List<Claim> claims = new()
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            JwtSecurityToken token = new(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: _credentials);

            return TokenHandler.WriteToken(token);
        }
    }
}
