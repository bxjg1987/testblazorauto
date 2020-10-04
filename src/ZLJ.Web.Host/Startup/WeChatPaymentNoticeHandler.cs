using Abp.Dependency;
using BXJG.Shop.Sale;
using BXJG.WeChat.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLJ.Web.Host.Startup
{
    /// <summary>
    /// 微信小程序支付结果通知处理器
    /// </summary>
    public class WeChatPaymentNoticeHandler : IWeChatPaymentNoticeHandler, ISingletonDependency
    {
        /// <summary>
        /// 用来处理微信结果通知的应用服务
        /// </summary>
        private readonly IOrderAppService orderAppService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderAppService">用来处理微信结果通知的应用服务</param>
        public WeChatPaymentNoticeHandler(IOrderAppService orderAppService)
        {
            this.orderAppService = orderAppService;
        }

        //这里可以做你想要的依赖注入
        public async Task PaymentNoticeAsync(WeChatPaymentNoticeContext context)
        {
            //这里可以通过context拿到微信提交过来的数据
            //你可以做业务处理
            //特别需要注意的是：你必须考虑并发问题
            //若业务处理失败 请抛出异常，否则无需任何处理
            await Task.CompletedTask;
        }
    }
}
