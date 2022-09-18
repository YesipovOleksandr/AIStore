using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Models.Settings
{
    public class AuthenticationsConfig
    {
        public Google Google { get; set; }
        public Facebook Facebook { get; set; }
    }
}
