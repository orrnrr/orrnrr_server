using DataAccessLib.Models;
using OrrnrrWebApi.Pagination;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Sort;

namespace OrrnrrWebApi.Services
{
    public interface ITokenService
    {
        PaginationResponse<TokenResponse> GetTokensOrderByChangeRate(Paging paging, Ordering ordering);
        PaginationResponse<TokenResponse> GetTokensOrderByCurrentPrice(Paging paging, Ordering ordering);
        PaginationResponse<TokenResponse> GetTokensOrderByTradeAmount(Paging paging, Ordering ordering);
    }
}
