using Abp.UI;
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Application.Common.ClientProxy.Http
{
    /// <summary>
    /// 解包abp的包装，若成功则返回被包装的数据，否则抛出UserFriendlyException
    /// </summary>
    public class AbpWraperDelegatHandler : DelegatingHandler
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jg = base.Send(request, cancellationToken);
            var r = jg.Content.ReadFromJsonAsync<AjaxResponse>(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
            cl(jg, r);
            return jg;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var jg = await base.SendAsync(request, cancellationToken);
            var r =await jg.Content.ReadFromJsonAsync<AjaxResponse>(cancellationToken);
            cl(jg, r);
            return jg;
        }

        private void cl(HttpResponseMessage jg, AjaxResponse r)
        {
            if (!r.Success)
                throw new UserFriendlyException(r.Error.Code, r.Error.Message, r.Error.Details);
            jg.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(r.Result));
        }
    }
}
