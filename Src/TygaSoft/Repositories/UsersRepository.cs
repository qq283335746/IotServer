using System;
using System.Linq;
using System.Threading.Tasks;
using TygaSoft.IRepositories;
using TygaSoft.Model.DbTables;

namespace TygaSoft.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private SqliteContext _context;

        public UsersRepository(SqliteContext context)
        {
            _context = context;
        }

        public async Task<int> SaveUsers(UsersInfo usersInfo)
        {
            if(usersInfo == null) return -1;

            Guid.TryParse(usersInfo.Id,out var gId);
            if(gId.Equals(Guid.Empty)){
                _context.Users.Add(usersInfo);
            }
            else{
                _context.Users.Update(usersInfo);
            }

            return await _context.SaveChangesAsync();
        }
    }
}