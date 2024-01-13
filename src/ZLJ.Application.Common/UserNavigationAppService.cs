using Abp;
using Abp.Application.Navigation;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common
{
    public class UserNavigationAppService : CommonBaseAppService//, IUserNavigationManager
    {
        IUserNavigationManager _userNavigationManager;
        IAbpSession abpSession;
        public UserNavigationAppService(IUserNavigationManager userNavigationManager, IAbpSession abpSession)
        {
            _userNavigationManager = userNavigationManager;
            this.abpSession = abpSession;
        }

        public async Task<UserMenu> GetMenuAsync([FromQuery] string menuName)
        {
            if (!abpSession.UserId.HasValue)
                return new UserMenu
                {
                    Items = new List<UserMenuItem>()
                };
            return await _userNavigationManager.GetMenuAsync(menuName, new UserIdentifier(abpSession.TenantId, abpSession.UserId.Value));
        }

        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync()
        {
            if (!abpSession.UserId.HasValue)
                return new List <UserMenu>();

            return await _userNavigationManager.GetMenusAsync(new UserIdentifier(abpSession.TenantId, abpSession.UserId.Value));
        }
    }
}
