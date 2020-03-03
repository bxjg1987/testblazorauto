using System.Threading.Tasks;
using ZLJ.Configuration.Dto;

namespace ZLJ.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
