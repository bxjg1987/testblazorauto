using BXJG.Utils.Application.Share.Session;
using BXJG.Utils.RCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.RCL.Abps
{
    public class SessionAppService : ISessionAppService
    {
        AppContainer appContainer;

        public SessionAppService(AppContainer appContainer)
        {
            this.appContainer = appContainer;
        }

        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            if (appContainer.T1 != default)
            {
                await appContainer.T1;
                return appContainer.CurrentLoginInformations;
            }
            return default;
        }
    }
}
