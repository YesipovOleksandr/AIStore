using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Enums;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Users;
using Microsoft.Extensions.Options;

namespace AIStore.BLL.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHasher _hasher;
        private readonly IOptions<AppSettings> _settings;

        public AuthService(IUserRepository userRepository,
                           IHasher hasher,
                           ITokenService tokenService,
                           IOptions<AppSettings> settings)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hasher = hasher;
            _settings = settings;
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
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
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
                IsEmailСonfirm = user.IsEmailСonfirm,
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