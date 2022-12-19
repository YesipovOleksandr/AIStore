using AIStore.BLL.Services.Factory;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.ExternalAuth;
using AIStore.Domain.Models.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace AIStore.BLL.Services
{
    public class AuthExternalService: IAuthExternalService
    {
        private readonly IOptions<AppSettings> _settings;

        public AuthExternalService(IOptions<AppSettings> settings)
        {
            _settings = settings;
        }

        public async Task<TokenResult> ExternalTokenAsync(string code, string provider)
        {
            var service = AuthExternalServiceFactory.CreateService(provider,_settings.Value);
            var tokenResult = await service.ExternalTokenAsync(code);
            return tokenResult;
        }

        public string GetAuthenticationUrl(string provider)
        {
            var service = AuthExternalServiceFactory.CreateService(provider, _settings.Value);
            var url = service.GetAuthenticationUrl(provider);

            return url;
        }

        public async Task<ProfileResult> GoogleOneTapProfileAsync(string idToken)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("id_token", idToken);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://oauth2.googleapis.com/");

                var response = await client.GetAsync($"tokeninfo?{query}");

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

        public async Task<ProfileResult> ProfileAsync(TokenResult token,string provider)
        {
            var service = AuthExternalServiceFactory.CreateService(provider, _settings.Value);
            return await service.ProfileAsync(token);
        }
    }
}
