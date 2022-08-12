using AIStore.Domain.Models.Settings;

namespace AIStore.Domain.Abstract
{
    public interface IAppSettingsService
    {
        AppSettings GetSettings { get; }
        AIStoreSettings AIStoreSettings { get; }

    }
}
