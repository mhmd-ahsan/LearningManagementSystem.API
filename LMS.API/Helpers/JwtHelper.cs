using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.API.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(int userId, string email, string role, IConfiguration configuration)
        {
            // 🔧 Load JWT settings from configuration
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer") ?? "LMS.API";
            var audience = jwtSettings.GetValue<string>("Audience") ?? "LMS.API";


            // ❗ Validate secret key
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new Exception("JWT SecretKey is missing in appsettings.json");

            // 🔐 Create signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🧾 Define claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
            };

            // 🕒 Create JWT token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            // 🔁 Return token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
