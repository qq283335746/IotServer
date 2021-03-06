using System;
using System.Collections.Generic;

namespace TygaSoft.Model.DbTables
{
    public class UsersInfo
    {
        public string Id { get; set; }
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PasswordFormat { get; set; }
        public string PasswordSalt { get; set; }
        public string Roles { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}