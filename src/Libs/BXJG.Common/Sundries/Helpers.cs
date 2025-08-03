using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BXJG.Common.Sundries
{
    public class Helpers
    {
        /// <summary>
        /// 获取当前方法的名称
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentMethodName()
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1); // 获取调用GetCurrentMethodName的上一层方法  
            var methodName = stackFrame.GetMethod().Name;
            return methodName;
        }

        public static readonly OSPlatform OSPlatformBrower = OSPlatform.Create("BROWSER");
        /// <summary>
        /// 判断当前代码是否运行在浏览器中
        /// .NET Standard 专用，.net5+请使用 OperatingSystem.IsBrower
        /// </summary>
        /// <returns></returns>
        public static bool IsBrower()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatformBrower);
        }



        /// <summary>
        /// 为URL添加单个查询参数
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>添加参数后的URL</returns>
        public static string AddQueryString(string url, string key, string value)
        {
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
                return url;

            // 检查是否已经有查询字符串
            var separator = url.Contains('?') ? "&" : "?";

            // 对键值进行URL编码
            var encodedKey = Uri.EscapeDataString(key);
            var encodedValue = value != null ? Uri.EscapeDataString(value) : string.Empty;

            return $"{url}{separator}{encodedKey}={encodedValue}";
        }

        /// <summary>
        /// 为URL添加多个查询参数
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="parameters">参数字典</param>
        /// <returns>添加参数后的URL</returns>
        public static string AddQueryString(string url, IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return url;

            var result = url;
            foreach (var param in parameters)
            {
                result = AddQueryString(result, param.Key, param.Value);
            }

            return result;
        }

        /// <summary>
        /// 为URL添加多个查询参数（可变参数形式）
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="parameters">参数键值对</param>
        /// <returns>添加参数后的URL</returns>
        public static string AddQueryString(string url, params KeyValuePair<string,string>[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return url;

            var result = url;
            foreach (var (key, value) in parameters)
            {
                result = AddQueryString(result, key, value);
            }

            return result;
        }

    }
}
