using DataAccessLib.Models;
using OrrnrrWebApi.Parameters;
using OrrnrrWebApi.Responses;

namespace OrrnrrWebApi.Services
{
    public class TokenService : ITokenService
    {
        private OrrnrrContext OrrnrrContext { get; }

        public TokenService(OrrnrrContext context)
        {
            OrrnrrContext = context;
        }
    }
}
