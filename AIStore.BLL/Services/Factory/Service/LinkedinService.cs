using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.ExternalAuth;
using AIStore.Domain.Models.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;

namespace AIStore.BLL.Services.Factory.Service
{
    public class LinkedinService : IExternalService
    {
        private readonly AppSettingsApi _settings;

        public LinkedinService(AppSettingsApi settings)
        {
            _settings = settings;
        }

        public async Task<TokenResult> ExternalTokenAsync(string code)
        {
            var redirectUrl = $"{_settings.ClientConfig.EnvironmentConfig.ApiUrl}api/Account/external/callback";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id",_settings.AuthenticationsConfig.Linkedin.ClientId),
                new KeyValuePair<string, string>("client_secret",_settings.AuthenticationsConfig.Linkedin.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUrl),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
            });

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.linkedin.com");

                var response = await client.PostAsync("oauth/v2/accessToken", content);

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

            var oAuthServerEndpoint = "https://www.linkedin.com/oauth/v2/authorization";

            var ClientId = _settings.AuthenticationsConfig.Linkedin.ClientId;
            var queryParams = new Dictionary<string, string>
            {
                {"response_type","code" },
                {"client_id",ClientId },
                {"redirect_uri",redirectUrl },
                {"scope",_settings.AuthenticationsConfig.Linkedin.Scope },
                {"state", provider}
            };

            var url = QueryHelpers.AddQueryString(oAuthServerEndpoint, queryParams);

            return url;
        }

        public async Task<ProfileResult> ProfileAsync(TokenResult token)
        {
            var getEmail = "https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))";
            var getUserinfo = "https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))";
            LinkedinProfileResult? profile= null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var responseUserInfo = await client.GetAsync(getUserinfo);

                if (responseUserInfo.IsSuccessStatusCode)
                {
                    var json = await responseUserInfo.Content.ReadAsStringAsync();
                    profile = JsonConvert.DeserializeObject<LinkedinProfileResult>(json);
                }

                var responseEmail = await client.GetAsync(getEmail);

                if (responseEmail.IsSuccessStatusCode)
                {
                    var json = await responseEmail.Content.ReadAsStringAsync();
                    var linkedinProfileResult = JsonConvert.DeserializeObject<LinkedinProfileResult>(json);
                    profile.Elements = linkedinProfileResult.Elements;
                }

                if (profile == null)
                    return null;

                return new ProfileResult(profile.Id, profile.Elements.FirstOrDefault().handle.emailAddress, profile.FirstName.localized.lang, profile.LastName.localized.lang, $"https://graph.facebook.com/{profile.Id}/picture?type=large&access_token={token.AccessToken}");
            }

            return null;
        }
    }
}
