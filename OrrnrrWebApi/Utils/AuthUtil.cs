
namespace OrrnrrWebApi.Utils
{
    internal static class AuthUtil
    {
        internal static string? GetUserName()
        {
            return Thread.CurrentPrincipal?.Identity?.Name;
        }
    }
}
