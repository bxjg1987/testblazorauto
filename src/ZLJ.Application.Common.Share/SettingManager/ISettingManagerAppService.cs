using Abp.Application.Services;
using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.Share.SettingManager
{
    /// <summary>
    /// abp设置管理
    /// </summary>
    public interface ISettingManagerAppService : IApplicationService, ISettingManager
    {
       
    }
}
