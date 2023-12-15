using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.ClientProxy
{
    /*
     * httpclient有两种使用模式，我们选择全局静态
     */

    public class HttpClientInstance
    {
        /// <summary>
        /// 统一的与后端通信的httpclient
        /// </summary>
        public static HttpClient Instance { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Func<DelegatingHandler> func;

        public void Init(Action<HttpClient> act = default)
        {
            DelegatingHandler d;
            var h = new AccessTokenHandler();
            h.InnerHandler = new SocketsHttpHandler();

            if (func != default)
            {
                d = func();
                d.InnerHandler = h;

            }
            else
                d = h;

            Instance = new HttpClient(d);
            if (act != null)
                act(Instance);
        }
    }
}