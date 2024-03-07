using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using LabSid.Services.DTO;
using static LabSid.Infra.UserRepository;

namespace LabSid.Services.Auth
{
    public static class TokenService
    {
        public static string GenerateToken(string email, string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", id),
                    new Claim("email", email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool IsJwtTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Secret));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                string? id = jwtToken?.Claims?.FirstOrDefault(x => x.Type == "id")?.Value;
                string? email = jwtToken?.Claims?.FirstOrDefault(x => x.Type == "email")?.Value;

                if (id == null || email == null)
                    throw new ArgumentException("Access token sem email e id.");

                return jwtToken?.ValidTo < DateTime.UtcNow;
            }
            catch (SecurityTokenExpiredException)
            {
                return true;
            }
            catch (SecurityTokenException)
            {
                return true;
            }
        }

        public static RefreshTokenDto RefreshToken(string email, int id, string token)
        {
            try
            {
                if (IsJwtTokenExpired(token))
                {
                    return new RefreshTokenDto()
                    {
                        RefreshToken = GenerateToken(email, id.ToString()),
                        Email = email,
                        Id = id
                    };
                }

                return new RefreshTokenDto()
                {
                    RefreshToken = token,
                    Email = email,
                    Id = id
                };
            }
            catch (SecurityTokenExpiredException)
            {
                throw;
            }
        }
    }
}
