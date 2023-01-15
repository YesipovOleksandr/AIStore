using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.Users;

namespace AIStore.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetById(long Id)
        {
            return await _userRepository.GetById(Id);
        }

        public async Task<User> GetByLogin(string login)
        {
            return await _userRepository.GetByLogin(login);
        }

        public async Task Update(User item)
        {
            _userRepository.Update(item);
            await _userRepository.Save();
        }
    }
}
