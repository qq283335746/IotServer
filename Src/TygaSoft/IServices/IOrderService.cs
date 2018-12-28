
using System;
using System.Threading.Tasks;
using TygaSoft.Model;

namespace TygaSoft.IServices
{
    public interface IOrderService
    {
        Task<LoginResult> LoginAsync(LoginRequestInfo requestInfo);
        Task<int> SaveUserAsync(UserRequestInfo requestInfo);
        Task<int> DeleteUserAsync(UserRequestInfo requestInfo);
    }
}