using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.Users;

namespace AIStore.BLL.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHasher _hasher;

        public AuthService(IUserRepository userRepository, IHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public bool Authenticate(User model)
        {
            var user = _userRepository.Get(model.Login);

            if (user == null)
            {
                return false;
            }

            if (!_hasher.Сompare(user.Password, model.Password))
            {
                return false;
            }

            return true;
        }

        public bool Create(User user)
        {
            User newUser = new User
            {
                Login = user.Login,
                Password = _hasher.GetHash(user.Password),
            };

            return _userRepository.Create(newUser); ;
        }
    }
}