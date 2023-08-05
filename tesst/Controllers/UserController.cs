using admin.Services.UserAuthService;
using admin.Services.UserService;
using common.Data.Response;
using common.Data.UserData;
using common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DemoDbContext DBContext;
        private readonly IUserService _UserService;
        private readonly IUserAuthService _UserAuthService;

        public UserController(DemoDbContext DBContext,
                              IUserService userService,
                              IUserAuthService userAuthService)
        {
            this.DBContext = DBContext;
            _UserService = userService;
            _UserAuthService = userAuthService;
        }

        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(GenericListResponse<UserData>), 400)]
        [ProducesResponseType(typeof(BasicResponse), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _UserService.GetAll();
                return Ok
                (
                    new GenericListResponse<UserData>()
                    {
                        Data = result
                    }
                );
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }

        [HttpGet("GetUserById")]
        [ProducesResponseType(typeof(GenericResponse<UserData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetUserById(int Id)
        {
            
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _UserService.GetUserById(Id);
                return Ok(new GenericResponse<UserData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }

            
        }

        [HttpPost("InsertUser")]
        [ProducesResponseType(typeof(GenericResponse<UserData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> InsertUser([FromBody] UserData User)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _UserService.Add(User);
                return Ok(new GenericResponse<UserData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserData User)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _UserService.Update(User);
                return Ok(new GenericResponse<UserData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }

        [HttpDelete("DeleteUser/{Id}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                await _UserService.Delete(Id);
                return Ok(new BasicResponse() { Message = "delete success" });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }
    }
}
