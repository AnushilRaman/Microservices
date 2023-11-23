﻿using Microservices.Web.Models;
using Microservices.Web.Service.IService;
using Microservices.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto response = await authService.LoginAsync(loginRequestDto);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Customerror", response.Message);
                return View(loginRequestDto);
            }
        }

        [HttpGet]

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin , Value = SD.RoleAdmin },
                new SelectListItem {Text = SD.RoleCustomer , Value = SD.RoleCustomer },
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto response = await authService.RegisterAsync(registrationRequestDto);
            ResponseDto assignRole;
            if (response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                }
                assignRole = await authService.AssignRoleAsync(registrationRequestDto);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["successMessage"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin , Value = SD.RoleAdmin },
                new SelectListItem {Text = SD.RoleCustomer , Value = SD.RoleCustomer },
            };
            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
    }
}
