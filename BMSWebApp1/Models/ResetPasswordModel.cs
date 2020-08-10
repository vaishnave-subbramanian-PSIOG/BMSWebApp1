using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMSWebApp1.Models
{
    public class ResetPasswordModel
    {
        public string token { get; set; }
        public string newpassword { get; set; }
    }
}