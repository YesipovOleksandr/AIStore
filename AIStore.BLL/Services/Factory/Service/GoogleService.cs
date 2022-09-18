using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.ExternalAuth;
using AIStore.Domain.Models.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace AIStore.BLL.Services.Factory.Service
{
    public class GoogleService: IExternalService
    {
        private readonly AppSettings _settings;

        public GoogleService(AppSettings settings)
        {
            _settings = settings;
        }

        public async Task<TokenResult> ExternalTokenAsync(string code)
        {
            var redirectUrl = $"{_settings.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/external/callback";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id",_settings.AuthenticationsConfig.Google.ClientId),
                new KeyValuePair<string, string>("client_secret",_settings.AuthenticationsConfig.Google.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
            });

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.googleapis.com");

                var response = await client.PostAsync("oauth2/v4/token", content);

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

            var oAuthServerEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

            var ClientId = _settings.AuthenticationsConfig.Google.ClientId;
            var scope = "email profile";
            var queryParams = new Dictionary<string, string>
            {
                {"scope",scope },
                {"client_id",ClientId },
                {"redirect_uri",redirectUrl },
                {"response_type","code" },
                {"approval_prompt", "force" },
                {"access_type", "online" },
                {"state", provider}
            };

            var url = QueryHelpers.AddQueryString(oAuthServerEndpoint, queryParams);

            return url;
        }

        public async Task<ProfileResult> ProfileAsync(TokenResult token)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("access_token", token.AccessToken);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.googleapis.com");

                var response = await client.GetAsync("oauth2/v1/userinfo/?" + query.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<GoogleProfileResult>(json);

                    //remove size parameter from url to get original photo size
                    if (!string.IsNullOrEmpty(profile.PhotoUrl))
                    {
                        profile.PhotoUrl = Regex.Replace(profile.PhotoUrl, "=s\\d+-c", string.Empty);
                    }

                    return new ProfileResult(profile.Id, profile.Email, profile.FirstName, profile.LastName, profile.PhotoUrl);
                }
            }
            return null;
        }
    }
}
