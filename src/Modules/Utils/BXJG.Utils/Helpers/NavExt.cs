using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Application.Navigation
{
    public static class NavExt
    {
        /// <summary>
        /// 递归向下查找
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="dxx"></param>
        /// <returns></returns>
        public static IHasMenuItemDefinitions? RecursionFindDown(this IHasMenuItemDefinitions item, string name, StringComparison dxx = StringComparison.OrdinalIgnoreCase)
        {

            foreach (var item1 in item.Items)
            {
                if (item1.Name.Equals(name, dxx))
                    return item1;

                var r = item1.RecursionFindDown(name, dxx);
                if (r != default)
                    return r;
            }

            return default;
        }
    }
}
