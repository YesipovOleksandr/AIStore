using AIStore.Domain.Models.ExternalAuth;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAuthExternalService
    {
        Task<TokenResult> ExternalTokenAsync(string code,string provider);
        Task<ProfileResult> ProfileAsync(TokenResult token,string provider);
        string GetAuthenticationUrl(string provider);
    }
}
