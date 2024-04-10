using DataAccessLib.Models;

namespace OrrnrrWebApi.Services
{
    public class TokenSourceService : ITokenSourceService
    {
        public OrrnrrContext OrrnrrContext { get; }
        public TokenSourceService(OrrnrrContext context)
        {
            OrrnrrContext = context;
        }

        public bool IsExistsById(int tokenSourceId)
        {
            return OrrnrrContext.TokenSources.Any(x => x.Id == tokenSourceId);
        }
    }
}
