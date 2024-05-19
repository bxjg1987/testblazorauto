using Microsoft.EntityFrameworkCore;
using ZLJ.Core.TestTree;
namespace ZLJ.EntityFrameworkCore
{
    public partial class ZLJDbContext
    {
         public DbSet<TestTreeEntity> TestTree { get; set; }
    }
}