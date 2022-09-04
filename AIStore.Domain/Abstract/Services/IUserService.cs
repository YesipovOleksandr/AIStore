using AIStore.Domain.Models.Users;

namespace AIStore.Domain.Abstract.Services
{
    public interface IUserService
    {
        User GetById(long Id);
        void Update(User item);
    }
}
