using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;

namespace System.Net.Http
{
    public static class SystemNetHttpExt
    {
      static  JsonSerializerSettings settings => BXJG.Utils.Application.Share.Consts.settings;
        public static async Task<T> Get<T>(this HttpClient hc, string url, object ps = default, CancellationToken cancellationToken = default)
        {
            //
            url = url.AddQueryString(ps);

            // Console.WriteLine(  System.Text.Json.JsonSerializer.Serialize(input));
            // Console.WriteLine(  url);

            var str = await hc.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(str, settings);
            //var x = CreateHttpClient().GetFromJsonAsync<T>(url, cancellationToken);
            //Console.WriteLine("返回对象");
            //try
            //{
            //    Console.WriteLine(x.GetValue<int>("totalCount"));
            //}
            //catch 
            //{
            //}

            //return x;
        }

        public static async Task<T> Post<T>(this HttpClient hc, string url, object ps = default, object qs = default, CancellationToken cancellationToken = default)
        {
            url = url.AddQueryString(qs);
            await Console.Out.WriteLineAsync("url");
            await Console.Out.WriteLineAsync(   url);
            var r = await hc.PostAsJsonAsync(url, ps, cancellationToken);
            var str = await r.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str, settings);
        }
        public static async Task Post(this HttpClient hc, string url, object ps = default, object qs = default, CancellationToken cancellationToken = default)
        {
            url = url.AddQueryString(qs);

            await hc.PostAsJsonAsync(url, ps, cancellationToken);

        }
    }
}
