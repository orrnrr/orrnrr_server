using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace OrrnrrWebApi.Authorization
{
    internal class JwtProvider
    {
        private JwtProvider() { }

        private static JwtProvider? _instance;
        public static JwtProvider Instance { get => _instance ?? throw new InvalidOperationException($"{nameof(JwtProvider)}의 인스턴스가 초기화되지 않았습니다."); }
        internal required SecurityKey SecurityKey { get; init; }
        internal required JwtSecurityTokenHandler TokenHandler { get; init; }

        internal static void CreateInstance(string secretKey)
        {
            if (_instance is not null)
            {
                return;
            }

            _instance = new JwtProvider
            {
                SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                TokenHandler = new JwtSecurityTokenHandler(),
            };
        }

        internal string CreateAccessToken(int userId, UserRoles userRoles)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("UserId", userId.ToString()),
                new Claim("UserRoles", ((int)userRoles).ToString()),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256),
            };

            var token = TokenHandler.CreateToken(tokenDescriptor);

            return TokenHandler.WriteToken(token);
        }

        internal IPrincipal GetPrincipalFromAccessToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            };

            IPrincipal principal;
            SecurityToken securityToken;

            try
            {
                principal = TokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new AuthorizationApiExeption(System.Net.HttpStatusCode.Unauthorized, "토큰이 만료되었습니다.", "EXPIRED_TOKEN");
            }
            catch (Exception e)
            {
                throw new AuthorizationApiExeption(System.Net.HttpStatusCode.Unauthorized, e.Message);
            }

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new AuthorizationApiExeption(System.Net.HttpStatusCode.Unauthorized, "액세스 토큰이 유효하지 않습니다.");
            }

            return principal;
        }
    }
}
