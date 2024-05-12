using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oauth2.Identity
{
    public sealed class ApplicationIdentityDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationIdentityDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
