using backend.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.Services
{
    // users managing context
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel registerViewModel);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel loginViewModel);
    }

    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            if (registerViewModel == null)
                throw new NullReferenceException("Register model is null");

            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the password",
                    IsSuccess = false
                };
            }

            var identityUser = new IdentityUser { UserName = registerViewModel.Username };

            var result = await _userManager.CreateAsync(identityUser, registerViewModel.Password);
            
            if (result.Succeeded)
            {
                IdentityUser user = await _userManager.FindByNameAsync(registerViewModel.Username);

                JwtSecurityToken token = GetToken(user);

                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                return new UserManagerResponse
                {
                    Message = tokenAsString,
                    IsSuccess = true,
                    ExpireDateTime = token.ValidTo
                };
            }

            return new UserManagerResponse
            {
                Message = "User didn't create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel loginViewModel)
        {
            IdentityUser user = await _userManager.FindByNameAsync(loginViewModel.Username);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that username",
                    IsSuccess = false
                };
            }

            bool passwordIsValid = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (!passwordIsValid)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false
                };
            }

            JwtSecurityToken token = GetToken(user);

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token); 

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDateTime = token.ValidTo
            };
        }

        private JwtSecurityToken GetToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var encryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["Clients:SpaAddress"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["AuthSettings:TokenLifeTimeInMinutes"])),
                signingCredentials: new SigningCredentials(encryptionKey, SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}
