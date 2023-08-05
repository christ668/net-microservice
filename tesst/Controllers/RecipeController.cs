using admin.Services.RecipeService;
using admin.Services.UserAuthService;
using common.Data.RecipeData;
using common.Data.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _RecipeService;
        private readonly IUserAuthService _UserAuthService;

        public RecipeController(
                              IRecipeService recipeService,
                              IUserAuthService userAuthService)
        {
            _RecipeService = recipeService;
            _UserAuthService = userAuthService;
        }

        [HttpGet("Recipe")]
        [ProducesResponseType(typeof(GenericListResponse<RecipeData>), 400)]
        [ProducesResponseType(typeof(BasicResponse), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                //var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                //var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                //if (!authenticated)
                //    return Unauthorized();

                var result = await _RecipeService.GetAll();
                return Ok
                (
                    new GenericListResponse<RecipeData>()
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

        [HttpGet("Recipe/Id")]
        [ProducesResponseType(typeof(GenericResponse<RecipeData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> GetById(int Id)
        {

            try
            {
                //var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                //var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                //if (!authenticated)
                //    return Unauthorized();

                var result = await _RecipeService.GetById(Id);
                return Ok(new GenericResponse<RecipeData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }


        }

        [HttpPost("Recipe/add")]
        [ProducesResponseType(typeof(GenericResponse<RecipeData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> InsertUser([FromBody] RecipeData data)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _RecipeService.Add(data);
                return Ok(new GenericResponse<RecipeData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }
        //for dev only

        [HttpPost("Recipe/add-no-auth")]
        [ProducesResponseType(typeof(GenericResponse<RecipeData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> InsertUserDev([FromBody] RecipeData data)
        {
            try
            {
                var result = await _RecipeService.Add(data);
                return Ok(new GenericResponse<RecipeData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }

        [HttpPut("Recipe/update")]
        [ProducesResponseType(typeof(GenericResponse<RecipeData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> UpdateUser(RecipeData User)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                var result = await _RecipeService.Update(User);
                return Ok(new GenericResponse<RecipeData>() { Data = result });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }

        [HttpDelete("Guest-Table/{Id}")]
        [ProducesResponseType(typeof(GenericResponse<RecipeData>), 200)]
        [ProducesResponseType(typeof(Error), 400)]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            try
            {
                var nameClaim = Request.Headers[HeaderNames.Authorization].ToString();
                var authenticated = _UserAuthService.ValidateToken(nameClaim.Replace("Bearer ", ""));

                if (!authenticated)
                    return Unauthorized();

                await _RecipeService.Delete(Id);
                return Ok(new BasicResponse() { Message = "delete success" });
            }
            catch (ErrorException e)
            {
                return BadRequest(new BasicResponse() { Error = e.Error });
            }
        }
    }
}
