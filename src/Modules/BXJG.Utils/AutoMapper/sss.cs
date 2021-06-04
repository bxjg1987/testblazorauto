using Abp.Domain.Entities;
using AutoMapper;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.AutoMapper
{
    public class sss : IMemberValueResolver<IExtendableObject, IExtendableDto, string, Dictionary<string, object>>
    {
        public Dictionary<string, object> Resolve(IExtendableObject source, IExtendableDto destination, string sourceMember, Dictionary<string, object> destMember, ResolutionContext context)
        {
            if (!string.IsNullOrWhiteSpace(source.ExtensionData))
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(source.ExtensionData);
            return new Dictionary<string, object>();
        }
    }
}
