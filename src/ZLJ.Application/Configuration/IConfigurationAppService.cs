using System.Threading.Tasks;
using ZLJ.Application.Configuration.Dto;

namespace ZLJ.Application.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
