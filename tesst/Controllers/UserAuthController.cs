using admin.Services.UserAuthService;
using common.Data.Response;
using common.Data.UserAuthData;
using common.Helper.TokenGenerator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace admin.Controllers
{
    [ApiController]
    [Route("user-auth")]
    public class UserAuthController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;

        public UserAuthController(IUserAuthService userAuthService
           )
        {
            _userAuthService = userAuthService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserAuthResponseData), 200)]
        [ProducesResponseType(typeof(BasicResponse), 400)]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {

                var (token, expires) = await _userAuthService.Login(email, password);

                return Ok(new UserAuthResponseData
                {
                    Token = token,
                    Expires = expires
                });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }
    }
}
