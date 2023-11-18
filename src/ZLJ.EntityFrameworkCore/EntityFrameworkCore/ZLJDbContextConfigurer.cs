using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ZLJ.EntityFrameworkCore
{
    public static class ZLJDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ZLJDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ZLJDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }

        //public static void Configure(DbContextOptionsBuilder<ZLJDbContext> builder, string connectionString)
        //{
        //    var serverVersion = ServerVersion.AutoDetect(connectionString);
        //    builder.UseMySql(connectionString, serverVersion);
        //}

        //public static void Configure(DbContextOptionsBuilder<ZLJDbContext> builder, DbConnection connection)
        //{
        //    var serverVersion = ServerVersion.AutoDetect(connection.ConnectionString);
        //    builder.UseMySql(connection, serverVersion);
        //}
    }
}
