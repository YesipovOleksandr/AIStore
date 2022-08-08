using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Models.Settings.ClientConfigs
{
    public class AuthParams
    {
        public string Client_id { get; set; }
        public string Grant_Type { get; set; }
        public string Source { get; set; }
    }
}
