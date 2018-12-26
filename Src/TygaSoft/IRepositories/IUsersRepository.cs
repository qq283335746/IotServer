using System;
using System.Threading.Tasks;
using TygaSoft.Model.DbTables;

namespace TygaSoft.IRepositories
{
    public interface IUsersRepository
    {
        Task<int> SaveUsers(UsersInfo usersInfo);
    }
}