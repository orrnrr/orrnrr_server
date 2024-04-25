using DataAccessLib.Models;
using DataAccessLib.Types;
using Microsoft.EntityFrameworkCore;
using OrrnrrWebApi.Exceptions;
using OrrnrrWebApi.Repositories;

namespace OrrnrrWebApi.Services
{
    internal class OrdersService : IOrdersService
    {
        private OrrnrrContext OrrnrrContext { get; }
        private ITransactionService TransactionService { get; }
        public OrdersService(OrrnrrContext context, ITransactionService transactionService)
        {
            OrrnrrContext = context;
            TransactionService = transactionService;
        }

        public TokenOrderHistory CreateLimitOrder(int userId, int tokenId, bool isBuyOrder, int price, int count)
        {
            return isBuyOrder switch
            {
                true => CreateLimitBuyOrder(userId, tokenId, price, count),
                false => CreateLimitSellOrder(userId, tokenId, price, count),
            };
        }

        private TokenOrderHistory CreateLimitSellOrder(int userId, int tokenId, int price, int count)
        {
            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            User user = OrrnrrContext.Users.GetUserById(userId) ?? throw new UnauthorizedApiExeption("유저 정보가 존재하지 않습니다.");
            Token token = OrrnrrContext.Tokens.GetTokenById(tokenId) ?? throw new NotFoundApiException("토큰 정보가 존재하지 않습니다.");

            int numberOwned = OrrnrrContext.TokenHoldingsHistories
                .Where(x => x.User == user)
                .Where(x => x.Token == token)
                .Select(x => x.TokenCount)
                .FirstOrDefault();

            var numberSell = OrrnrrContext.TokenOrderHistories
                .Where(x => x.User == user)
                .Where(x => x.Token == token)
                .Where(x => !x.IsBuyOrder)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => !x.IsCanceled)
                .Sum(x => x.OrderCount - x.CompleteCount);

            if (numberOwned < numberSell + count)
            {
                throw new BadRequestApiException("소유한 토큰 개수가 충분하지 않습니다.");
            }

            var newOrder = TokenOrderHistory.CreateSellOrder(user, token, price, count);

            var existsOrders = OrrnrrContext.TokenOrderHistories
                .GetMatchingOrders(newOrder)
                .ToArray();

            foreach (var existsOrder in existsOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                int signedPrice = existsOrder.OrderPrice!.Value;
                TransactionHistory newTrade = TransactionService.CreateTransactionHistory(newOrder, existsOrder, signedPrice);
                OrrnrrContext.TransactionHistories.Add(newTrade);
            }

            OrrnrrContext.TokenOrderHistories.Add(newOrder);
            OrrnrrContext.SaveChanges();
            transaction.Commit();

            return newOrder;
        }

        private TokenOrderHistory CreateLimitBuyOrder(int userId, int tokenId, int price, int count)
        {
            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            User user = OrrnrrContext.Users.GetUserById(userId) ?? throw new UnauthorizedApiExeption("유저 정보가 존재하지 않습니다.");
            Token token = OrrnrrContext.Tokens.GetTokenById(tokenId) ?? throw new NotFoundApiException("토큰 정보가 존재하지 않습니다.");

            int existsBuyPayment = OrrnrrContext.TokenOrderHistories
                .Where(x => x.User == user)
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsBuyOrder)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => x.OrderPrice.HasValue)
                .Sum(x => x.OrderPrice!.Value * (x.OrderCount - x.CompleteCount));

            if (!user.CanPay(existsBuyPayment + price * count))
            {
                throw new BadRequestApiException("잔고가 충분하지 않습니다.");
            }

            var newOrder = TokenOrderHistory.CreateBuyOrder(user, token, price, count);

            var existsOrders = OrrnrrContext.TokenOrderHistories
                .GetMatchingOrders(newOrder)
                .ToArray();

            foreach (var existsOrder in existsOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                int signedPrice = existsOrder.OrderPrice!.Value;
                TransactionHistory newTrade = TransactionService.CreateTransactionHistory(newOrder, existsOrder, signedPrice);
                OrrnrrContext.TransactionHistories.Add(newTrade);
            }

            OrrnrrContext.TokenOrderHistories.Add(newOrder);
            OrrnrrContext.SaveChanges();
            transaction.Commit();

            return newOrder;
        }

        public TokenOrderHistory CreateMarketOrder(int userId, int tokenId, bool isBuyOrder, int count)
        {
            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);

            User user = OrrnrrContext.Users.GetUserById(userId) ?? throw new UnauthorizedApiExeption("유저 정보가 존재하지 않습니다.");
            Token token = OrrnrrContext.Tokens.GetTokenById(tokenId) ?? throw new NotFoundApiException("토큰 정보가 존재하지 않습니다.");

            var newOrder = new TokenOrderHistory(user, token, isBuyOrder, count);

            var matchedOrders = OrrnrrContext.TokenOrderHistories
                .GetMatchingOrders(newOrder)
                .ToArray();

            foreach (var matchedOrder in matchedOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                int signedPrice = matchedOrder.OrderPrice!.Value;
                TransactionHistory newTransactionHistory = TransactionService.CreateTransactionHistory(newOrder, matchedOrder, signedPrice);
                OrrnrrContext.TransactionHistories.Add(newTransactionHistory);
            }

            OrrnrrContext.TokenOrderHistories.Add(newOrder);
            OrrnrrContext.SaveChanges();
            transaction.Commit();

            return newOrder;
        }

        public Result<TokenOrderHistory> CreateMarketOrder(int userId, int tokenId, bool isBuyOrder, int count)
        {
            using var transaction = OrrnrrContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);


            var user = OrrnrrContext.Users.GetUserById(userId);
            if (user is null)
            {
                return ErrorCode.NotFoundUser.GetResult<TokenOrderHistory>();
            }

            var token = OrrnrrContext.Tokens.GetTokenById(tokenId);
            if (token is null)
            {
                return new Result<TokenOrderHistory>("토큰 정보가 존재하지 않습니다.", ErrorCode.NotFoundToken);
            }

            TokenOrderHistory newOrder = new TokenOrderHistory(user, token, isBuyOrder, count);

            var matchedOrders = OrrnrrContext.TokenOrderHistories
                .GetMatchingOrders(newOrder)
                .ToArray();

            foreach (var matchedOrder in matchedOrders)
            {
                if (newOrder.ExecutableCount == 0)
                {
                    break;
                }

                int signedPrice = matchedOrder.OrderPrice!.Value;
                TransactionHistory newTransactionHistory = TransactionService.CreateTransactionHistory(newOrder, matchedOrder, signedPrice);
                OrrnrrContext.TransactionHistories.Add(newTransactionHistory);
            }

            OrrnrrContext.TokenOrderHistories.Add(newOrder);
            OrrnrrContext.SaveChanges();
            transaction.Commit();

            return new Result<TokenOrderHistory>(newOrder);
        }
    }
}
