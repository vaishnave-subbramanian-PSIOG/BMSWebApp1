using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using BMSWebApp1.Models;

namespace BMSWebApp1.Managers
{
    public interface IAuthService1
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
