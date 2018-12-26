using System;
using Microsoft.EntityFrameworkCore;
using TygaSoft.IRepositories;
using TygaSoft.Model.DbTables;

namespace TygaSoft.Repositories
{
    public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        { }

        public DbSet<UsersInfo> Users { get; set; }
    }
}
