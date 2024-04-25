using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;

namespace OrrnrrWebApi.Repositories
{
    public static class TokenRepository
    {
        public static Token? GetTokenById(this DbSet<Token> tokens, int tokenId)
        {
            return tokens
                .Where(token => token.Id == tokenId)
                .FirstOrDefault();
        }
    }
}
