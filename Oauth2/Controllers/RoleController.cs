using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oauth2.Identity;

namespace Oauth2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(RoleManager<AppRole> roleManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(string role, CancellationToken cancellationToken)
        {
            AppRole appRole = new AppRole
            {
                Name = role
            };
            IdentityResult result = await roleManager.CreateAsync(appRole);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            List<AppRole> appRoles = await roleManager.Roles.ToListAsync();

            return Ok(appRoles);
        }
    }
}
