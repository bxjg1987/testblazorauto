using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Application.Share
{
    public class Consts
    {   /// <summary>
        /// 配置需要跟后端api匹配
        /// </summary>
        public static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            //// 设置时间格式
            //DateFormatString = "yyyy-MM-dd HH:mm:ss",

            // 忽略循环引用
            // ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            // 数据格式首字母小写
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
            //{
            //    NamingStrategy = new CamelCaseNamingStrategy()
            //}
            // 数据格式按原样输出
            // ContractResolver = new DefaultContractResolver(),

            // 忽略空值
            //  NullValueHandling = NullValueHandling.Ignore
        };
    }
}
