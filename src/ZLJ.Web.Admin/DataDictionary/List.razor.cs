using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.DataDictionary
{
    public partial class List
    {
        protected override string FuncName => "数据字典";

        protected override async Task OnInitialized2Async()
        {
            await InitPermission(BXJGUtilsConsts.GeneralTreeCreatePermissionName, BXJGUtilsConsts.GeneralTreeUpdatePermissionName, BXJGUtilsConsts.GeneralTreeDeletePermissionName);
            await base.OnInitialized2Async();
        }
    }
}
