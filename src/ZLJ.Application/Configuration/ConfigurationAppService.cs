using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ZLJ.App.Admin.Configuration.Dto;
using ZLJ.Configuration;

namespace ZLJ.App.Admin.Configuration
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
