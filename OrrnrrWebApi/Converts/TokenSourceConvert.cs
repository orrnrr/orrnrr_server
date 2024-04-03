using DataAccessLib.Models;
using OrrnrrWebApi.Requests;
using OrrnrrWebApi.Responses;

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

        internal static TokenSourceResponse ToTokenSourceResponse(this TokenSource tokenSource)
        {
            return new TokenSourceResponse
            {
                Id = tokenSource.Id,
                Name = tokenSource.Name
            };
        }
    }
}
