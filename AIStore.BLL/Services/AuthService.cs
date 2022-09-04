using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Enums;
using AIStore.Domain.Models.Users;

namespace AIStore.BLL.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHasher _hasher;

        public AuthService(IUserRepository userRepository, IHasher hasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hasher = hasher;
        }

        public User Authenticate(User model)
        {
            var user = _userRepository.GetByLogin(model.Login);

            if (user == null)
            {
                return null;
            }

            if (!_hasher.Сompare(user.Password, model.Password))
            {
                return null;
            }

            user.Token = _tokenService.GenerateAccessToken(user);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            user.RefreshToken = _tokenService.GenerateRefreshToken();

            _userRepository.Update(user);

            return user;
        }

        public User Registration(User user)
        {
            User newUser = new User
            {
                Login = user.Login,
                Password = _hasher.GetHash(user.Password),
                UserRoles=new List<UserRoles> { new UserRoles { User = user, Role = Role.User } }
            };

            return _userRepository.Create(newUser);
        }

        public bool IsUserLoginExist(string login)
        {
            var user = _userRepository.GetByLogin(login);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}