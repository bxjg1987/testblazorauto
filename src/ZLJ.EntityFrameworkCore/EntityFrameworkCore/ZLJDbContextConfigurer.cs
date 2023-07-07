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
    }
}
