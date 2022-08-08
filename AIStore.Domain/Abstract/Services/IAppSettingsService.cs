using AIStore.Domain.Models.Settings;

namespace AIStore.Domain.Abstract.Services
{
    public interface IAppSettingsService
    {
        AppSettings GetSettings { get; }
        AIStoreSettings AIStoreSettings { get; }

    }
}
