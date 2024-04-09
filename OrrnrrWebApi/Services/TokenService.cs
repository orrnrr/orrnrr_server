using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore.Internal;
using OrrnrrWebApi.Pagination;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Sort;

namespace OrrnrrWebApi.Services
{
    public class TokenService : ITokenService
    {
        private OrrnrrContext OrrnrrContext { get; }

        public TokenService(OrrnrrContext context)
        {
            OrrnrrContext = context;
        }

        public PaginationResponse<TokenResponse> GetTokensOrderByChangeRate(Paging paging, Ordering ordering)
        {
            throw new NotImplementedException();
        }

        public PaginationResponse<TokenResponse> GetTokensOrderByCurrentPrice(Paging paging, Ordering ordering)
        {
            throw new NotImplementedException();
        }

        public PaginationResponse<TokenResponse> GetTokensOrderByTradeAmount(Paging paging, Ordering ordering)
        {
            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day);

            var query = from token in OrrnrrContext.Tokens
                        join tokenSource in OrrnrrContext.TokenSources on token.TokenSourceId equals tokenSource.Id
                        join order in OrrnrrContext.TokenOrderHistories.Where(x => x.CompleteCount > 0) on token.Id equals order.TokenId into orderGroup
                        select new TokenResponse
                        {
                            Id = token.Id,
                            Name = token.Name,
                            Description = token.Description,
                            TokenSourceId = token.TokenSourceId,
                            TokenSourceName = tokenSource.Name,
                            CurrentPrice = orderGroup.OrderByDescending(x => x.OrderDateTime).Select(x => x.OrderPrice).DefaultIfEmpty().FirstOrDefault(0),
                            PreviousPrice = orderGroup.Where(x => x.OrderDateTime < today).OrderByDescending(x => x.OrderDateTime).Select(x => x.OrderPrice).DefaultIfEmpty().FirstOrDefault(0),
                            TradeAmount = orderGroup.Where(x => x.OrderDateTime >= today).DefaultIfEmpty().Sum(x => x != null ? x.OrderPrice * x.CompleteCount : 0)
                        };

            query = ordering.OrderDirection == OrderDirectionType.Descending ?
                query.OrderByDescending(x => x.TradeAmount) :
                query.OrderBy(x => x.TradeAmount);

            var responses = query.ToArray();
            int total = responses.Length;

            var data = responses
                .Skip(paging.Skip)
                .Take(paging.Size)
                .ToArray();

            return PaginationResponse<TokenResponse>.Of(paging, data, total);
        }
    }
}
