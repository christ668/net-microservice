using admin.Services.GuestTableService;
using admin.Services.UserAuthService;
using admin.Services.UserService;
using common.Data.GuestTable;
using common.Data.Response;
using common.Data.UserData;
using common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestTableController : ControllerBase
    {
        private readonly IGuestTableService _GuestTableService;
        private readonly IUserAuthService _UserAuthService;

        public GuestTableController(
                              IGuestTableService guestTableService,
                              IUserAuthService userAuthService)
        {
            _GuestTableService = guestTableService;
            _UserAuthService = userAuthService;
        }

        [HttpGet("Guest-Table")]
        [ProducesResponseType(typeof(GenericListResponse<GuestTableData>), 400)]
        [ProducesResponseType(typeof(BasicResponse), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _GuestTableService.GetAll();
                return Ok
                (
                    new GenericListResponse<GuestTableData>()
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

        [HttpGet("Guest-Table/Id")]
        [ProducesResponseType(typeof(GenericResponse<GuestTableData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetUserById(int Id)
        {

            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _GuestTableService.GetById(Id);
                return Ok(new GenericResponse<GuestTableData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }


        }

        [HttpPost("Guest-Table/add")]
        [ProducesResponseType(typeof(GenericResponse<GuestTableData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> InsertUser([FromBody] GuestTableData data)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _GuestTableService.Add(data);
                return Ok(new GenericResponse<GuestTableData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }
        //for dev only
        [HttpPost("Guest-Table/add-no-auth")]
        [ProducesResponseType(typeof(GenericResponse<GuestTableData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> InsertUserDev([FromBody] GuestTableData data)
        {
            try
            {
                var result = await _GuestTableService.Add(data);
                return Ok(new GenericResponse<GuestTableData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }

        [HttpPut("Guest-Table/update")]
        [ProducesResponseType(typeof(GenericResponse<GuestTableData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> UpdateUser(GuestTableData User)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _GuestTableService.Update(User);
                return Ok(new GenericResponse<GuestTableData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }

        [HttpDelete("Guest-Table/{Id}")]
        [ProducesResponseType(typeof(GenericResponse<GuestTableData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                await _GuestTableService.Delete(Id);
                return Ok(new BasicResponse() { Message = "delete success" });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }
    }
}
