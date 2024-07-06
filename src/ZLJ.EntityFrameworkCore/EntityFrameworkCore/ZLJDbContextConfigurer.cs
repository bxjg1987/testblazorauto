using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ZLJ.EntityFrameworkCore
{
    public static class ZLJDbContextConfigurer
    {  
        //ef8支持sqlserver2016+  我们暂时需要兼容2012
        //https://learn.microsoft.com/zh-cn/ef/core/what-is-new/ef-core-8.0/breaking-changes#mitigations
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
