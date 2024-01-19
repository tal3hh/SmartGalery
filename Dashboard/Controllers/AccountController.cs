using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Account;
using ServiceLayer.Services;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.ViewModels;
using System.Security.Principal;

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

        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            AppUser? user = await _userManager.FindByEmailAsync(dto.UsernameorEmail);
            if (user is null)
                user = await _userManager.FindByNameAsync(dto.UsernameorEmail);

            if (user is null) return NotFound(dto);

            var identity = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, false);

            if (identity.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                TokenResponseDto token = _tokenService.GenerateJwtToken(user.UserName, (List<string>)roles);

                if (token is null) return BadRequest("Token null");

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
            if (!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                ModelState.AddModelError("", $"Qeydiyyat zamanı daxil etdiyiniz e-poçtu təsdiqləyin." +
                                                $"Əks halda hesaba daxil ola bilməzsiniz." +
                                                $"E-poçt ünvanı: {user.Email}");
                return BadRequest(dto);
            }

            return BadRequest("Istifadəçi adı və şifrə yalnışdır.");
        }
        #endregion

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

            return Redirect("https://www.figma.com/file/id2hFRKe4GIGO6dQPFBPtq/ecommerce-cavid?type=design&node-id=5-10933&mode=design");
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

        #region ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPassVM vm)
        {
            if (!ModelState.IsValid) return BadRequest(vm);

            AppUser? user = await _userManager.FindByNameAsync(vm.Username);
            if (user is null) return NotFound("Belə bir istifadəçi tapılmadı.");

            if (!await _userManager.CheckPasswordAsync(user, vm.OldPassword))
                return BadRequest("Əvvəlki şifrə yanlışdır.");

            var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);

            if (result.Succeeded)
                return Ok("Şifrə uğurla dəyişdirildi.");

            return BadRequest(result.Errors.Select(e => e.Description));
        }
        #endregion


        //Hazir DEYIL
        #region ForgotPassowrd
        //[HttpPost("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword(string email)
        //{
        //    if (string.IsNullOrEmpty(email)) return BadRequest("Email tələb olunur.");

        //    AppUser? user = await _userManager.FindByEmailAsync(email);

        //    if (user == null) return NotFound($"Email '{email}' ilə istifadəçi tapılmadı.");
        //    if (!(await _userManager.IsEmailConfirmedAsync(user))) return BadRequest("Email təsdiqlənməyib.");

        //    string code = await _userManager.GeneratePasswordResetTokenAsync(user);

        //    var url = Url.Action("ForgotResetPassword", "Account", new { email = email, token = code }, Request.Scheme);

        //    // E-poçt göndərmə prosesi
        //    _messageSend.MimeMessageResetPassword(user, url, code);

        //    return Ok("Şifrə sıfırlama linki e-poçtunuza göndərildi.");
        //}

        //[HttpPost("ForgotResetPassword")]
        //public async Task<IActionResult> ForgotResetPassword([FromQuery] string token, string email, string password)
        //{
        //    if (email == null || token == null) return BadRequest("Email və token tələb olunur.");

        //    AppUser? user = await _userManager.FindByEmailAsync(email);

        //    if (user == null) return NotFound($"Email '{email}' ilə istifadəçi tapılmadı.");

        //    // Token'ın doğruluğu yoxlanılır
        //    var result = await _userManager.ResetPasswordAsync(user, token, password);

        //    if (result.Succeeded)
        //    {
        //        return Ok("Şifrə uğurla sıfırlanıb.");
        //    }

        //    // Hata halları üçün daha spesifik xəbərlər əlavə olunur
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error.Description);
        //    }
        //    return BadRequest("Şifrə sıfırlama uğursuz oldu.");
        //}
        #endregion
    }
}
