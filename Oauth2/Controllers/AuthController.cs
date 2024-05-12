using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oauth2.Data.DTO;
using Oauth2.Identity;

namespace Oauth2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager) : ControllerBase
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
    }
}
