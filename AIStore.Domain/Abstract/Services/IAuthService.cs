using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAuthService
    {
        bool Create(User user);
        bool Authenticate(User user);
    }
}
