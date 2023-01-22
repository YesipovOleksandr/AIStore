using AIStore.Domain.Abstract.Repository;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Abstract.Services.RecoverPassword;
using AIStore.Domain.Abstract.Services.Verifier;
using AIStore.Domain.Enums;
using AIStore.Domain.Models.Email;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Users;
using Microsoft.AspNetCore.Server.HttpSys;
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
        private readonly IRecoverPasswordService _recoverPasswordService;

        public AuthService(IUserRepository userRepository,
                           IHasher hasher,
                           ITokenService tokenService,
                           IOptions<AppSettingsApi> settings,
                           IVerifierService verifierService,
                           IMailService mailService,
                           IRecoverPasswordService recoverPasswordService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hasher = hasher;
            _settings = settings;
            _verifierService = verifierService;
            _mailService = mailService;
            _recoverPasswordService = recoverPasswordService;
        }

        public async Task<User> Authenticate(User model, bool isPassword = true)
        {
            var user = await _userRepository.GetByLogin(model.Login);

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

            await _userRepository.Update(user);
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

            var user = await _userRepository.Create(newUser);

            if (!user.IsEmailСonfirm)
            {
                var verify = await _verifierService.SetVerificationCode(user.Id);
                var linl = $"{_settings.Value.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/email-confirmation?code={verify.Code}&userId={user.Id}";
                await _mailService.SendEmailConfirm(new EmailConfirm { Email = model.Login, Link = linl, Code = verify.Code, ViewName = "~/TemplateMail/EmailConfirm" });
            }

            return user;
        }

        public async Task<bool> IsUserLoginExist(string login)
        {
            var user = await _userRepository.GetByLogin(login);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task EmailConfirmation(User user, string code)
        {
            try
            {
                _verifierService.VerificationCode(user.Id, code);
                user.IsEmailСonfirm = true;
                await _userRepository.Update(user);
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
                var verify = await _recoverPasswordService.SetRecoverPasswordCode(model.Id);
                await _mailService.SendForgotPassword(new ForgotPassword { Email = model.Login, Code = verify.Code, ViewName = "~/TemplateMail/ForgotPassword" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ResetPassword(User model, string newPassword)
        {
            try
            {
                model.Password = _hasher.GetHash(newPassword);
                await _userRepository.Update(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}