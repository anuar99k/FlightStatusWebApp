using backend.DataTransferObjects;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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
                UserManagerResponse loginResult = await _userService.LoginUserAsync(loginViewModel);

                if (loginResult.IsSuccess)
                {
                    // в случае успешной авторизации в DTO объекте loginResult поле message содержит токен.
                    // сохранение токена в куках
                    HttpContext.Response.Cookies.Append("Token", loginResult.Message,
                        new CookieOptions
                        {
                            MaxAge = TimeSpan.FromMinutes(Convert.ToInt32(_configuration["AuthSettings:TokenLifeTimeInMinutes"]))
                        });

                    loginResult.Message = "Signed in successfully";

                    return Ok(loginResult);
                }

                return BadRequest(loginResult);
            }
            return BadRequest("Some properties are not valid");
        }
    }
}
