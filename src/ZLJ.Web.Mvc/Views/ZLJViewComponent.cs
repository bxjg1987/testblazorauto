using Abp.AspNetCore.Mvc.ViewComponents;

namespace ZLJ.Web.Views
{
    public abstract class ZLJViewComponent : AbpViewComponent
    {
        protected ZLJViewComponent()
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
        }
    }
}
