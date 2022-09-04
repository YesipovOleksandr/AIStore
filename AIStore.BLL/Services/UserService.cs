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

        public User GetById(long Id)
        {
            return _userRepository.GetById(Id);
        }

        public void Update(User item)
        {
            _userRepository.Update(item);
            _userRepository.Save();
        }
    }
}
