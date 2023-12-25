using System.Threading.Tasks;
using ZLJ.App.Admin.Configuration.Dto;

namespace ZLJ.App.Admin.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
