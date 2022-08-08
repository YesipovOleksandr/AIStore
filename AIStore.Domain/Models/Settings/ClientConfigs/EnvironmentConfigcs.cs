using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Models.Settings.ClientConfigs
{
    public class EnvironmentConfig
    {
        public string ApiUrl { get; set; }
        public string BasePath { get; set; }
        public string BaseUrl { get; set; }
    }
}
