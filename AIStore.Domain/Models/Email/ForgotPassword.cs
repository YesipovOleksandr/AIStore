using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStore.Domain.Models.Email
{
    public class ForgotPassword
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public string ViewName { get; set; }
    }
}
