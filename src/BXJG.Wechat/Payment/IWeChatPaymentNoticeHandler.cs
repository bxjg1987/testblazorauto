using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 当微信用户支付成功后，微信会回调我们的接口以通知我们：有用户支付成功了...
    /// 支付结果通知中间件(WeChatPaymentNoticeMiddleware)将拦截请求，然后解析微信提交过来的数据，然后调用此接口进行业务处理
    /// 您应该实现此接口来完成自己的业务处理，并在配置时指定你的类型（本质上是将你的类型注册到ioc容器）
    /// </summary>
    public interface IWeChatPaymentNoticeHandler
    {
        Task PaymentNoticeAsync(WeChatPaymentNoticeContext context);
    }
}
