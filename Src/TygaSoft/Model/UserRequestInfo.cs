using System;
using System.Collections.Generic;

namespace TygaSoft.Model
{
    public class UserRequestInfo : RequestBaseInfo
    {
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Roles { get; set; }

    }
}