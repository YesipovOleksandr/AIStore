using AIStore.Api.ViewModels;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Extensions;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
            
        public AccountController(IAuthService authService,
                                 IMapper mapper, 
                                 IOptions<AppSettings> settings, 
                                 ITokenService tokenService,
                                 IUserService userService)
        {
            _authService = authService;
            _mapper = mapper;
            _settings = settings;
            _tokenService = tokenService;
            _userService = userService;
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

                return Unauthorized();
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
            user.RefreshToken = newRefreshToken;
            _userService.Update(user);

            return new ObjectResult(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
