using Microsoft.AspNetCore.Authentication.BearerToken;
using OrrnrrWebApi.Parameters;
using OrrnrrWebApi.Responses;

namespace OrrnrrWebApi.Services
{
    public interface ITokenService
    {
        IEnumerable<TokenResponse> GetTokens(PagenationParameter pagenationParamter);
    }
}
