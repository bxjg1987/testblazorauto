using Abp.Configuration;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using BXJG.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Settings
{
    //public class AbpSettingChangeHandler : IAsyncEventHandler<EntityChangedEventData<Abp.Configuration.Setting>>, ITransientDependency
    //{
    //    public async Task HandleEventAsync(EntityChangedEventData<Setting> eventData)
    //    {
    //        //仅关注应用程序级别的配置
    //        if(!eventData.Entity.TenantId.HasValue&& !eventData.Entity.UserId.HasValue)
    //            await Zhongjie.Instance.Chufa(Share.BXJGUtilsConsts.OnAbpApplicationSettingsChanged);
    //    }
    //}
}