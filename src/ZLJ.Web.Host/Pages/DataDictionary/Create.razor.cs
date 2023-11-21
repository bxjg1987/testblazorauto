using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Host.DataDictionary
{
    public partial class Create
    {
        protected override string FuncName => "数据字典";

        protected override async Task OnInitializedAsync()
        {
            await base.InitPermission(BXJGUtilsConsts.GeneralTreeCreatePermissionName);
            await base.OnInitializedAsync();
        }
    }
}
