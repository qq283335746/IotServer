using System.Collections.Generic;
using TygaSoft.Model;

namespace TygaSoft.Model
{
    public class Result
    {
        public ResCodeOptions ResCode { get; set; }

        public string Message { get; set; }
    }

    public class HelloResult : Result
    {
        public string Data { get; set; }
    }

    public class LoginResult : Result
    {
        public string Token { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }

    public class UsersResult : Result
    {
        public UserInfo UserInfo { get; set; }
    }

    public class FindOrderResult : Result
    {
        public IEnumerable<OrderInfo> Orders { get; set; }
    }
}