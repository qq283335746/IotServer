using System;

namespace TygaSoft.Model
{
    public class LoginRequestInfo : RequestBaseInfo
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }
}