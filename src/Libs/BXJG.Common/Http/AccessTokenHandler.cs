using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Common.Http
{
    /// <summary>
    /// 引用此库的项目应提供实现，并注入到ioc容器，同时它还应该处理accessToken的更新
    /// </summary>
    public interface IAccessTokenProvider
    {
        string GetAccessToken();
        string GetEncryptedAccessToken();
    }

    public class NullAccessTokenProvider : IAccessTokenProvider
    {
        public string GetAccessToken()
        {
            return null;
        }

        public string GetEncryptedAccessToken()
        {
            return null;
        }
    }

    /// <summary>
    /// 为http请求设置accesstoken的http header
    /// </summary>
    public class AccessTokenHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider accessTokenProvider;

        //这里去做 accessToken的滑动过期

        // public Func<string> AccessTokenProvider { get; set; }
        public AccessTokenHandler(IAccessTokenProvider accessTokenProvider)
        {
            this.accessTokenProvider = accessTokenProvider;
           // this.nxhh = nxhh;
        }
        //protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    sdfdsf(request);
        //    return base.Send(request, cancellationToken);
        //}
        //dic

        ///// <summary>
        ///// 负载均衡场景中，粘性会话id的容器
        ///// key 粘性会话键的名称，value 粘性会话值
        ///// </summary>
        //nxhhrq nxhh ;
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            sdfdsf(request);

            /*
             * 粘性会话对于前端处理来说是个麻烦的事情，所以这里只是粗糙的设计
             * 
             * 后端的粘性会话是每个群集对于一个，而一个前端可能访问多个群集
             * 具体访问哪个是后端的路由决定的，后端的路由规则可能很复杂
             * 难道前端也要写一样的规则，来觉得处理哪个粘性会话吗？
             * 
             * 所以这里粗暴点，全部设置，后端返回的粘性会话值是经过加密的，所以也没啥安全问题，只是比较浪费
             * 
             * 推荐后端粘性会话名称 ak-群集id-会话类型
             * 
             * 若这个库 不上调用我们的后端，那么别人的粘性会话可能不按我们这里的规则命名
             * 此时此库的调用方应该再自己定义一个DelegatingHandler来处理，可以重命名为我们的规则，或者自己处理粘性会话
             * 
             * 包括别人的会话如果没加密，也应该自定义DelegatingHandler
             * 
             * 在blazor assembly中，全局静态无所谓
             * 
             * 在blazor server时，不同用户的会员会被覆盖，所以存储粘性会话的容器应该特别处理。
             * 
             * 统一以scope方式注入到ioc比较合理，注意blazor server需要使用inject
             * 
             */



            /////// 搞错了， api木有必要用粘性会话 signalR才需要，问ai

            //foreach (var key in nxhh.Keys)
            //{
            //    if (nxhh[key].IsNotNullOrWhiteSpaceBXJG())
            //        request.Headers.Add(key, nxhh[key]);
            //}
            var r = await base.SendAsync(request, cancellationToken);
            //foreach (var item in r.Headers.Where(x => x.Key.StartsWith("ak-", StringComparison.OrdinalIgnoreCase)))
            //{
            //    if (item.Value != default && item.Value.Any())
            //        nxhh.TryAdd(item.Key, item.Value.First());
            //}

            return r;
        }

        void sdfdsf(HttpRequestMessage request)
        {
            var token = accessTokenProvider.GetAccessToken();
            //Console.WriteLine($"请求前设置accessToken：{token}");
            if (token.IsNotNullOrWhiteSpaceBXJG())
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        }
    }

    //public class nxhhrq : ConcurrentDictionary<string, string> { }
}
