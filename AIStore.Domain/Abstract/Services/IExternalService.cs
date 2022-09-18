using AIStore.Domain.Models.ExternalAuth;

namespace AIStore.Domain.Abstract.Services
{
    public interface IExternalService
    {
        Task<TokenResult> ExternalTokenAsync(string code);
        string GetAuthenticationUrl(string provider);
        Task<ProfileResult> ProfileAsync(TokenResult token);
    }
}
