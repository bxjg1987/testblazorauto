using Abp.Configuration;
using BXJG.Common;
using BXJG.Utils.File;
using Microsoft.Extensions.Configuration;

namespace ZLJ
{
    public class CustomTempFileManager : TempFileManager
    {
        public CustomTempFileManager(IEnv env, ISettingManager settingManager, IConfiguration configuration) : base(env,
            settingManager, configuration)
        {
        }
    }
}