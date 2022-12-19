using AIStore.Domain.Models.ExternalAuth;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAuthExternalService
    {
        Task<TokenResult> ExternalTokenAsync(string code,string provider);
        Task<ProfileResult> ProfileAsync(TokenResult token,string provider);
        Task<ProfileResult> GoogleOneTapProfileAsync(string id_token);
        string GetAuthenticationUrl(string provider);
    }
}
