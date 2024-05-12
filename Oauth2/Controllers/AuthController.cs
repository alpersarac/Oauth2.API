using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oauth2.Data.DTO;
using Oauth2.Identity;
using System;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Oauth2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager
        ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            AppUser appUser = new AppUser
            {
                Email = registerDto.email,
                firstName = registerDto.firstName,
                lastName = registerDto.lastName,
                UserName = registerDto.userName
            };
            IdentityResult result = await userManager.CreateAsync(appUser, registerDto.password);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(new { message = result.Errors.Select(x => x.Description) });
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByIdAsync(changePasswordDto.userId.ToString());

            if (appUser == null)
            {
                return BadRequest(new { message = "User not found" });
            }
            IdentityResult result = await userManager.ChangePasswordAsync(appUser, changePasswordDto.currentPassword, changePasswordDto.newPassword);
            if (result.Succeeded)
            {
                return Ok(new { message = "Password updated" });
            }
            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> GenerateTokenForPasswordReset(string email, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(email);
            if (appUser == null)
            {
                return BadRequest(new { message = "User not found with that email" });
            }
            string token = await userManager.GeneratePasswordResetTokenAsync(appUser);

            return Ok(token);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordWithToken(ChangePasswordWithTokenDto changePasswordWithToken, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.FindByEmailAsync(changePasswordWithToken.email);
            if (appUser is null)
            {
                return BadRequest(new { message = "User not found with that email" });
            }
            IdentityResult result = await userManager.ResetPasswordAsync(appUser, changePasswordWithToken.token, changePasswordWithToken.newPassword);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors.Select(x => x.Description));
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == loginDto.usernameOrEmail ||
            x.UserName == loginDto.usernameOrEmail,
            cancellationToken);

            if (appUser is null)
            {
                return BadRequest(new { message = "User not found with that email or username" });
            }
            bool result = await userManager.CheckPasswordAsync(appUser, loginDto.password);
            if (!result)
            {
                return BadRequest(new { message = "Password is incorrect" });
            }
            return Ok(new {token = "token"});
        }
        [HttpPost]
        public async Task<IActionResult> LoginWithSignInManager(LoginDto loginDto, CancellationToken cancellationToken)
        {
            AppUser? appUser = await userManager.Users.FirstOrDefaultAsync(x =>
            x.Email == loginDto.usernameOrEmail ||
            x.UserName == loginDto.usernameOrEmail,
            cancellationToken);

            if (appUser is null)
            {
                return BadRequest(new { message = "User not found with that email or username" });
            }
            SignInResult result = await signInManager.CheckPasswordSignInAsync(appUser, loginDto.password, true);
            if (result.IsLockedOut)
            {
                TimeSpan? timeSpan = appUser.LockoutEnd - DateTime.UtcNow;
                if (timeSpan is not null)
                {
                    return StatusCode(500, $"User has blocked due to multiple login attempts please try {timeSpan.Value.TotalSeconds} second later");
                }
                else
                {
                    return StatusCode(500, $"User has blocked due to multiple login attempts please try 2 minutes later");
                }
            }
            if (!result.Succeeded)
            {
                return StatusCode(500, $"Your username or email address is incorrect.");
            }
            if (result.IsNotAllowed)
            {
                return StatusCode(500, $"Your email is not verified.");
            }
            
            return Ok(new { token = "token"});
        }
    }
}
