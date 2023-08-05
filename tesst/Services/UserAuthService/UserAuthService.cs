using common.Data.Response;
using common.Helper.HashGenerator;
using common.Helper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Security.Claims;
using common.Helper.TokenGenerator;
using admin.Services.UserService;
using common.Constant;
using System.IdentityModel.Tokens.Jwt;

namespace admin.Services.UserAuthService
{
    public class UserAuthService : IUserAuthService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IHashGenerator _hashGenerator;
        private readonly IUserService _userService;
        public UserAuthService(
            ITokenGenerator tokenGenerator,
            IHashGenerator hashGenerator,
            IUserService userService
            )
        {
            _tokenGenerator = tokenGenerator;
            _hashGenerator = hashGenerator;
            _userService = userService;
        }

        public async Task<(string, DateTime)> Login(string username, string password)
        {
            
            var user = await _userService.GetUsername(username);
            if (user == null) throw new ErrorException(ErrorUtil.NoUserFound);

            if (!_hashGenerator.ValidatePassword(password, user.Password))
                throw new ErrorException(ErrorUtil.LoginInvalid);

            string roleClaim;

            roleClaim = Constants.CLAIM_VALUE_USER;

            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim(Constants.CLAIM_TYPES_ROLE, roleClaim)
            };

            var (token, expires) = _tokenGenerator.CreateToken(claims);


            return (token, expires);
        }

        public bool ValidateToken(string token)
        {
            if (token == null)
                return false;

            try
            {

                var tokenValue = _tokenGenerator.ValidateToken(token);

                if (tokenValue.Item1 != null) return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
