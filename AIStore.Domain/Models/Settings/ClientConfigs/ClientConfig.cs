using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Models.Settings.ClientConfigs
{
    public class ClientConfig
    {
        public EnvironmentConfig EnvironmentConfig { get; set; }
        public string StaticUrl { get; set; }
    }
}
