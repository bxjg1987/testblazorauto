using System.Net.Http.Json;
using ZLJ.Application.Common.Share.Models.TokenAuth;

namespace ZLJ.Application.Common.ClientProxy
{
    /// <summary>
    /// 对后端公共api的封装
    /// </summary>
    public class AppCommonClientProxy
    {
        /*
         * 使用类型和客户端
         */
        protected readonly HttpClient httpClient;

        public AppCommonClientProxy(HttpClient client)
        {
            this.httpClient = client;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AuthenticateResultModel> Authenticate(AuthenticateModel model, CancellationToken cancellationToken = default)
        {
            var r = await httpClient.PostAsJsonAsync("", model, cancellationToken);
            return await r.Content.ReadFromJsonAsync<AuthenticateResultModel>();
        }
    }
}