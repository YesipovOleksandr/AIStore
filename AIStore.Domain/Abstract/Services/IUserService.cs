using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IUserService
    {
        User GetById(long Id);
        User GetByLogin(string login);
        void Update(User item);
    }
}
