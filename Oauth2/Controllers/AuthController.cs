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
            AppUser appUser = new AppUser{
                Email=registerDto.email,
                firstName=registerDto.firstName,
                lastName=registerDto.lastName,
                UserName=registerDto.userName
            };
            IdentityResult result = await userManager.CreateAsync(appUser, registerDto.password);

            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }
    }
}
