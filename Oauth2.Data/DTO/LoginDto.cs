using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oauth2.Data.DTO
{
    public sealed record LoginDto(string usernameOrEmail, string password);
}
