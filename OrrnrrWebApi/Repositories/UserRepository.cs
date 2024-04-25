using DataAccessLib.Models;
using Microsoft.EntityFrameworkCore;

namespace OrrnrrWebApi.Repositories
{
    public static class UserRepository
    {
        public static User? GetUserById(this DbSet<User> users, int userId)
        {
            return users
                .Where(user => user.Id == userId)
                .FirstOrDefault();
        }
    }
}
