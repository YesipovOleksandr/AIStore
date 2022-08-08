using Microsoft.Extensions.Configuration;

namespace AIStore.Domain.Models.Settings
{
    public class AIStoreSettings
    {
        public AIStoreSettings(IConfiguration configuration)
        {
            IConfigurationSection section = configuration.GetSection("PdfEscapeSettings");
            section.Bind(this);
        }

        public AIStoreSettings() { }
    }
}
