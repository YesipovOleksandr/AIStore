using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Gets()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var role= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;


            var result = new List<string> { "видеоэффект", "фотоэффект", "аудиоэффект", "работастекстом", "аудиоэффект" };
            return Ok(result);
        }
    }
}
