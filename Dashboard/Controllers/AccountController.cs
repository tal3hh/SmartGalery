using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Account;
using ServiceLayer.Services.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IMessageSend _messageSend;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IMessageSend messageSend, ITokenService tokenService, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _messageSend = messageSend;
            _tokenService = tokenService;
            _context = context;
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

                await _userManager.AddToRoleAsync(user, "Admin");

                AppUser? appUser = await _userManager.FindByEmailAsync(user.Email);

                if (appUser == null) return NotFound(dto);

                string? token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string? url = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, token = token }, Request.Scheme, Request.Host.ToString());

                _messageSend.MimeKitConfrim(appUser, url);

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
            if (UsernameorEmail is null) return BadRequest("E-mail yada istifadəçi adını daxil edin.");
            var user = new AppUser();

            user = await _userManager.FindByNameAsync(UsernameorEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(UsernameorEmail);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return Ok("İstifadəçi silindi.");
            }
            return NotFound("İstifadəçi tapılmadı.");
        }

        [HttpDelete("Role/{name}")]
        public async Task<IActionResult> RoleRemove(string name)
        {
            if (name is null) return BadRequest("Ad boş olmamalıdır.");

            var role = await _roleManager.FindByNameAsync(name);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                return Ok("Role silindi");
            }
            return NotFound("Role taılmadı.");
        }
        #endregion

        #region ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPassDto vm)
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

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            AppUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == model.Email);

            if (user is null) return NotFound("User not found");
            
            string? token = Guid.NewGuid().ToString("N");

            var dbToken = await _context.PasswordResetTokens.SingleOrDefaultAsync(x => x.AppUserId == user.Id);
            if (dbToken != null)
            {
                _context.PasswordResetTokens.Remove(dbToken);
                await _context.SaveChangesAsync();
            }

            var resetToken = new PasswordResetToken { AppUserId = user.Id, Token = token };
            await _context.PasswordResetTokens.AddAsync(resetToken);
            await _context.SaveChangesAsync();

            var resetLink = $"http://your-app.com/reset-password?token={token}";
            //SendResetEmail(model.Email, resetLink);
            _messageSend.MimeMessageResetPassword(user, resetLink);

            return Ok("Password reset link sent to your email");
        }

        [HttpPost("ForgotResetPassword")]
        public async Task<IActionResult> ForgotResetPassword(ResetPasswordDto model)
        {
            var resetToken = _context.PasswordResetTokens
                                     .Include(t => t.AppUser)
                                     .SingleOrDefault(t => t.Token == model.Token);

            if (resetToken == null || resetToken.IsExpired) return BadRequest("Invalid or expired token");

            // Update the user's password
            var result = await _userManager.ChangePasswordAsync(resetToken.AppUser,resetToken.AppUser.PasswordHash,model.NewPassword);

            // Delete the used reset token
            _context.PasswordResetTokens.Remove(resetToken);
            await _context.SaveChangesAsync();

            return Ok("Password reset successfully");
        }

        private void SendResetEmail(string email, string resetLink)
        {
            var fromAddress = new MailAddress("your_email@example.com", "Your Name");
            var toAddress = new MailAddress(email);
            const string fromPassword = "your_email_password";
            const string subject = "Password Reset";
            string body = $"Click the following link to reset your password: {resetLink}";

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
        #endregion
    }
}
