using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IUserService
    {
        Task<User> GetById(long Id);
        Task<User> GetByLogin(string login);
        Task Update(User item);
    }
}
