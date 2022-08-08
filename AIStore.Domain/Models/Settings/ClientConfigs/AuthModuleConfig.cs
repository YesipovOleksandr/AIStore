using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Models.Settings.ClientConfigs
{
    public class AuthModuleConfig
    {
        public string ApiUrl { get; set; }

        public AuthParams AuthParams { get; set; }

    }
}
