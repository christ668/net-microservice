using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Helper.TokenGenerator
{
    public class JWTOptions
    {
        public int TokenDuration { get; set; }
        public int RefreshTokenDuration { get; set; }
    }
}
