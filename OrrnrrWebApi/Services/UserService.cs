using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;
using OrrnrrWebApi.Utils;

namespace OrrnrrWebApi.Services
{
    public class UserService : IUserService
    {
        public UserService(OrrnrrContext orrnrrContext)
        {
            OrrnrrContext = orrnrrContext;
        }
        
        protected OrrnrrContext OrrnrrContext{ get; }

        public User? GetUserById(int userId)
        {
            return OrrnrrContext.Users
                .Where(user => user.Id == userId)
                .FirstOrDefault();
        }
    }
}
