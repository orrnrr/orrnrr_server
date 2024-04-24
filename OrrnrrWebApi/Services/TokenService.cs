using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OrrnrrWebApi.Pagination;
using OrrnrrWebApi.Responses;
using OrrnrrWebApi.Sort;
using System.Diagnostics.CodeAnalysis;

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
                        join trade in OrrnrrContext.TransactionHistories.Include(x => x.BuyOrder) on token.Id equals trade.BuyOrder.TokenId into orderGroup
                        select new TokenResponse
                        {
                            Id = token.Id,
                            Name = token.Name,
                            Description = token.Description,
                            TokenSourceId = token.TokenSourceId,
                            TokenSourceName = tokenSource.Name,
                            CurrentPrice = orderGroup.OrderByDescending(x => x.TransactionDateTime).Select(x => x.SignedPrice).DefaultIfEmpty().FirstOrDefault(0),
                            PreviousPrice = orderGroup.Where(x => x.TransactionDateTime < today).OrderByDescending(x => x.TransactionDateTime).Select(x => x.SignedPrice).DefaultIfEmpty().FirstOrDefault(0),
                            TradeAmount = orderGroup.Where(x => x.TransactionDateTime >= today).DefaultIfEmpty().Sum(x => x != null ? x.SignedPrice * x.TransactionCount : 0)
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

        public Token CreateToken(int tokenSourceId, string name, string description)
        {
            var tokenSource = OrrnrrContext.TokenSources
                .Where(x => x.Id == tokenSourceId)
                .FirstOrDefault() ?? throw new ArgumentOutOfRangeException(nameof(tokenSourceId), tokenSourceId, $"존재하지 않는 토큰소스 아이디입니다.");

            var existsToken = OrrnrrContext.Tokens.Where(x => x.Name == name).FirstOrDefault();
            if (existsToken is not null)
            {
                throw new InvalidOperationException("이미 같은 이름의 토큰이 존재하여 토큰을 생성하지 못했습니다.");
            }

            var createdToken = new Token
            {
                Description = description,
                Name = name,
                TokenSource = tokenSource,
            };

            OrrnrrContext.Tokens.Add(createdToken);
            OrrnrrContext.SaveChanges();

            return createdToken;
        }

        public bool IsExistsTokenName(string name)
        {
            return OrrnrrContext.Tokens.Any(x => x.Name == name);
        }

        public Token? GetTokenById(int tokenId)
        {
            return OrrnrrContext.Tokens
                .Where(x => x.Id == tokenId)
                .FirstOrDefault();
        }
    }
}
