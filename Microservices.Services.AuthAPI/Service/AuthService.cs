﻿using Microservices.Services.AuthAPI.Data;
using Microservices.Services.AuthAPI.Models.Dto;
using Microservices.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Microservices.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (user == null || !isValid)
            {
                return new LoginResponseDto() { User = null, Token = string.Empty };
            }

            UserDto userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                PhoneNumer = user.PhoneNumber
            };
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = string.Empty,
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumer
            };
            try
            {
                var result = await userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = db.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);
                    var userDto = new UserDto()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumer = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }
    }
}
