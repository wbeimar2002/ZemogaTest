using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZemogaTest.Services.Interfaces;
using ZemogaTest.Utilities.Payloads;

namespace ZemogaTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserPayload userPayload)
        {
            var user = _userService.Authenticate(userPayload);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}