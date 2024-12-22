using Abp.UI;
using Abp.Web.Models;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.ClientProxy.Http
{
    /// <summary>
    /// 解包abp的包装，若成功则返回被包装的数据，否则抛出UserFriendlyException
    /// </summary>
    public class AbpWraperDelegatHandler : DelegatingHandler
    {
        //protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    var jg = base.Send(request, cancellationToken);

        //    var xxx = jg.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        //    var r = JsonConvert.DeserializeObject<AjaxResponse>(xxx,BaseAppServiceClient.settings);

        //    //  var r = jg.Content.ReadFromJsonAsync<AjaxResponse>(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
        //    cl(jg, r);
        //    return jg;
        //}
        private readonly IErrorCallback errorCallback;
        ILogger logger;
        public AbpWraperDelegatHandler(IErrorCallback errorCallback, ILogger<AbpWraperDelegatHandler> logger)
        {
            this.errorCallback = errorCallback;
            this.logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jg = await base.SendAsync(request, cancellationToken);
            var xxx = await jg.Content.ReadAsStringAsync();
            var r = System.Text.Json.JsonSerializer.Deserialize<AjaxResponse>(xxx, Consts.JsonSerializerOptions);
            cl(jg, r, xxx);
            return jg;
        }

        private void cl(HttpResponseMessage jg, AjaxResponse r, string str)
        {
            //单个数据的失败很好处理，抛出异常就行，各应用的ui层全局异常处理即可。
            //批量操作时，目前规定后端返回成功，错误列表中包含错误信息列表。为了方便调用方，这里搞个回调。
            //这样前端可以统一处理 批量操作的错误。

            if (!r.Success)
                throw new UserFriendlyException(r.Error.Code, r.Error.Message, r.Error.Details);

            //string json;// = System.Text.Json.JsonSerializer.Serialize(r.Result, Consts.JsonSerializerOptions);

            if (str.Contains("ids") && str.Contains("errorMessage"))
            {
                //logger.LogDebug("批量操作返回数据，开始解析错误信息"+str + errorCallback.GetType().FullName);
                //try
                //{
                var plcz = System.Text.Json.JsonSerializer.Deserialize<BatchOperationOutputBase>(System.Text.Json.JsonSerializer.Serialize(r.Result, Consts.JsonSerializerOptions), Consts.JsonSerializerOptions);

                //logger.LogDebug("plcz"+ (plcz==null));

                //这样保证调用方使用统一的全局异常处理
                if (plcz.Ids == null || !plcz.Ids.Any())
                {
                    logger.LogDebug("全部失败" + System.Text.Json.JsonSerializer.Serialize(plcz)+str);

                    //这里抛出异常，免得调用方每次都要判断，由于后台接口通常返回成功，所以这里的r.Error为空
                    string message = string.Join("\n", plcz.ErrorMessage.Select(p => p.Message));
                    throw new UserFriendlyException(500, "批量操作全部失败", message);
                }
                if (plcz.ErrorMessage != default && plcz.ErrorMessage.Any())
                {
                   // logger.LogDebug("部分失败");
                    errorCallback.Hand(plcz.ErrorMessage); 
                }
                //}
                //  catch (UserFriendlyException)
                // {
                //     throw;
                //  }
                // catch { 

                //  }
            }

            //  Console.WriteLine(  json);
            //   jg.Content = new StringContent(r.Result);

            jg.Content = JsonContent.Create(r.Result,options: Consts.JsonSerializerOptions);
        }
    }

    public interface IErrorCallback
    {
        void Hand(IEnumerable<BatchOperationErrorMessage> errorInfo);
    }
    public class NullErrorCallback : IErrorCallback
    {
        public void Hand(IEnumerable<BatchOperationErrorMessage> errorInfo)
        {
        }
    }
}
