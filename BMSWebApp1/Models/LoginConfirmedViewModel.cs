using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BMSDbEntities;

namespace BMSWebApp1.Models
{
    public class LoginConfirmedViewModel
    {
        public string AuthToken;
        public CUSTOMER Customer;
    }
}