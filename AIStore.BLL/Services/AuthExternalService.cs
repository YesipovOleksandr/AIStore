using AIStore.BLL.Services.Factory;
using AIStore.Domain.Abstract.Services;
using AIStore.Domain.Models.ExternalAuth;
using AIStore.Domain.Models.Settings;
using Microsoft.Extensions.Options;

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

        public async Task<ProfileResult> ProfileAsync(TokenResult token,string provider)
        {
            var service = AuthExternalServiceFactory.CreateService(provider, _settings.Value);
            return await service.ProfileAsync(token);
        }
    }
}
