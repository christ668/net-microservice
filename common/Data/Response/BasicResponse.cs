using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common.Data.Response
{
    public class BasicResponse
    {
        public Error Error { get; set; }
        public string Message { get; set; } = "ok";
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }
}
