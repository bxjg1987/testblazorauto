using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.EFCore.ValueConverts
{
    /// <summary>
    /// 数据保护转换器
    /// 这种方式是基于ef，最好的方式还是基于属性的拦截器，在应用层来做
    /// <see href="https://www.tabsoverspaces.com/233774-using-data-protection-in-entity-framework-core-with-value-converters">参考博客</see>
    /// <see href="https://learn.microsoft.com/zh-cn/ef/core/modeling/value-conversions?tabs=data-annotations#built-in-converters">参考官方文档</see>
    /// </summary>
    public class ProtectedConverter : ValueConverter<string, string>
    {
        class Wrapper
        {
            readonly IDataProtector _dataProtector;

            public Wrapper(IDataProtectionProvider dataProtectionProvider)
            {
                _dataProtector = dataProtectionProvider.CreateProtector(nameof(ProtectedConverter));
            }

            public Expression<Func<string, string>> To => x => x != null ? _dataProtector.Protect(x) : null;
            public Expression<Func<string, string>> From => x => x != null ? _dataProtector.Unprotect(x) : null;
        }

        public ProtectedConverter(IDataProtectionProvider provider, ConverterMappingHints mappingHints = default)
            : this(new Wrapper(provider), mappingHints)
        { }

        ProtectedConverter(Wrapper wrapper, ConverterMappingHints mappingHints)
            : base(wrapper.To, wrapper.From, mappingHints)
        { }
    }

    /// <summary>
    /// 用不用是你的事，基础设施还是要提供这个
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ProtectedAttribute : Attribute
    { }

   
}
