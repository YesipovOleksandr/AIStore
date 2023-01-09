using AIStore.Api.ViewModels;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Abstract.Services.Verifier;
using AIStore.Domain.Extensions;
using AIStore.Domain.Models.Email;
using AIStore.Domain.Models.ExternalAuth;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace AIStore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IOptions<AppSettingsApi> _settings;
        private readonly IUserService _userService;
        private readonly IAuthExternalService _authExternalService;
        private readonly IVerifierService _verifierService;

        public AccountController(IAuthService authService,
                                 IMapper mapper,
                                 IOptions<AppSettingsApi> settings,
                                 ITokenService tokenService,
                                 IAuthExternalService authExternalService,
                                 IUserService userService,
                                 IVerifierService verifierService)
        {
            _authService = authService;
            _mapper = mapper;
            _settings = settings;
            _tokenService = tokenService;
            _userService = userService;
            _authExternalService = authExternalService;
            _verifierService = verifierService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Authenticate(_mapper.Map<User>(model));
                if (user == null)
                {
                    return Unauthorized();
                }
                AuthViewModel result = null;

                result = _mapper.Map<AuthViewModel>(user);

                return Ok(result);
            }
            else
            {
                ModelState.AddModelError("LoginError", "Некорректные логин и(или) пароль");
                return ValidationProblem(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public IActionResult Registration([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.Registration(_mapper.Map<User>(model));
                if (user == null)
                {
                    return BadRequest();
                }

                AuthViewModel result = null;

                result = _mapper.Map<AuthViewModel>(_authService.Authenticate(_mapper.Map<User>(model)));

                if (result == null)
                {
                    return Unauthorized();
                }

                return Ok(result);
            }
            else
            {
                ModelState.AddModelError("RegisterError", "Некорректные логин и(или) пароль");
                return ValidationProblem(ModelState);
            }
        }

        [AllowAnonymous]
        [HttpPost("login-exist")]
        public IActionResult IsExistEmail([FromBody] UserLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.IsUserLoginExist(model.Login);
                if (user)
                {
                    ModelState.AddModelError("LoginError", "логин уже существует");
                    return ValidationProblem(ModelState);
                }
            }
            else
            {
                ModelState.AddModelError("LoginError", "Некорректные логин");
                return ValidationProblem(ModelState);
            }
            var response = new Dictionary<string, string> { { "status", "200" } };
            return Ok(response);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userId = principal.GetId();
            if (userId == null)
                return BadRequest("Invalid client request");
            var user = _userService.GetById(userId.Value);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newExpires = DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newExpires;
            _userService.Update(user);

            return Ok(new TokenResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expires = newExpires
            });
        }

        [AllowAnonymous]
        [HttpGet("external/login")]
        public IActionResult GoogleLogin(string provider)
        {
            var url = _authExternalService.GetAuthenticationUrl(provider);
            return Redirect(url);
        }

        [AllowAnonymous]
        [HttpGet("external/callback")]
        public async Task<IActionResult> ExternaLoginCallback(string state, CancellationToken cancellationToken, string? code = null)
        {
            var provider = state;
            var token = await _authExternalService.ExternalTokenAsync(code, provider);

            if (object.Equals(token, null))
                return BadRequest("Token is undefined");

            var profile = await _authExternalService.ProfileAsync(token, provider);

            if (object.Equals(profile, null))
                return BadRequest("Your email is required to complete the sign-in process. Profile is null.");

            if (string.IsNullOrEmpty(profile.Email))
                return BadRequest("Your email is required to complete the sign-in process. Email is empty.");

            var user = _userService.GetByLogin(profile.Email);
            if (user == null)
            {
                var newPassword = Guid.NewGuid().ToString();
                user = new User { Login = profile.Email, IsEmailСonfirm = true, Password = newPassword };
                user = await _authService.Registration(_mapper.Map<User>(user));
            }

            SetCookie(user);
            return Redirect(_settings.Value.ClientConfig.EnvironmentConfig.WebUrl);
        }

        [AllowAnonymous]
        [HttpPost("external/google-token")]
        public async Task<IActionResult> GoogleToken()
        {
            var authorization = Request.Headers.Authorization;

            if (object.Equals(authorization, null))
                return BadRequest($"{nameof(Request.Headers.Authorization)} header is required to complete the sign-in process. {nameof(Request.Headers.Authorization)} is undefined");

            var profile = await _authExternalService.GoogleOneTapProfileAsync(authorization);

            if (object.Equals(profile, null))
                return BadRequest("Your email is required to complete the sign-in process. Profile is null.");

            if (string.IsNullOrEmpty(profile.Email))
                return BadRequest("Your email is required to complete the sign-in process. Email is empty.");

            var user = _userService.GetByLogin(profile.Email);
            if (user == null)
            {
                user = new User { Login = profile.Email };
                user.UserRoles = new List<UserRoles>();
            }
            user.Token = _tokenService.GenerateAccessToken(user);
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
            user.RefreshToken = _tokenService.GenerateRefreshToken();

            AuthViewModel result = _mapper.Map<AuthViewModel>(user);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string login)
        {
            try
            {
                var user = _userService.GetByLogin(login);
                if (user == null)
                {
                    return BadRequest("user is null");
                }

                await _authService.SendForgotPassword(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("verify-recover-password-code")]
        public async Task<IActionResult> VerifyPasswordCode(string login, string code)
        {
            try
            {
                var user = _userService.GetByLogin(login);
                if (user == null)
                {
                    return BadRequest("user is null");
                }

                _verifierService.VerificationCode(user.Id, code, false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassord(ResetPassordViewModel model)
        {
            try
            {
                var user = _userService.GetByLogin(model.login);
                if (user == null)
                {
                    return BadRequest("user is null");
                }

                _verifierService.VerificationCode(user.Id, model.code);
                _authService.ResetPassword(user, model.newPassword);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("email-confirmation")]
        public async Task<IActionResult> EmailConfirm(string code, long userId)
        {
            var user = _userService.GetById(userId);
            if (user == null)
            {
                return BadRequest("user is null");
            }
            else if (user.IsEmailСonfirm)
            {
                SetCookie(user);
                return Redirect($"{_settings.Value.ClientConfig.EnvironmentConfig.WebUrl}?{nameof(EmailConfirm)}=true");
            }
            try
            {
                _authService.EmailConfirmation(user, code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            SetCookie(user);
            return Redirect($"{_settings.Value.ClientConfig.EnvironmentConfig.WebUrl}?{nameof(EmailConfirm)}=true");
        }

        [HttpGet("send-activation-email")]
        public async Task<IActionResult> SendActivationEmail()
        {
            try
            {
                var user = _userService.GetById(User.GetId().Value);
                await _authService.SendActivationEmail(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        private void SetCookie(User user)
        {
            if (user != null)
            {
                user.Token = _tokenService.GenerateAccessToken(user);
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
                user.RefreshToken = _tokenService.GenerateRefreshToken();
                _userService.Update(user);
                var tokenResult = new TokenResult
                {
                    AccessToken = user.Token,
                    RefreshToken = user.RefreshToken,
                    Expires = user.RefreshTokenExpiryTime
                };
                var json = JsonConvert.SerializeObject(tokenResult, Formatting.Indented);
                var cookieName = _settings.Value.JWTOptions.CookieName;
                Response.Cookies.Append(cookieName, json);
            }
        }
    }
}
