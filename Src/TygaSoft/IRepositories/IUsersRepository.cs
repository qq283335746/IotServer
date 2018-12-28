using System;
using System.Threading.Tasks;
using TygaSoft.Model.DbTables;

namespace TygaSoft.IRepositories
{
    public interface IUsersRepository
    {
        UsersInfo GetUserInfo(int applicationId, string userName);
        Task<int> InsertAsync(UsersInfo userInfo);
        Task<int> UpdateAsync(UsersInfo userInfo);
        Task<int> DeleteAsync(int applicationId, string userName);
    }
}