using System;
using System.Threading.Tasks;
using TygaSoft.Model.DbTables;

namespace TygaSoft.IRepositories
{
    public interface IUsersRepository
    {
        Task<UsersInfo> GetUserInfoAsync(int applicationId, string name);
        Task<int> DeleteAsync(int applicationId, string name);
        Task<int> InsertAsync(UsersInfo model);
        Task<int> UpdateAsync(UsersInfo model);
        
    }
}