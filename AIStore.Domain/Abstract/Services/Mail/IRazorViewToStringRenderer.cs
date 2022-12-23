

namespace AIStore.Domain.Abstract.Services.Mail
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TMode>(string viewName, TMode model);
    }
}