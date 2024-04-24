using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;

namespace OrrnrrWebApi.Repositories
{
    public static class OrderRepository
    {
        public static IEnumerable<TokenOrderHistory> GetCanBuyOrdersForLimit(this DbSet<TokenOrderHistory> orders, Token token, int price)
        {
            return orders
                .Where(x => x.Token == token)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => !x.IsBuyOrder)
                .Where(x => !x.IsCanceled)
                .Where(x => x.OrderPrice.HasValue)
                .Where(x => x.OrderPrice <= price)
                .Include(x => x.User)
                .OrderBy(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);
        }

        

        public static IEnumerable<TokenOrderHistory> GetCanSellOrdersForLimit(this DbSet<TokenOrderHistory> orders, Token token, int price)
        {
            return orders
                .Where(x => x.TokenId == token.Id)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => x.IsBuyOrder)
                .Where(x => !x.IsCanceled)
                .Where(x => x.OrderPrice.HasValue)
                .Where(x => x.OrderPrice >= price)
                .Include(x => x.User)
                .OrderByDescending(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);
        }

        public static IQueryable<TokenOrderHistory> GetMatchingOrders(this DbSet<TokenOrderHistory> orders, TokenOrderHistory order)
        {
            var query = orders
                .Where(x => x.Token == order.Token)
                .Where(x => x.OrderCount > x.CompleteCount)
                .Where(x => !x.IsCanceled)
                .Where(x => x.IsBuyOrder != order.IsBuyOrder)
                .Where(x => x.OrderPrice.HasValue);

            query = order.OrderPrice.HasValue switch
            {
                true => query.GetMatchingOrdersForMarket(order),
                false => query.GetMatchingOrdersForLimit(order),
            };

            return query.Include(x => x.User);
        }

        private static IQueryable<TokenOrderHistory> GetMatchingOrdersForMarket(this IQueryable<TokenOrderHistory> orders, TokenOrderHistory order)
        {
            return order.IsBuyOrder switch
            {
                true => orders.GetCanBuyOrdersForMarket(order),
                false => orders.GetCanSellOrdersForMarket(order),
            };
        }

        private static IQueryable<TokenOrderHistory> GetCanBuyOrdersForMarket(this IQueryable<TokenOrderHistory> orders, TokenOrderHistory order)
        {
            return orders
                .OrderBy(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);
        }

        private static IQueryable<TokenOrderHistory> GetCanSellOrdersForMarket(this IQueryable<TokenOrderHistory> orders, TokenOrderHistory order)
        {
            return orders
                .OrderByDescending(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);
        }

        private static IQueryable<TokenOrderHistory> GetMatchingOrdersForLimit(this IQueryable<TokenOrderHistory> orders, TokenOrderHistory order)
        {
            return order.IsBuyOrder switch { 
                true => orders.GetCanBuyOrdersForLimit(order),
                false => orders.GetCanSellOrdersForLimit(order),
            };
        }

        private static IQueryable<TokenOrderHistory> GetCanBuyOrdersForLimit(this IQueryable<TokenOrderHistory> orders, TokenOrderHistory order)
        {
            return orders
                .Where(x => x.OrderPrice <= order.OrderPrice)
                .OrderBy(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);
        }

        private static IQueryable<TokenOrderHistory> GetCanSellOrdersForLimit(this IQueryable<TokenOrderHistory> orders, TokenOrderHistory order)
        {
            return orders
                .Where(x => x.OrderPrice >= order.OrderPrice)
                .OrderByDescending(x => x.OrderPrice)
                .ThenBy(x => x.OrderDateTime);
        }
    }
}
