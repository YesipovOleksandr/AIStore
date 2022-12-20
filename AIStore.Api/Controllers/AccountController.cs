using AIStore.Api.ViewModels;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Extensions;
using AIStore.Domain.Models.ExternalAuth;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;
using System.Web;

namespace AIStore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IOptions<AppSettings> _settings;
        private readonly IUserService _userService;
        private readonly IAuthExternalService _authExternalService;

        public AccountController(IAuthService authService,
                                 IMapper mapper,
                                 IOptions<AppSettings> settings,
                                 ITokenService tokenService,
                                 IAuthExternalService authExternalService,
                                 IUserService userService)
        {
            _authService = authService;
            _mapper = mapper;
            _settings = settings;
            _tokenService = tokenService;
            _userService = userService;
            _authExternalService = authExternalService;
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

        [AllowAnonymous]
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
            var newExpires= DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newExpires;
            _userService.Update(user);

            return Ok( new TokenResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expires= newExpires
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
            var token = await _authExternalService.ExternalTokenAsync(code,provider);

            if (object.Equals(token, null))
                return BadRequest("Token is undefined");

            var profile = await _authExternalService.ProfileAsync(token, provider);

            if (object.Equals(profile, null))
                return BadRequest("Your email is required to complete the sign-in process. Profile is null.");

            if (string.IsNullOrEmpty(profile.Email))
                return BadRequest("Your email is required to complete the sign-in process. Email is empty.");

            var user = new User { Login = profile.Email };
            user.UserRoles = new List<UserRoles>();
            user.Token = _tokenService.GenerateAccessToken(user);
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
            user.RefreshToken = _tokenService.GenerateRefreshToken();;

            var tokenResult = new TokenResult { AccessToken=user.Token,
                                               RefreshToken=user.RefreshToken ,
                                               Expires= user.RefreshTokenExpiryTime };
            var json = JsonConvert.SerializeObject(tokenResult, Formatting.Indented);
            var cookieName = _settings.Value.JWTOptions.CookieName;
            Response.Cookies.Append(cookieName, json);
            return Redirect(_settings.Value.ClientConfig.EnvironmentConfig.BaseUrl);
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

            var user = new User { Login = profile.Email };
            user.UserRoles = new List<UserRoles>();
            user.Token = _tokenService.GenerateAccessToken(user);
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_settings.Value.JWTOptions.TokenLongLifeTime);
            user.RefreshToken = _tokenService.GenerateRefreshToken();

            AuthViewModel result = null;

            result = _mapper.Map<AuthViewModel>(user);

            return Ok(result);
        }
    }
}
