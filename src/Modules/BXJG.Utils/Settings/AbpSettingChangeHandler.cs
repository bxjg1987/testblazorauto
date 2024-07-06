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
    public class AbpSettingChangeHandler : IAsyncEventHandler<EntityChangingEventData<Abp.Configuration.Setting>>, ITransientDependency
    {
        public async Task HandleEventAsync(EntityChangingEventData<Setting> eventData)
        {
            await Zhongjie.Instance.Chufa(Share.BXJGUtilsConsts.OnAbpApplicationSettingsChanged);
        }
    }
}