using Microsoft.EntityFrameworkCore;
using ZLJ.Core.TestSimple;
namespace ZLJ.EntityFrameworkCore
{
    public partial class ZLJDbContext
    {
         public DbSet<TestSimpleEntity> TestSimple { get; set; }
    }
}