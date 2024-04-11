using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Utils;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;

namespace OrrnrrWebApi.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class RequireAccessTokenAttribute : Attribute, IAuthorizationFilter
    {
        private UserRoles AllowedRoles { get; }
        private JwtProvider Provider { get; } = AuthProviderManager.CreateJwtProvider();

        internal RequireAccessTokenAttribute(UserRoles allowedRoles)
        {
            AllowedRoles = allowedRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var accessToken = context.HttpContext.Request.Headers["AccessToken"]
                .Where(x => x?.StartsWith("Bearer ") ?? false)
                .Select(x => x?["Bearer ".Length..])
                .FirstOrDefault();

            if (accessToken is null) {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new ObjectResult(new ApiFailureResult { Message = "액세스 토큰이 누락되었습니다."});
                return;
            }

            ClaimsPrincipal principal;
            try
            {
                principal = Provider.GetPrincipalFromAccessToken(accessToken);
            } catch (AuthorizationApiExeption e)
            {
                context.HttpContext.Response.StatusCode = (int)e.StatusCode;
                context.Result = new ObjectResult(e.GetApiFailureResult());
                return;
            }

            UserRoles matchedRoles = AllowedRoles & AuthUtil.GetUserRoles(principal);
            if (matchedRoles == UserRoles.None)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Result = new ObjectResult(new ApiFailureResult { Message = "접근 권한이 없습니다." });
                return;
            }

            context.HttpContext.User = principal;
        }
    }
}
