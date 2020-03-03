using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ZLJ.Configuration.Dto;

namespace ZLJ.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ZLJAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
