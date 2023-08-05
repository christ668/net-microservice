using common.Data.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace common.Helper.TokenGenerator
{
    public interface ITokenGenerator
    {
        (string, DateTime) CreateToken(ICollection<Claim> claims);
        (string, DateTime) CreateRefreshToken(string userId);
        (ClaimsPrincipal, Error) ValidateToken(string token);
    }
}
