using AIStore.Domain.Extensions;
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

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? auth_user = context.Request.Cookies["auth_user"];

            if (auth_user != null)
            {
                var json = JObject.Parse(auth_user);
                var accessToken = json?.SelectToken("access_token")?.ToString();

                if (!String.IsNullOrWhiteSpace(accessToken))
                {
                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                    var identity = new ClaimsIdentity(jwt.Claims, "basic");
                    context.User = new ClaimsPrincipal(identity);

                    if (!json.ContainsKey("id")){
                        json.Add("id", context.User.GetId());
                        json.Add("role", context.User.GetRole().ToString());
                        json.Add("email", context.User.GetUserEmail());
                        context.Response.Cookies.Append("auth_user", json.ToString());
                    }
                }
            }

            await _next(context);
        }
    }
}