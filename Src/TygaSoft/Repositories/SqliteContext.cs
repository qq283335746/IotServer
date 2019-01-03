using Microsoft.EntityFrameworkCore;
using TygaSoft.Model.DbTables;

namespace TygaSoft.Repositories
{
    public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersInfo>()
            .HasKey(c => c.Id)
            .HasName("PrimaryKey_UsersId")
            ;
            modelBuilder.Entity<OrdersInfo>()
            .HasKey(c => c.Id)
            .HasName("PrimaryKey_OrdersId")
            ;
            
        }

        public DbSet<UsersInfo> Users { get; set; }
         public DbSet<OrdersInfo> Orders { get; set; }
        //public DbSet<UserIdentityInfo> UserIdentities { get; set; }
    }
}
