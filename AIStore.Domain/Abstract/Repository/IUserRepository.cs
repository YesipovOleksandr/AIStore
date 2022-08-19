
using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Repository
{
    public interface IUserRepository
    {
        User GetByLogin(string Login);
        User Create(User user);
    }
}
