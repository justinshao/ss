using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSystem.OmnipotentCardWebUI.Models
{
    public class UserLoginModel
    {
        public string UserAccount { get; set; }
        public string Password { get; set; }
        public bool RememberPassword { get; set; }
        public string ErrorMessage { get; set; }
    }
}