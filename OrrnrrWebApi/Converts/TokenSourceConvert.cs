using DataAccessLib.Models;
using OrrnrrWebApi.Requests;
using OrrnrrWebApi.Responses;

namespace OrrnrrWebApi.Converts
{
    internal static class TokenSourceConvert
    {
        public static TokenSource ToTokenSource(this TokenSourceRequest request)
        {
            return new TokenSource
            {
                Name = request.Name,
                RequestUrl = request.RequestUrl,
            };
        }
    }
}
