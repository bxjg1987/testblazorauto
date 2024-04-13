using Abp.Application.Features;
using BXJG.Utils.RCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BXJG.Utils.RCL.Abps
{
    /*
     * 设计思路同setting，客户端只读取，设置部分有blazor server页面完成，保存是刷新页面以便客户端拿到新的值
     */

    public class ClientFeatureChecker : IFeatureChecker
    {
        AppContainer _appContainer;

        public ClientFeatureChecker(AppContainer appContainer)
        {
            _appContainer = appContainer;
        }
        public string GetValue(string name)
        {
            return _appContainer.AbpUserConfiguration.Features.AllFeatures[name].Value;
        }

        public string GetValue(int tenantId, string name)
        {
            return GetValue(name);
        }

        public Task<string> GetValueAsync(string name)
        {
            return Task.FromResult(GetValue(name));
        }

        public Task<string> GetValueAsync(int tenantId, string name)
        {
            return GetValueAsync(name);
        }

        public bool IsEnabled(string featureName)
        {
            return bool.Parse(GetValue(featureName));
        }

        public bool IsEnabled(int tenantId, string featureName)
        {
            return IsEnabled(featureName);
        }

        public Task<bool> IsEnabledAsync(string featureName)
        {
            return Task.FromResult(IsEnabled(featureName));
        }

        public Task<bool> IsEnabledAsync(int tenantId, string featureName)
        {
            return IsEnabledAsync(featureName);
        }
    }
}
