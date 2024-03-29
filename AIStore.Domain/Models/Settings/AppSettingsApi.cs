﻿using AIStore.Domain.Models.Settings.ClientConfigs;

namespace AIStore.Domain.Models.Settings
{
    public class AppSettingsApi
    {
        public ClientConfig ClientConfig { get; set; }
        public JWTOptions JWTOptions { get; set; }
        public AuthenticationsConfig AuthenticationsConfig { get; set; }
        public MailSettings MailSettings { get; set; }
    }
}
