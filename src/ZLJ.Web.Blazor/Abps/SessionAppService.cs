using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Application.Common.Share.Session;

namespace ZLJ.Web.Blazor.Abps
{
    public class SessionAppService : ISessionAppService
    {
        AppContainer appContainer;

        public SessionAppService(AppContainer appContainer)
        {
            this.appContainer = appContainer;
        }

        public Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            return appContainer.CurrentLoginInformations;
        }
    }
}
