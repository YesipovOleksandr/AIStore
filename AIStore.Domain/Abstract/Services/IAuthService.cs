using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAuthService
    {
        User Create(User user);
        User Authenticate(User user);
        bool IsUserLoginExist(string login);
    }
}
