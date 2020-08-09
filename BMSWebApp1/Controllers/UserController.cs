using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BMSDbEntities;
using BMSWebApp1.Helper;

namespace BMSWebApp1.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("api/user/registeruser")]
        public void PostRegisterUser([FromBody] CUSTOMER customer)
        {
            BMSDbMethods.UserRegistration(customer);
        }

    }
}
