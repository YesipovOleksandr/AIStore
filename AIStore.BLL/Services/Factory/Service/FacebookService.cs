using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.ExternalAuth;
using AIStore.Domain.Models.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Web;

namespace AIStore.BLL.Services.Factory.Service
{
    public class FacebookService : IExternalService
    {
        private readonly AppSettings _settings;

        public FacebookService(AppSettings settings)
        {
            _settings = settings;
        }

        public async Task<TokenResult> ExternalTokenAsync(string code)
        {
            var redirectUrl = $"{_settings.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/external/callback";
            var content = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string, string>("client_id", _settings.AuthenticationsConfig.Facebook.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.AuthenticationsConfig.Facebook.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri",redirectUrl),
                new KeyValuePair<string, string>("code", code)
            });

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");

                var response = await client.PostAsync("oauth/access_token", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TokenResult>(json);
                }
            }

            return null;
        }

        public string GetAuthenticationUrl(string provider)
        {
            var redirectUrl = $"{_settings.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/external/callback";
            var oAuthServerEndpoint = "https://www.facebook.com/dialog/oauth";
            var queryParams = new Dictionary<string, string>
            {
                {"response_type","code" },
                {"client_id", _settings.AuthenticationsConfig.Facebook.ClientId },
                {"scope",_settings.AuthenticationsConfig.Facebook.Scope },
                {"state", provider},
                {"redirect_uri",redirectUrl },
                {"display","page" }
            };

            var url = QueryHelpers.AddQueryString(oAuthServerEndpoint, queryParams);
            return url;
        }

        public async Task<ProfileResult> ProfileAsync(TokenResult token)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("fields", "email,first_name,last_name,verified");
            query.Add("access_token", token.AccessToken);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.facebook.com");

                var response = await client.GetAsync("me/?" + query.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<FacebookProfileResult>(json);

                    return new ProfileResult(profile.Id, profile.Email, profile.FirstName, profile.LastName, $"https://graph.facebook.com/{profile.Id}/picture?type=large&access_token={token.AccessToken}");
                }
            }

            return null;
        }
    }
}
