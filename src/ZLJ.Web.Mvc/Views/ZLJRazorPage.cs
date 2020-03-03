using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace ZLJ.Web.Views
{
    public abstract class ZLJRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected ZLJRazorPage()
        {
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
        }
    }
}
