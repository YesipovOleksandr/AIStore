using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIStore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(/*[FromBody] LoginViewModel userLoginVM*/)
        {

            //if (userLoginVM.AccountType == AccountType.Null)
            //    ModelState.TryAddModelError("AccountType", AppStrings.ValueNotValid);

            //if (!ModelState.IsValid)
            //    return BadRequest(new ApiModelErrors(ModelState));

            //AuthViewModel result = null;

            //try
            //{
            //    result = _userAuthService.Authenticate(userLoginVM);
            //    if (result == null)
            //        return NotFound();
            //}
            //catch (Exception ex)
            //{
            //    var logger = _loggerFactory.CreateLogger("AccountController");
            //    logger.LogError(ex, $"Auth error");
            //    return new StatusCodeResult((int)ResponseStatus.InternalServerError);
            //}

            //if (userLoginVM.AccountType == AccountType.Admin)
            //{
            //    SetTokenCookie(result.Token);
            //}

            //return Ok(result);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("registration")]
        public IActionResult Registration(/*[*//*FromBody] RegisterViewModel userRegisterVM*/)
        {
            return Ok();
        }


        //[AllowAnonymous]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var isUser = _authService.Authenticate(_mapper.Map<User>(model));
        //        if (isUser)
        //        {
        //            await Authenticate(model.Login); // аутентификация

        //            return RedirectToAction("Admin", "Account");
        //        }
        //        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        //    }
        //}
    }
}
