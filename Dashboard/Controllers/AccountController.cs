﻿using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Account;
using ServiceLayer.Services;
using ServiceLayer.Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMessageSend _messageSend;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IMessageSend messageSend, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _messageSend = messageSend;
            _tokenService = tokenService;
        }

        #region Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            var user = new AppUser
            {
                Fullname = dto.Fullname,
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.Number
            };

            IdentityResult identity = await _userManager.CreateAsync(user, dto.Password);

            if (identity.Succeeded)
            {
                var role = new IdentityRole
                {
                    Name = "Admin"
                };

                await _userManager.AddToRoleAsync(user, "Admin");

                AppUser? appUser = await _userManager.FindByEmailAsync(user.Email);

                if (appUser == null) return NotFound(dto);

                string? code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string? url = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, token = code }, Request.Scheme, Request.Host.ToString());

                _messageSend.MimeKitConfrim(appUser, url, code);

                var roles = await _userManager.GetRolesAsync(user);

                var token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

                return Ok(dto);
            }

            foreach (var error in identity.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest(dto);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (userId == null || token == null) return BadRequest();
            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null) return BadRequest();
            await _userManager.ConfirmEmailAsync(user, token);

            return Ok();
        }
        #endregion

        #region Role
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(RoleCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            var role = new IdentityRole
            {
                Name = dto.Name
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
                return Ok(dto);

            return BadRequest(dto);
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            AppUser? user = await _userManager.FindByEmailAsync(dto.UsernameorEmail);
            if (user == null)
                user = await _userManager.FindByNameAsync(dto.UsernameorEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "İstifadəçi tapılmadı");
                return NotFound(dto);
            }

            var identity = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, false);

            if (identity.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                TokenResponseDto token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

                if (token == null) return BadRequest("Token null");

                var homeDto = new HomeUserDto
                {
                    Username = user.UserName,
                    Fullname = user.Fullname,
                    Email = user.Email,
                    Token = token.Token,
                    ExpireDate = token.ExpireDate
                };
                return Ok(homeDto);
            }
            else
            {
                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError("", $"Qeydiyyat zamanı daxil etdiyiniz e-poçtu təsdiqləyin." +
                                                    $"Əks halda hesaba daxil ola bilməzsiniz." +
                                                    $"E-poçt ünvanı: {user.Email}");
                    return BadRequest(dto);
                }
            }
            return BadRequest(dto);
        }
        #endregion

        #region UsersandRoles

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var userList = await _userManager.Users.ToListAsync();

            List<DashUserDto> userDtos = userList.Select(user => new DashUserDto
            {
                Fullname = user.Fullname,
                Username = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                CreateDate = user.CreateDate
            }).ToList();

            return Ok(userDtos);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            List<DashRoleDto> roleDtos = roles.Select(role => new DashRoleDto
            {
                Name = role.Name
            }).ToList();

            return Ok(roleDtos);
        }
        #endregion

        #region Delete
        [HttpDelete("User/{UsernameorEmail}")]
        public async Task<IActionResult> UserRemove(string UsernameorEmail)
        {
            if (UsernameorEmail != null)
            {
                var user = new AppUser();

                user = await _userManager.FindByNameAsync(UsernameorEmail);
                if (user == null)
                    user = await _userManager.FindByEmailAsync(UsernameorEmail);

                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                    return Ok("Istifadeci silindi");
                }
                return NotFound("Istifadeci tapilmadi.");
            }

            return Unauthorized("UsernameorEmail bosdur");
        }

        [HttpDelete("Role/{name}")]
        public async Task<IActionResult> RoleRemove(string name)
        {
            if (name != null)
            {
                var role = await _roleManager.FindByNameAsync(name);
                
                if (role != null)
                {
                    await _roleManager.DeleteAsync(role);
                    return Ok("Role silindi");
                }
                return NotFound("Role tapilmadi.");
            }

            return Unauthorized("Name bosdur");
        }
        #endregion
    }
}