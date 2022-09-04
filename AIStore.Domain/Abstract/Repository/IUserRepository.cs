
using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Repository
{
    public interface IUserRepository
    {
        User GetById(long Id);
        User GetByLogin(string Login);
        User Create(User item);
        void Update(User item);
        void Save();
    }
}
