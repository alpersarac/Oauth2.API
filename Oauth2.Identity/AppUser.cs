using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oauth2.Identity
{
    public sealed class AppUser: IdentityUser<Guid>
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName => string.Join(" ", firstName, lastName);
    }
}
