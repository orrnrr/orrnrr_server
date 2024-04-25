using DataAccessLib.Models;
using DataAccessLib.Types;
using Microsoft.EntityFrameworkCore;

namespace OrrnrrWebApi.Repositories
{
    public static class TradeActionRepository
    {
        public static TradeAction? GetTradeAction(this DbSet<TradeAction> tradeActions, TradeActionType tradeActionType)
        {
            return tradeActions
                .Where(x => x.Name == tradeActionType.GetActionName())
                .FirstOrDefault();
        }
    }
}
