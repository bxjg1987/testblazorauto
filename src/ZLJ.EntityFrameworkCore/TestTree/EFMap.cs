using ZLJ.Core.TestTree;
using ZLJ.Core.Share.TestTree;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZLJ.EntityFrameworkCore.TestTree
{
    /// <summary>
    /// 测试树 ef映射
    ///</summary>
    public class EFMap : IEntityTypeConfiguration<TestTreeEntity>
    {
        public void Configure(EntityTypeBuilder<TestTreeEntity> builder)
        {
            //表名称
            builder.ToTable("ZLJ_TestTree",t=>t.HasComment("测试树"));

            builder.MapGeneralTree();
            /// <summary>
            /// 名称
            ///</summary>
            var Name = builder.Property(x => x.Name);
            Name.HasComment("测试树");
            Name.HasMaxLength(TestTreeShareConsts.NameMaxLength);
            Name.IsUnicode(true);
            Name.IsRequired(false);
            /// <summary>
            /// 年龄
            ///</summary>
            var Age = builder.Property(x => x.Age);
            Age.HasComment("测试树");
            Age.IsRequired(false);
            /// <summary>
            /// 出生日期
            ///</summary>
            var Birthday = builder.Property(x => x.Birthday);
            Birthday.HasComment("测试树");
            Birthday.IsRequired(false);
            /// <summary>
            /// 字符串字段1
            ///</summary>
            var StringField1 = builder.Property(x => x.StringField1);
            StringField1.HasComment("测试树");
            StringField1.HasMaxLength(TestTreeShareConsts.StringField1MaxLength);
            StringField1.IsUnicode(false);
            StringField1.IsRequired(true);
            /// <summary>
            /// 状态
            ///</summary>
            var Status = builder.Property(x => x.Status);
            Status.HasComment("测试树");
            Status.IsRequired(true);
            /// <summary>
            /// 测试3
            ///</summary>
            var F2 = builder.Property(x => x.F2);
            F2.HasComment("测试树");
            F2.IsRequired(false);
            /// <summary>
            /// 测试4
            ///</summary>
            var F3 = builder.Property(x => x.F3);
            F3.HasComment("测试树");
            F3.IsRequired(true);
        }
    }
}