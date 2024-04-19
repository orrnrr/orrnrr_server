using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;

namespace OrrnrrWebApi.Repositories
{
    public static class OrderRepository
    {
        public static IEnumerable<TokenOrderHistory> GetExecutableOrders(this DbSet<TokenOrderHistory> orders, Token token, int price, bool isBuyOrder)
        {
            var query = orders
                .Where(x => x.TokenId == token.Id)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => x.IsBuyOrder != isBuyOrder)
                .Where(x => !x.IsCanceled);

            query = isBuyOrder ? query.Where(x => x.OrderPrice <= price) :
                query.Where(x => x.OrderPrice >= price);

            return query
                .OrderByDescending(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);
        }
    }
}
