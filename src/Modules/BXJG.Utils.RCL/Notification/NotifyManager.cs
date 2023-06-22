using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Notification
{
    /// <summary>
    /// 通知管理组件抽象类
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    public class NotifyManager<TUser, TUserManager, TRole> : AbpComponentBase<TUser, TUserManager, TRole>
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        /// <summary>
        /// 当前消息类别下的未读消息数量
        /// </summary>
        protected int dangqianLeibieWeiduShuliang = 0;
        /// <summary>
        /// 当前选择的通知定义
        /// </summary>
        protected NotifyDefineDto dangqianTongzhiDingyi => tongzhiDingyLiebiao.SingleOrDefault(c => c.Selected);
        /// <summary>
        /// 通知定义列表
        /// </summary>
        protected List<NotifyDefineDto> tongzhiDingyLiebiao = new List<NotifyDefineDto>();
        /// <summary>
        /// 消息列表
        /// </summary>
        protected List<MessageDto> messages = new List<MessageDto>();
        /// <summary>
        /// 过滤条件
        /// </summary>
        protected readonly GetTotalInput Tiaojian = new GetTotalInput();
        /// <summary>
        /// 通知应用服务
        /// </summary>
        [Inject]
        public PersonNotificationAppService<TUser> PersonNotificationAppService { get; set; }

        protected override Task OnInitialized2Async()
        {
            return base.OnInitialized2Async();
        }

        protected virtual async Task LoadCurrent()
        {
            messages = await PersonNotificationAppService.GetAllAsync(new GetAllInput { EndTime = DateTime.Now });
        }
    }
}