using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TygaSoft.IRepositories;
using TygaSoft.Model.DbTables;

namespace TygaSoft.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly SqliteContext _context;

        public UsersRepository(SqliteContext context)
        {
            _context = context;
        }

        public async Task<UsersInfo> GetUserInfoAsync(int applicationId, string name)
        {
            if (Guid.TryParse(name, out var userId))
            {
                return await _context.Users.SingleOrDefaultAsync(m => m.ApplicationId == applicationId && m.Id == name);
            }
            return await _context.Users.SingleOrDefaultAsync(m => m.ApplicationId == applicationId && m.Name == name);
        }

        public async Task<int> DeleteAsync(int applicationId, string name)
        {
            UsersInfo oldInfo = null;
            if (Guid.TryParse(name, out var userId))
            {
                oldInfo = await _context.Users.SingleOrDefaultAsync(m => m.ApplicationId == applicationId && m.Id == name);
            }
            else
            {
                oldInfo = await _context.Users.SingleOrDefaultAsync(m => m.ApplicationId == applicationId && m.Name == name);
            }

            if (oldInfo == null) return -1;

            _context.Users.Remove(oldInfo);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> InsertAsync(UsersInfo model)
        {
            _context.Users.Add(model);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(UsersInfo model)
        {
            _context.Users.Update(model);
            return await _context.SaveChangesAsync();
        }
    }
}