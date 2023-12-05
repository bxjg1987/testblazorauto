using System;
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
    }
    /// <summary>
    /// 自动处理accessToken
    /// </summary>
    public class AccessTokenHandler : DelegatingHandler
    {
        private readonly IAccessTokenProvider accessTokenProvider;

        //这里去做 accessToken的滑动过期

        // public Func<string> AccessTokenProvider { get; init; }
        public AccessTokenHandler(IAccessTokenProvider accessTokenProvider)
        {
            this.accessTokenProvider = accessTokenProvider;
        }
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            sdfdsf(request);
            return base.Send(request, cancellationToken);
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            sdfdsf(request);
            return base.SendAsync(request, cancellationToken);
        }

        void sdfdsf(HttpRequestMessage request)
        {
            var token = accessTokenProvider.GetAccessToken();

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer ", token);

        }
    }
}
