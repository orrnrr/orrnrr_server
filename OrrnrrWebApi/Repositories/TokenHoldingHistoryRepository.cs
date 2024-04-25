using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;

namespace OrrnrrWebApi.Repositories
{
    public static class TokenHoldingHistoryRepository
    {
        public static TokenHoldingsHistory CopyLatestTokenHoldingHistory(this DbSet<TokenHoldingsHistory> holdingsHistories, User user, Token token)
        {
            var latest = holdingsHistories
                .Where(x => x.User == user)
                .Where(x => x.Token == token)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (latest is null)
            {
                return new TokenHoldingsHistory(user, token);
            }

            return latest.Copy();
        }
    }
}
