using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAssignment.Models;

namespace MyAssignment.Services
{
    /// <summary>
    /// Generates signed JWT tokens for authenticated Identity users, reading
    /// signing parameters from configuration (appsettings.json "Jwt" section).
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        /// <summary>
        /// Builds a JWT containing the user's id, email, a unique token
        /// identifier (jti), and one claim per assigned role.
        /// </summary>
        public string GenerateToken(IdentityUser user, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            string key = _jwtSettings.Key;
            string issuer = _jwtSettings.Issuer;
            string audience = _jwtSettings.Audience;
            int expiryMinutes = _jwtSettings.ExpiryMinutes;

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}