using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using BMSDbEntities;
using BMSWebApp1.Helper;
using BMSWebApp1.Managers;
using BMSWebApp1.Models;

namespace BMSWebApp1.Controllers
{
    public class TokenController : ApiController
    {
        [HttpPost]
        [Route("token")]
        public IHttpActionResult GetToken(HttpRequestMessage request)
        {
            try
            {
                var email = HttpContext.Current.Request.Form["email"];
                var password = HttpContext.Current.Request.Form["password"];
                if (BMSDbMethods.LoginAttempt(email, password, out CUSTOMER customer) == "Success")
                {
                    var role = "user";
                    if (customer.isAdmin == true)
                    {
                        role = "admin";
                    }
                    IAuthContainerModel model = GetJWTContainerModel(customer.CustomerEmail, role);
                    IAuthService1 authService = new JWTService(model.SecretKey);

                    string token = authService.GenerateToken(model);

                    if (!authService.IsTokenValid(token))
                        throw new UnauthorizedAccessException();
                    else
                    {
                        List<Claim> claims = authService.GetTokenClaims(token).ToList();

                    }
                    return Ok(token);
                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return BadRequest();
            }
        }

        [NonAction]
        #region Private Methods
        private static JWTContainerModel GetJWTContainerModel(string username, string role)
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }
            };
        }
        #endregion
    }
}
