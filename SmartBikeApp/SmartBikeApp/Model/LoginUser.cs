using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBikeApp.Model
{
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Type = "user";
    }
}
