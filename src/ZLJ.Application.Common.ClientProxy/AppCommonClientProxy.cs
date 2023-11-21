using System.Net.Http.Json;
using ZLJ.Application.Common.Share.Models.TokenAuth;

namespace ZLJ.Application.Common.ClientProxy
{
    /*
     * 不同应用要么继承，要么用两个客户端代理
     * 
     * 比如AppCommonClientProxy访问公共应用服务
     * AdminClientProxy访问后台管理的应用服务
     * 
     * 要不要让AdminClientProxy继承AppCommonClientProxy，这样调用方能简单点
     * 
     * 后台服务是实现，逻辑代码比较多，所以是每个应用服务分开定义的
     * 
     * 前端代理比较简单，所以就放一起算了
     * 
     */

    /// <summary>
    /// 对后端公共api的封装
    /// 单例
    /// accesstoken自动滑动过期
    /// </summary>
    public class AppCommonClientProxy
    {
        protected HttpClient hc => HttpClientInstance.Instance;

        public async Task<AuthenticateResultModel> Authenticate(AuthenticateModel model, CancellationToken cancellationToken = default)
        {
            //hc=new HttpClient(new HttpClientHandler)
           
            var r = await hc.PostAsJsonAsync("", model, cancellationToken);
            return await r.Content.ReadFromJsonAsync<AuthenticateResultModel>();
        }
    }
}