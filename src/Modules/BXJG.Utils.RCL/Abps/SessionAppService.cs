using BXJG.Utils.RCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Session;

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
            await appContainer.T1;
            return appContainer.CurrentLoginInformations;
        }
    }
}
