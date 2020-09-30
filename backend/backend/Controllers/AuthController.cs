using backend.DataTransferObjects;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(registerViewModel);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }
        
        // api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(loginViewModel);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }
    }
}
