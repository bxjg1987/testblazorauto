using System.Threading.Tasks;
using ZLJ.Application.Admin.Configuration.Dto;

namespace ZLJ.Application.Admin.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
