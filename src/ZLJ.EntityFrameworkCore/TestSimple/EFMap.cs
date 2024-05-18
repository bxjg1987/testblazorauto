using ZLJ.Core.TestSimple;
using ZLJ.Core.Share.TestSimple;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZLJ.EntityFrameworkCore.TestSimple
{
    /// <summary>
    /// 普通数据测试 ef映射
    ///</summary>
    public class EFMap : IEntityTypeConfiguration<TestSimpleEntity>
    {
        public void Configure(EntityTypeBuilder<TestSimpleEntity> builder)
        {
            //表名称
            builder.ToTable("ZLJ_TestSimple",t=>t.HasComment("普通数据测试"));

            /// <summary>
            /// 名称
            ///</summary>
            var Name = builder.Property(x => x.Name);
            Name.HasComment("普通数据测试");
            Name.HasMaxLength(TestSimpleShareConsts.NameMaxLength);
            Name.IsUnicode(true);
            Name.IsRequired(true);
            /// <summary>
            /// 年龄
            ///</summary>
            var Age = builder.Property(x => x.Age);
            Age.HasComment("普通数据测试");
            Age.IsRequired(false);
            /// <summary>
            /// 出生日期
            ///</summary>
            var Birthday = builder.Property(x => x.Birthday);
            Birthday.HasComment("普通数据测试");
            Birthday.IsRequired(false);
            /// <summary>
            /// 字符串字段1
            ///</summary>
            var StringField1 = builder.Property(x => x.StringField1);
            StringField1.HasComment("普通数据测试");
            StringField1.HasMaxLength(TestSimpleShareConsts.StringField1MaxLength);
            StringField1.IsUnicode(false);
            StringField1.IsRequired(false);
            /// <summary>
            /// 状态
            ///</summary>
            var Status = builder.Property(x => x.Status);
            Status.HasComment("普通数据测试");
            Status.IsRequired(true);
            /// <summary>
            /// 测试3
            ///</summary>
            var F2 = builder.Property(x => x.F2);
            F2.HasComment("普通数据测试");
            F2.IsRequired(false);
            /// <summary>
            /// 测试4
            ///</summary>
            var F3 = builder.Property(x => x.F3);
            F3.HasComment("普通数据测试");
            F3.IsRequired(true);
        }
    }
}