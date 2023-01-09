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

        public async Task<User> Registration(User model)
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
                var verify = await _verifierService.SetVerificationCode(user.Id);
                var linl = $"{_settings.Value.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/email-confirmation?code={verify.Code}&userId={user.Id}";
                await _mailService.SendEmailConfirm(new EmailConfirm { Email = model.Login, Link = linl, Code = verify.Code, ViewName = "~/TemplateMail/EmailConfirm" });
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

        public async Task SendActivationEmail(User model)
        {
            try
            {
                var verify = await _verifierService.SetVerificationCode(model.Id);
                var link = $"{_settings.Value.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/email-confirmation?code={verify.Code}&userId={model.Id}";
                await _mailService.SendEmailConfirm(new EmailConfirm { Email = model.Login, Link = link, Code = verify.Code, ViewName = "~/TemplateMail/EmailConfirm" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SendForgotPassword(User model)
        {
            try
            {
                var verify = await _verifierService.SetVerificationCode(model.Id);
                await _mailService.SendForgotPassword(new ForgotPassword { Email = model.Login, Code = verify.Code, ViewName = "~/TemplateMail/ForgotPassword" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ResetPassword(User model, string newPassword)
        {
            try
            {
                model.Password = _hasher.GetHash(newPassword);
                _userRepository.Update(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}