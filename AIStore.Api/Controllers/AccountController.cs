using AIStore.Api.ViewModels;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIStore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var isUser = _authService.Authenticate(_mapper.Map<User>(model));
                if (isUser)
                {
                    // await Authenticate(model.Login); // аутентификация
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }



        [AllowAnonymous]
        [HttpPost("registration")]
        public IActionResult Registration([FromBody] RegisterViewModel model)
        {
            return Ok();
        }
    }
}
