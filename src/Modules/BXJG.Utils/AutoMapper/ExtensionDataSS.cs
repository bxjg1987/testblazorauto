using Abp.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.AutoMapper
{
    public class ExtensionDataSS<TSource, TDestination> : IValueResolver<TSource, TDestination, Dictionary<string, object>>
        where TSource : IExtendableObject
        where TDestination : IExtendableDto
    {
        public Dictionary<string, object> Resolve(TSource source, TDestination destination, Dictionary<string, object> destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.ExtensionData))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(source.ExtensionData);
            return new Dictionary<string, object>();
        }
    }
    public class ExtensionDataSS : ExtensionDataSS<IExtendableObject, IExtendableDto>
    {
    }
    public static class cvx
    {
        public static IMappingExpression<TSource, TDestination> MapExtensionData<TSource, TDestination>(this IMappingExpression<TSource, TDestination> map)
            where TSource : IExtendableObject
            where TDestination : IExtendableDto
        {
            return map.ForMember(c => c.ExtensionData, c => c.MapFrom<ExtensionDataSS<TSource, TDestination>>());
        }
    }

}
