using DataAccessLib.Models;

namespace OrrnrrWebApi.Services
{
    public interface IUserService
    {
        User? GetUserById(int userId);
    }
}
