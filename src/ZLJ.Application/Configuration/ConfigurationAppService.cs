using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ZLJ.Application.Configuration.Dto;
using ZLJ.Core.Configuration;

namespace ZLJ.Application.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : AdminBaseAppService, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
