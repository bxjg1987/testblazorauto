using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Utils.Feedback;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore
{
    //用这种方式 会污染 模板项目，可能人家有自己的父类，这样强迫另一个项目必须继承BXJGUtilsDbContext不好
    //但这里只是提供默认的，你任然可以继承之前的，然后手动添加DbSet<FeedbackEntity> 
    public abstract class BXJGUtilsDbContext<TTenant, TRole, TUser, TSelf> : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
   where TTenant : AbpTenant<TUser> where TRole : AbpRole<TUser> where TUser : AbpUser<TUser> where TSelf : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
    {
        public virtual DbSet<FeedbackEntity> Feedbacks { get; set; }
        protected BXJGUtilsDbContext(DbContextOptions<TSelf> options) : base(options)
        {
        }
    }
}
