using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Helper.TokenGenerator
{
    public class ServerSecretOptions
    {
        public string ChecksumKey { get; set; }
        public string JWTSecret { get; set; }
    }
}
