using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.Response
{
    public class ErrorException : Exception
    {
        public Error Error { get; set; }
        public ErrorException(Error error)
        {
            Error = error;
        }
    }
}
