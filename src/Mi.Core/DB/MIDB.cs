using System.Data;

using Microsoft.EntityFrameworkCore;

namespace Mi.Repository.DB
{
    public class MIDB : DbContext
    {
        public MIDB()
        { }

        public MIDB(DbContextOptions<MIDB> contextOptions) : base(contextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>();
        }

        /// <summary>
        /// Db连接对象
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                return base.Database.GetDbConnection();
            }
        }
    }
}