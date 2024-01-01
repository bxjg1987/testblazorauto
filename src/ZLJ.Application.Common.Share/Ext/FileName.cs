using Abp.Application.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;

namespace Abp.Application.Navigation
{
    /*
     * 考虑到非blazor项目也需要，因此定义在此项目中
     */

    public static class FileName
    {
        /// <summary>
        /// 递归向下
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="items"></param>
        /// <param name="del">递归向下时的回调，返回true则继续递归向下，否则停止递归</param>
        public static bool RecursionDown(this IList<UserMenuItem> items, Func<UserMenuItem, UserMenuItem, bool> del, UserMenuItem parent = default)
        {
            foreach (var item in items)
            {
                var r = del(parent, item);
                if (r)
                {
                    if (item.Items != default)
                    {
                        var x = RecursionDown(item.Items, del, item);
                        if (x==false)
                            return false;
                    }
                }
                else
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 递归向上，记得使用RecursionWrapParent
        /// </summary>
        /// <param name="item"></param>
        /// <param name="del">true继续，false停止递归</param>
        public static bool RecursionUp(this UserMenuItem item, Func<UserMenuItem, bool> del)
        {
            var p = item.GetParent();
            if (p == default)
                return false;

            var r = del(p);
            if (r)
            { 
                var x = RecursionUp(p, del);
                if (x == false)
                    return false;
            }
            //else
            //    return;
            return true;
        }
        /// <summary>
        /// 获取父节点的引用，记得先调用RecursionWrapParent
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static UserMenuItem GetParent(this UserMenuItem item)
        {
            return (item.CustomData as InnerWraper)?.Parent;
        }
        /// <summary>
        /// 递归地建立父子关系，木有使用深度克隆，用完后记得解包
        /// </summary>
        /// <param name="items"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IDisposable RecursionWrapParent(this IList<UserMenuItem> items, UserMenuItem parent = default)
        {
            items.RecursionDown((p, m) =>
            {
                var temp = new InnerWraper
                {
                    Old = m.CustomData,
                    Parent = p
                };
                m.CustomData = temp;
                return true;
            }, parent);
            return new Yx(parent, items);
        }

        class InnerWraper
        {
            public UserMenuItem Parent { get; set; }
            public object Old { get; set; }
        }
        class Yx : IDisposable
        {
            IList<UserMenuItem> items;
            UserMenuItem parent;

            public Yx(UserMenuItem parent, IList<UserMenuItem> items)
            {
                this.parent = parent;
                this.items = items;
            }

            public void Dispose()
            {
                items.RecursionDown((p, m) =>
                {
                    var temp = m.CustomData as InnerWraper;
                    m.CustomData = temp.Old;
                    return true;
                }, parent);
            }
        }
    }
}
