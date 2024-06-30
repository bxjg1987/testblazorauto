using BXJG.Utils.Application.Share.Session;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BXJG.Utils.Application.Share
{
    public class Consts
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions =new  JsonSerializerOptions( JsonSerializerDefaults.Web);

        public const string TongyongLianjie = "TongyongLianjie";

        ///// <summary>
        ///// 配置需要跟后端api匹配
        ///// 整个项目通用的json设置，一直升级过来的，一直使用的是json.net，前后端统一
        ///// </summary>
        //public static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        //{
        //    //// 设置时间格式
        //    //DateFormatString = "yyyy-MM-dd HH:mm:ss",

        //    // 忽略循环引用
        //    // ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

        //    // 数据格式首字母小写
        //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //    //ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
        //    //{
        //    //    NamingStrategy = new CamelCaseNamingStrategy()
        //    //}
        //    // 数据格式按原样输出
        //    // ContractResolver = new DefaultContractResolver(),

        //    // 忽略空值
        //    //  NullValueHandling = NullValueHandling.Ignore
        //};

    }
}
