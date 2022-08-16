using AIStore.Domain.Models.Settings.ClientConfigs;

namespace AIStore.Domain.Models.Settings
{
    public class AppSettings
    {
        public ClientConfig ClientConfig { get; set; }
        public JWTOptions JWTOptions { get; set; }
    }
}
