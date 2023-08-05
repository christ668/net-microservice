using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.Response
{
    public class Error
    {
        public string code { get; set; } = null;
        public string message { get; set; } = null;
        public string target { get; set; } = null;
        public object[] details { get; set; } = null;
    }
}
