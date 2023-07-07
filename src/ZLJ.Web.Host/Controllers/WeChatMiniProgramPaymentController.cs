using BXJG.WeChat.Payment;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLJ.Web.Host.Controllers
{
    public class WeChatMiniProgramPaymentController: ControllerBase
    {
        private readonly WeChatPaymentService weChatPaymentService;

        public WeChatMiniProgramPaymentController(WeChatPaymentService weChatPaymentService)
        {
            this.weChatPaymentService = weChatPaymentService;
        }
        /// <summary>
        /// 统一下单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WeChatPaymentUnifyOrderResultForMiniProgram> UnifyOrder(UnifyOrderInput input)
        {
            //业务处理...各种判断、处理优惠策略、创建本地系统订单等等...

            //创建要向微信提交的订单
            var weChatOrder = this.weChatPaymentService.Create("商品描述", "本地系统订单号", 3.5m);

            //其它可选属性设置
            //weChatOrder.attach = "附件数据";

            //向微信发起统一支付
            var rt = await this.weChatPaymentService.PayAsync(weChatOrder, HttpContext.RequestAborted);

            //返回小程序端需要的数据
            return rt.CreateMiniProgramResult();
        }
    }

    public class UnifyOrderInput
    {
        public int ProductId { get; set; }
        public int Count { get; set; }

        //优惠券Id
    }

    public class WeChatPaymentNoticeHandler : IWeChatPaymentNoticeHandler
    {
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
