using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AIStore.Web.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        // Dependency Injection
        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Reading the AuthHeader which is signed with JWT
            string? auth_user = context.Request.Cookies["auth_user"];

            if (auth_user != null)
            {
                var accessToken = JObject.Parse(auth_user).SelectToken("access_token").ToString();

                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

                var identity = new ClaimsIdentity(jwt.Claims, "basic");
                context.User = new ClaimsPrincipal(identity);
            }
            //Pass to the next middleware
            await _next(context);
        }
    }
}