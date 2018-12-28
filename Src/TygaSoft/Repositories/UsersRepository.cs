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

        public UsersInfo GetUserInfo(int applicationId, string userName)
        {
            return _context.Users.FirstOrDefault(m => m.ApplicationId == applicationId && m.Name == userName);
        }

        public async Task<int> InsertAsync(UsersInfo userInfo)
        {
            _context.Users.Add(userInfo);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(UsersInfo userInfo)
        {
            _context.Users.Update(userInfo);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int applicationId, string userName)
        {
            var userInfo = _context.Users.FirstOrDefault(m => m.ApplicationId == applicationId && m.Name == userName);
            if (userInfo == null) return -1;

            _context.Users.Remove(userInfo);

            return await _context.SaveChangesAsync();
        }
    }
}