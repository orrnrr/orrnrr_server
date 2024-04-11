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
    public class JwtProvider
    {
        public JwtProvider(string secretKeyStr)
        {
            if (string.IsNullOrEmpty(secretKeyStr))
            {
                throw new ArgumentNullException(nameof(secretKeyStr));
            }

            SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKeyStr));
            TokenHandler = new JwtSecurityTokenHandler();
        }

        public SecurityKey SecurityKey { get; }
        public JwtSecurityTokenHandler TokenHandler { get; }

        public string CreateAccessToken(int userId, UserRoles userRoles, DateTime Expires)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(AuthUtil.USER_ID_TYPE, userId.ToString()),
                new Claim(AuthUtil.ROLES_TYPE, ((int)userRoles).ToString()),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = Expires,
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256),
            };

            var token = TokenHandler.CreateToken(tokenDescriptor);

            return TokenHandler.WriteToken(token);
        }

        internal string CreateSuperUserAccessToken()
        {
            return CreateAccessToken(1, UserRoles.User, DateTime.UtcNow.AddMonths(6));
        }
        internal string CreateSuperManagerAccessToken()
        {
            return CreateAccessToken(2, UserRoles.Manager, DateTime.UtcNow.AddMonths(6));
        }
        internal string CreateSuperDeveloperAccessToken()
        {
            return CreateAccessToken(3, UserRoles.Developer, DateTime.UtcNow.AddMonths(6));
        }

        internal ClaimsPrincipal GetPrincipalFromAccessToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            };

            ClaimsPrincipal principal;
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
