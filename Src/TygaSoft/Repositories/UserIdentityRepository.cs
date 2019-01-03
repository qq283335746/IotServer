using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TygaSoft.IRepositories;
using TygaSoft.Model.DbTables;

namespace TygaSoft.Repositories
{
    public class UserIdentityRepository : IUserIdentityRepository
    {
        private readonly SqliteContext _context;

        public UserIdentityRepository(SqliteContext context)
        {
            _context = context;
        }

        // public UserIdentityInfo GetUserIdentityInfo(int applicationId, string userId)
        // {
        //     return _context.UserIdentities.FirstOrDefault(m => m.ApplicationId == applicationId && m.UserId == userId);
        // }

        // public async Task<int> DeleteAsync(int applicationId, string userId)
        // {
        //     var oldInfo = _context.UserIdentities.FirstOrDefault(m => m.ApplicationId == applicationId && m.UserId == userId);
        //     if (oldInfo == null) return -1;

        //     _context.UserIdentities.Remove(oldInfo);

        //     return await _context.SaveChangesAsync();
        // }

        // public async Task<int> InsertAsync(UserIdentityInfo model)
        // {
        //     _context.UserIdentities.Add(model);
        //     return await _context.SaveChangesAsync();
        // }

        // public async Task<int> UpdateAsync(UserIdentityInfo model)
        // {
        //     _context.UserIdentities.Update(model);
        //     return await _context.SaveChangesAsync();
        // }
    }
}