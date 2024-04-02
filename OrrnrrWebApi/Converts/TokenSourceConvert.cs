using DataAccessLib.Models;
using OrrnrrWebApi.Requests;

namespace OrrnrrWebApi.Converts
{
    internal static class TokenSourceConvert
    {
        internal static TokenSource ToTokenSource(this ITokenSourceRequest request)
        {
            return new TokenSource
            {
                RequestUrl = request.RequestUrl,
                Name = request.Name
            };
        }
    }
}
