using BXJG.Utils.Feedback;
using BXJG.Utils.Files;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.EFMaps
{
    public class FeedbackMap : IEntityTypeConfiguration<FeedbackEntity>
    {
         public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
         {
            builder.ToTable("BXJGUtilsFeedbacks", x => x.HasComment("通用的留言（将来可能关联tag、评论作为回复、图片等）"));
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.ExtensionData).IsUnicode().HasMaxLength(BXJGUtilsConsts.ExtDataMaxLength).HasComment("扩展数据");
            builder.Property(c => c.Title).IsUnicode().HasMaxLength(BXJGUtilsConsts.FeedbackTitleMaxLength).HasComment("标题");

            builder.Property(c => c.Content).IsRequired().IsUnicode().HasMaxLength(BXJGUtilsConsts.FeedbackContentMaxLength).HasComment("内容");
            builder.Property(c => c.ConnectInfo).IsUnicode().HasMaxLength(BXJGUtilsConsts.FeedbackConnectInfoMaxLength).HasComment("联系方式 如：手机号17723345454 或者 邮箱 17723345454@163.com ");
            builder.Property(c => c.ConnectName).IsUnicode().HasMaxLength(BXJGUtilsConsts.FeedbackConnectNameMaxLength).HasComment("称呼/姓名");

            builder.HasIndex(c => c.CreationTime);
        }
    }
}