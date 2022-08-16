
using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Repository
{
    public interface IUserRepository
    {
        User Get(string Login);
        User Create(User user);
    }
}
