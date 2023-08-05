using common.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Helper
{
    public static class ErrorUtil
    {
        public static readonly Error NoUserFound = new Error() { code = "10000", message = "No User Found with current param" };
        public static readonly Error AuthTokenInvalid = new Error() { code = "10001", message = "Auth token invalid" };
        public static readonly Error LoginInvalid = new Error() { code = "10002", message = "Login Invalid" };
    }
}
