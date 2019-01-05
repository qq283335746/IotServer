using System;

namespace TygaSoft.Model.DbTables
{
    public class UserIdentityInfo
    {
        public int ApplicationId { get; set; }
        public string UserId{get;set;}
        public string Token{get;set;}
        public DateTime CreatedDate{get;set;}
        public DateTime LastUpdatedDate{get;set;}
    }
}