
using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Repository
{
    public interface IUserRepository
    {
        Task<User> GetById(long Id);
        Task<User> GetByLogin(string Login);
        Task<User> Create(User item);
        Task Update(User item);
        Task Save();
    }
}
