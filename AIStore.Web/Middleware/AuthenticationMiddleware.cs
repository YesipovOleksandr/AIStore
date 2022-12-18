using AIStore.Domain.Extensions;
using AIStore.Domain.Models.Settings;
using AIStore.Domain.Models.Settings.ClientConfigs;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace AIStore.Web.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly HttpClient _httpclient;
        private readonly RequestDelegate _next;
        private readonly IOptions<AppSettings> _appSettings;

        public AuthenticationMiddleware(RequestDelegate next,
                                        HttpClient httpclient,
                                         IOptions<AppSettings> appSettings)
        {
            _next = next;
            _httpclient = httpclient;
            _appSettings = appSettings;
        }

        public class TokenModel
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }

        public async Task Invoke(HttpContext context)
        {
            string? auth_user = context.Request.Cookies["auth_user"];

            if (auth_user != null)
            {
                var json = JObject.Parse(auth_user);
                var accessToken = json?.SelectToken("access_token")?.ToString();
                var refreshToken = json?.SelectToken("refresh_token")?.ToString();

                if (!String.IsNullOrWhiteSpace(accessToken))
                {
                    var jwtToken = new JwtSecurityToken(accessToken);
                    if ((jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow))
                    {
                        var tokenModul = new TokenModel { AccessToken = accessToken, RefreshToken = refreshToken };
                        var parametrs = new StringContent(JsonConvert.SerializeObject(tokenModul), Encoding.UTF8, "application/json");
                        var response = await _httpclient.PostAsync(_appSettings.Value.ClientConfig.EnvironmentConfig.ApiUrl + "api/Account/refresh", parametrs).ConfigureAwait(false);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var responseData = response.Content.ReadAsStringAsync().Result;
                            var responseJson = JObject.Parse(responseData);

                            json.Property("access_token").Value = responseJson?.SelectToken("access_token")?.ToString();
                            json.Property("refresh_token").Value = responseJson?.SelectToken("refresh_token")?.ToString();
                            context.Response.Cookies.Append("auth_user", json.ToString());
                        }
                        else
                        {
                            context.Response.Cookies.Delete("auth_user");
                            context.Response.Redirect("/");
                        }
                    }

                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                    var identity = new ClaimsIdentity(jwt.Claims, "basic");
                    context.User = new ClaimsPrincipal(identity);

                    if (!json.ContainsKey("id"))
                    {
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