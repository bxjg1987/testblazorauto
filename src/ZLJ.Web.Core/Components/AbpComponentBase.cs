using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.Components
{
    /* 由于 BXJG.Utils.AbpComponentBase中的服务都是Lazy的，猜测是不会占用太多内存的，也只是猜测
     * 大不了将来把抽象类中的服务去掉，子类再手动改
     * 
     * 通常我们会在组件中注入服务
     * 而服务的抽象类通常已包含abp常用对象，所以 抽象类中提供的abp相关对象木有太大必要
     * 除非只是想操作下abp提供的对象，而不需要引用服务时才需要
     * 
     * 不用担心这些对象太大，会在blazorserver占用太多资源，因为大部分的对象都是提供功能的，没有太多属性
     * 
     */


    /// <summary>
    /// 当前项目的blazor组件抽象类
    /// </summary>
    public class AbpComponentBase : BXJG.Utils.AbpBaseComponent
    {
        public AbpComponentBase()
        {
            //当前库作为公共rcl库 和 公共ui逻辑 应使用core的本地化和commonApp的本地化，前者是核心公共的，后者是应用层的
            //abp默认只允许设置一种，多出来的得自己做，可以考虑增加一个复合型的本地化管理器
            LocalizationSourceName = ZLJConsts.LocalizationSourceName;
        }
    }
}
