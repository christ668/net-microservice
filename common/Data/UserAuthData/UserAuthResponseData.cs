using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.UserAuthData
{
    public class UserAuthResponseData
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
