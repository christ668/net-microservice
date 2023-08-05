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
        public static readonly Error NoUserFound = new Error() { code = "9000", message = "No User Found with current param" };
        public static readonly Error NoUserDetailFound = new Error() { code = "9001", message = "No User Detail Found with current param" };
        public static readonly Error UserAlreadyExist = new Error() { code = "9002", message = "user already exist" };
        public static readonly Error NoTableFound = new Error() { code = "9003", message = "No Guest table found with current param" };
        public static readonly Error GuestTableExist = new Error() { code = "9004", message = "guest table already exist" };

        public static readonly Error AuthTokenInvalid = new Error() { code = "10001", message = "Auth token invalid" };
        public static readonly Error LoginInvalid = new Error() { code = "10002", message = "Login Invalid" };
    }
}
