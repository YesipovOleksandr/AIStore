using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Gets()
        {
            var result = new List<string> { "AI", "mobile", "web" };
            return Ok(result);
        }
    }
}
