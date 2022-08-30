using AIStore.Api.ViewModels;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AIStore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _settings;

        public AccountController(IAuthService authService, IMapper mapper, IOptions<AppSettings> settings)
        {
            _authService = authService;
            _mapper = mapper;
            _settings = settings;
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
                result.Token = GenerateJWT(user);

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
                result.Token = GenerateJWT(user);

                if (result == null)
                {
                    return Unauthorized();
                }

                return Ok(result);
            }
            else
            {
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
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

        private string GenerateJWT(User model)
        {
            var authParams = _settings.Value.JWTOptions;

            var securityKey = authParams.Secret;
            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,model.Id.ToString()),
                new Claim(ClaimTypes.Email,model.Login),
            };

            foreach (var role in model.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.ToString()));
            }

            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
