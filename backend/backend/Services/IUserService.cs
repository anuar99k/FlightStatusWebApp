﻿using backend.DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
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

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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

            var identityUser = new IdentityUser
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(identityUser, registerViewModel.Password);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created successfully",
                    IsSuccess = true
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
            IdentityUser user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that email address",
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

            return new UserManagerResponse
            {
                Message = "Signed in successfully",
                IsSuccess = true
            };
        }
    }
}
