

using Microsoft.AspNetCore.Http;
using OrrnrrWebApi.Authorization;
using System.Security.Claims;

namespace OrrnrrWebApi.Utils
{
    internal static class AuthUtil
    {
        internal const string USER_ID_TYPE = "USER_ID_TYPE";

        internal const string ROLES_TYPE = "ROLES_TYPE";

        internal static int GetUserId(ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == USER_ID_TYPE).Select(x => int.Parse(x.Value)).FirstOrDefault();
        }

        internal static UserRoles GetUserRoles(ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == ROLES_TYPE).Select(x => (UserRoles)int.Parse(x.Value)).FirstOrDefault();
        }
    }
}
