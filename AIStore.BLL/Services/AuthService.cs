using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Abstract.Services.Verifier;
using AIStore.Domain.Enums;
using AIStore.Domain.Models.Email;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Users;
using Microsoft.Extensions.Options;

namespace AIStore.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IHasher _hasher;
        private readonly IOptions<AppSettingsApi> _settings;
        private readonly IVerifierService _verifierService;
        private readonly IMailService _mailService;

        public AuthService(IUserRepository userRepository,
                           IHasher hasher,
                           ITokenService tokenService,
                           IOptions<AppSettingsApi> settings,
                           IVerifierService verifierService,
                           IMailService mailService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hasher = hasher;
            _settings = settings;
            _verifierService = verifierService;
            _mailService = mailService;
        }

        public User Authenticate(User model, bool isPassword = true)
        {
            var user = _userRepository.GetByLogin(model.Login);

            if (user == null)
            {
                return null;
            }

            if (isPassword && !_hasher.Сompare(user.Password, model.Password))
            {
                return null;
            }

            user.Token = _tokenService.GenerateAccessToken(user);
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
            user.RefreshToken = _tokenService.GenerateRefreshToken();

            _userRepository.Update(user);
            return user;
        }

        public User Registration(User model)
        {
            User newUser = new User
            {
                Login = model.Login,
                Password = _hasher.GetHash(model.Password),
                IsEmailСonfirm = model.IsEmailСonfirm,
                UserRoles = new List<UserRoles> { new UserRoles { User = model, Role = Role.User } }
            };

            var user = _userRepository.Create(newUser);

            if (!user.IsEmailСonfirm)
            {
                var verifyCode = _verifierService.SetVerificationCode(user.Id)?.Code;
                var linl = $"{_settings.Value.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/email-confirmation?code={verifyCode}&userId={user.Id}";
                _mailService.SendEmailConfirm(new EmailConfirm { Email = model.Login, Link = linl, Code = verifyCode, ViewName = "~/TemplateMail/EmailConfirm" });
            }

            return user;
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

        public void EmailConfirmation(User user, string code)
        {
            try
            {
                _verifierService.VerificationCode(user.Id, code);
                user.IsEmailСonfirm = true;
                _userRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SendActivationEmail(User model)
        {
            try
            {
                var verifyCode = _verifierService.SetVerificationCode(model.Id)?.Code;
                var linl = $"{_settings.Value.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/email-confirmation?code={verifyCode}&userId={model.Id}";
                _mailService.SendEmailConfirm(new EmailConfirm { Email = model.Login, Link = linl, Code = verifyCode, ViewName = "~/TemplateMail/EmailConfirm" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SendForgotPassword(User model)
        {
            try
            {
                var verifyCode = _verifierService.SetVerificationCode(model.Id)?.Code;
                _mailService.SendForgotPassword(new ForgotPassword { Email = model.Login, Code = verifyCode, ViewName = "~/TemplateMail/ForgotPassword" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}