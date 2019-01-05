using System;
using System.Collections.Generic;

namespace TygaSoft.Model
{
    public class UserInfo
    {
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Token { get; set; }
    }
}