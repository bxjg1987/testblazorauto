using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BXJGUtilsDIExt
    {
        //https://github.com/volosoft/castle-windsor-ms-adapter/issues/48
        //abp依赖的ioc三方容器不支持.net8的keyed service，这里临时处理下

        //此功能可能会用到前端页面 或后端 公用，所以定义在share项目中

        static readonly ConcurrentDictionary<string, Type> dic = new ConcurrentDictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
     
        public static IServiceCollection AddKeyedServiceBXJG(this IServiceCollection services, string key, Type inter, Type implement, ServiceLifetime serviceLifetime)
        {
            if (dic.TryAdd(key, implement))
                services.Add(ServiceDescriptor.Describe(inter, implement, serviceLifetime));
            return services;
         
        }
        public static IServiceCollection AddKeyedServiceTranBXJG<TService,TImplement>(this IServiceCollection services, string key)
        {
           return services.AddKeyedServiceBXJG(key, typeof(TService), typeof(TImplement), ServiceLifetime.Transient);

        }
        public static TService GetKeyedServiceBXJG<TService>(this IServiceProvider keyedService, string key) 
        {
            var t = dic[key];

            return (TService)keyedService.GetRequiredService(t);
        }
        //public static IEnumerable<TService> GetAllImplementationsBXJG<TService>(this IServiceProvider serviceProvider)
        //{
        //    var serviceType = typeof(TService);
        //    var allDescriptors = serviceProvider.GetService<IEnumerable<ServiceDescriptor>>();

        //    if (allDescriptors == null)
        //        yield break;

        //    foreach (var descriptor in allDescriptors)
        //    {
        //        if (serviceType.IsAssignableFrom(descriptor.ServiceType) || serviceType == descriptor.ServiceType)
        //        {
        //            var service = serviceProvider.GetService(descriptor.ServiceType);
        //            if (service is TService typed)
        //                yield return typed;
        //        }
        //    }
        //}

    }
}
