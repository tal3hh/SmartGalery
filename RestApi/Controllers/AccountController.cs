using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Account;
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

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IMessageSend messageSend)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _messageSend = messageSend;
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
                    Name = "Member"
                };

                await _userManager.AddToRoleAsync(user, "Member");

                AppUser? appUser = await _userManager.FindByEmailAsync(user.Email);

                if (appUser == null) return NotFound(dto);

                string? code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string? url = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, token = code }, Request.Scheme, Request.Host.ToString());

                _messageSend.MimeKitConfrim(appUser, url, code);

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
                var homeDto = new HomeUserDto
                {
                    Username = user.UserName,
                    Fullname = user.Fullname,
                    Email = user.Email
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
    }
}
