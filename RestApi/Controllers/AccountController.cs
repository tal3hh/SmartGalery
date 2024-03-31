using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Account;
using ServiceLayer.Services;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.Utilities;

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
            if (!ModelState.IsValid) return BadRequest(new ResultDto<UserCreateDto>(false,"Validation error" , dto));

            var user = new AppUser
            {
                Fullname = dto.Fullname,
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.Number
            };

            var userEmail = await _userManager.FindByEmailAsync(user.Email);

            if (userEmail != null) return BadRequest(new ResultDto<UserCreateDto>(false, "Bu email artiq istifade olunub.", null));

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

                _messageSend.MimeKitConfrim(appUser, url);

                var roles = await _userManager.GetRolesAsync(user);

                var token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

                return Ok(new ResultDto<UserCreateDto>(true, "E-mailə gələn mesaji təsdiq edin.", dto));
            }

            foreach (var error in identity.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest(new ResultDto<UserCreateDto>(false, "Ugursuz cehd",dto));
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (userId == null || token == null) return BadRequest(new ResultDto<UserCreateDto>(false, "Token və user boşdur.",null));
            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null) return BadRequest(new ResultDto<UserCreateDto>(false, "Token boşdur.", null));
            await _userManager.ConfirmEmailAsync(user, token);

            return Ok(new ResultDto<UserCreateDto>(false, "Uğurla tamamlandı.", null));
        }
        #endregion

        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(new ResultDto<UserLoginDto>(false,"Validation error", dto));

            AppUser? user = await _userManager.FindByEmailAsync(dto.UsernameorEmail);
            if (user == null)
                user = await _userManager.FindByNameAsync(dto.UsernameorEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "İstifadəçi tapılmadı");
                return NotFound(new ResultDto<UserLoginDto>(false, "İstifadəçi tapılmadı.",null));
            }

            var identity = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, false);

            if (identity.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                TokenResponseDto token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

                if (token == null) return BadRequest(new ResultDto<UserLoginDto>(false, "Token boşdur.",null));

                var homeDto = new HomeUserDto
                {
                    Username = user.UserName,
                    Fullname = user.Fullname,
                    Email = user.Email,
                    Token = token.Token,
                    ExpireDate = token.ExpireDate
                };
                return Ok(new ResultDto<HomeUserDto>(true,"Ugurlu", homeDto));
            }
            else
            {
                if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    //ModelState.AddModelError("", $"Qeydiyyat zamanı daxil etdiyiniz e-poçtu təsdiqləyin." +
                    //                                $"Əks halda hesaba daxil ola bilməzsiniz." +
                    //                                $"E-poçt ünvanı: {user.Email}");
                    return BadRequest(new ResultDto<UserLoginDto>(false, $"Qeydiyyat zamanı daxil etdiyiniz e-poçtu                                                            təsdiqləyin." +
                                                    $"Əks halda hesaba daxil ola bilməzsiniz." +
                                                    $"E-poçt ünvanı: {user.Email}",null));
                }
            }
            return BadRequest(new ResultDto<UserLoginDto>(false, "Uğursuz cəhd.", null));
        }
        #endregion
    }
}
