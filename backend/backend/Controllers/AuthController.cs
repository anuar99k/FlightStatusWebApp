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
                var registrationResult = await _userService.RegisterUserAsync(registerViewModel);

                if (registrationResult.IsSuccess)
                {
                    // в случае успешной регистрации в DTO объекте registrationResult поле message содержит токен.
                    // сохранение токена в куках
                    SetTokenInCookies(registrationResult.Message);

                    return Ok(registrationResult);
                }

                return BadRequest(registrationResult);
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
                    SetTokenInCookies(loginResult.Message);

                    loginResult.Message = "Signed in successfully";

                    return Ok(loginResult);
                }

                return BadRequest(loginResult);
            }
            return BadRequest("Some properties are not valid");
        }
        private void SetTokenInCookies(string token)
        {
            HttpContext.Response.Cookies.Append("Token", token,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(Convert.ToInt32(_configuration["AuthSettings:TokenLifeTimeInMinutes"]))
                });
        }
    }
}
