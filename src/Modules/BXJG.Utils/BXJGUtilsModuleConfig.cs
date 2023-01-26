using Abp.Domain.Uow;
using BXJG.Utils.Enums;
using DotNetCore.CAP;
using DotNetCore.CAP.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils
{
    public class BXJGUtilsModuleConfig
    {
        //EnumLocalizationContainer enumLocalizationContainer;

        //public BXJGUtilsModuleConfig(EnumLocalizationContainer enumLocalizationContainer)
        //{
        //    this.enumLocalizationContainer = enumLocalizationContainer;
        //}

        ////最终处理办法是属性类型直接用List 而不是IList
        //[Obsolete("需要访问此对象时应直接注入EnumLocalizationContainer")]
        //public IReadOnlyList<EnumLocalizationDefine> Enums => enumLocalizationContainer;

        /// <summary>
        /// 通过它注册本地化枚举定义
        /// </summary>
        public ICollection<Func<IEnumerable<EnumLocalizationDefine>>> EnumLocalizationProviders { get; internal set; } = new List<Func<IEnumerable<EnumLocalizationDefine>>>();
        public Func<IDispatcher, IActiveUnitOfWork, ICapTransaction> wt;

        public BXJGUtilsModuleConfig()
        {
            //诡异的问题，在这里初始化后，  调用方从容器中获取或注入的单例对象的Enums是一个数组
            //Enums = new List<EnumConfigItem>();

        }
    }
}
