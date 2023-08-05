using common.Data.Response;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace common.Helper.TokenGenerator
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ServerSecretOptions _secretOptions;
        private readonly JWTOptions _jwtOptions;

        public TokenGenerator(IOptions<ServerSecretOptions> secretOptions, IOptions<JWTOptions> jwtOptions)
        {
            _secretOptions = secretOptions.Value;
            _jwtOptions = jwtOptions.Value;
        }

        public (string, DateTime) CreateToken(ICollection<Claim> claims)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("abcdefghijklmnoprstuvwxyz11234567890");
            var expires = DateTime.UtcNow.AddMinutes(60);

            var jwtToken = new JwtSecurityToken
            (
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            return (tokenHandler.WriteToken(jwtToken), expires);
        }

        public (string, DateTime) CreateRefreshToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("abcdefghijklmnoprstuvwxyz11234567890");
            var expiredOn = DateTime.Now.AddMinutes(600);

            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.NameId, userId)
            };

            var jwtToken = new JwtSecurityToken
            (
                claims: claims,
                expires: expiredOn,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            var token = tokenHandler.WriteToken(jwtToken);

            return (token, expiredOn);
        }

        public (ClaimsPrincipal, Error) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("abcdefghijklmnoprstuvwxyz11234567890");
            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameter, out SecurityToken vt);
            }
            catch (Exception e)
            {
                var validateTokenError = ErrorUtil.AuthTokenInvalid;
                validateTokenError.details = new string[1] { e.Message };
                return (null, validateTokenError);
            }

            return (claimsPrincipal, null);
        }
    }
}
