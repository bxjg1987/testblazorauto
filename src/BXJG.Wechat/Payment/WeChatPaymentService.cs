using BXJG.WeChat.MiniProgram;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BXJG.WeChat.Payment.WeChatPaymentUnifyOrderResult;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 微信小程序支付服务类
    /// </summary>
    public class WeChatPaymentService //貌似没啥必要抽象一个接口出来
    {
        #region 字段和属性
        /// <summary>
        /// 微信小程序登录的选项对象，内部包含appId
        /// </summary>
        private readonly MiniProgramAuthenticationOptions authOptions;
        /// <summary>
        /// 微信小程序支付的选项对象
        /// </summary>
        private readonly WeChatPaymentOptions paymentOptions;
        /// <summary>
        /// 调用微信小程序支付统一下单返回的结果将封装为一个对象，此对象则是通过这个工厂创建的
        /// </summary>
        private readonly WeChatPaymentUnifyOrderResultFactory weChatPaymentUnifyOrderResultFactory;
        /// <summary>
        /// 按asp.net core 身份验证系统的设计，它的OAuth2的方式会允许Options对象来提供HttpClient，我们这里为了简单直接注入IHttpClientFactory
        /// 参考官网HttpClientFacotry
        /// </summary>
        private readonly IHttpClientFactory clientFactory;
        /// <summary>
        /// 微信小程序支付过程中涉及到sign签名的帮助类
        /// </summary>
        private readonly WeChatPaymentSecuret securet;
        #endregion

        #region 构造函数
        /// <summary>
        /// 实例化微信小程序支付服务对象
        /// </summary>
        /// <param name="paymentOptions">微信小程序支付的选项对象</param>
        /// <param name="authOptions">微信小程序登录的选项对象，内部包含appId</param>
        /// <param name="weChatPaymentUnifyOrderResultFactory">调用微信小程序支付统一下单返回的结果将封装为一个对象，此对象则是通过这个工厂创建的</param>
        /// <param name="clientFactory">按asp.net core 身份验证系统的设计，它的OAuth2的方式会允许Options对象来提供HttpClient，我们这里为了简单直接注入IHttpClientFactory.参考官网HttpClientFacotry</param>
        /// <param name="securet">微信小程序支付过程中涉及到sign签名的帮助类</param>
        public WeChatPaymentService(
              IOptionsMonitor<WeChatPaymentOptions> paymentOptions,
              IOptionsMonitor<MiniProgramAuthenticationOptions> authOptions,
              WeChatPaymentUnifyOrderResultFactory weChatPaymentUnifyOrderResultFactory,
              IHttpClientFactory clientFactory,
              WeChatPaymentSecuret securet)
        {
            this.paymentOptions = paymentOptions.CurrentValue;
            this.authOptions = authOptions.CurrentValue;
            this.weChatPaymentUnifyOrderResultFactory = weChatPaymentUnifyOrderResultFactory;
            this.clientFactory = clientFactory;
            this.securet = securet;
        }
        #endregion

        #region 私有方法

        #endregion

        #region 公共方法
        /// <summary>
        /// 调用微信发起统一下单
        /// 在此之前你应该调用Create方法来创建要提交的数据（参数data）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Obsolete("考虑其它重载")]
        public async Task<WeChatPaymentUnifyOrderResult> PayAsync(WeChatPaymentUnifyOrderInput data, CancellationToken cancellationToken = default)
        {
            var client = clientFactory.CreateClient(WeChatPaymentConsts.HttpClientName);
            var str = data.ToXml();
            var ct = new StringContent(str);
            var response = await client.PostAsync(paymentOptions.UnifyOrderUrl, ct, cancellationToken);
            return await weChatPaymentUnifyOrderResultFactory.LoadAsync(await response.Content.ReadAsStreamAsync(), cancellationToken);
        }

        /// <summary>
        /// 创建统一下单时要准备提交的数据
        /// 参考WeChatPaymentUnifyOrderInputFactory的备注
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="body">商品简单描述，该字段请按照规范传递 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2 </param>
        /// <param name="out_trade_no">商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*且在同一个商户号下唯一 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2 </param>
        /// <param name="total_fee">订单总金额，单位为分 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2 </param>
        /// <returns></returns>
        [Obsolete("请直接调用PayAsync方法")]
        public WeChatPaymentUnifyOrderInput Create(
            string body,
            string out_trade_no,
            decimal total_fee)
        {
            return new WeChatPaymentUnifyOrderInput(
                securet,
                authOptions.AppId,
                paymentOptions.mch_id,
                paymentOptions.notify_url,
                paymentOptions.ip,
                body,
                out_trade_no,
                total_fee);
        }
        
        //没有将方法的参数封装为对象，因为这样调用方用起来更简单
        /// <summary>
        /// 向微信的小程序支付服务发起预支付
        /// 参考：https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=9_1&index=1
        /// </summary>
        /// <param name="body">商品简单描述，该字段请按照规范传递，具体请见参数规定 参考 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="total_fee">金额</param>
        /// <param name="device_info">设备号 自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"</param>
        /// <param name="detail">商品详细描述，对于使用单品优惠的商户，该字段必须按照规范上传， https://pay.weixin.qq.com/wiki/doc/api/danpin.php?chapter=9_102&index=2</param>
        /// <param name="attach">自定义的附加信息，将来微信推送支付结果通知时会原样返回给我们</param>
        /// <param name="time_start">交易开始时间，默认为当前时间</param>
        /// <param name="time_expire">交易结束时间，默认为交易开始时间的5分钟后</param>
        /// <param name="product_id">商品ID，trade_type=NATIVE时，此参数必传。此参数为二维码中包含的商品ID，商户自行定义。</param>
        /// <param name="limit_pay">支付方式，上传此参数no_credit--可限制用户不能使用信用卡支付</param>
        /// <param name="openid">用户与小程序之间建立的唯一id</param>
        /// <param name="receipt">电子发票入口开放标识，传入Y时，支付成功消息和支付详情页将出现开票入口。需要在微信支付商户平台或微信公众平台开通电子发票功能，传此字段才可生效</param>
        /// <param name="scene_info">场景信息，该字段常用于线下活动时的场景信息上报，支持上报实际门店信息，商户也可以按需求自己上报相关信息。该字段为JSON对象数据，对象格式为{"store_info":{"id": "门店ID","name": "名称","area_code": "编码","address": "地址" }} ，字段详细说明请点击行前的+展开</param>
        /// <returns>返回结果中包含前端小程序发起正式支付需要的数据</returns>
        public async Task<WeChatPaymentUnifyOrderResult> PayAsync(
            string body,
            string out_trade_no,
            decimal total_fee,
            string device_info = default,
            string detail = default,
            string attach = default,
            DateTime? time_start = default,
            DateTime? time_expire = default,
            string product_id = default,
            string limit_pay = default,
            string openid = default,
            string receipt = default,
            string scene_info = default, 
            CancellationToken cancellationToken = default)
        {
            //由于微信开发中多个地方都会使用统一下单，因此使用了一个通用的模型
            //目前这里开发的小程序支付完全可以不使用这个模型，而直接在这里的代码中直接构建一个xml格式的数据进行提交
            var data = new WeChatPaymentUnifyOrderInput(
                securet,
                authOptions.AppId,
                paymentOptions.mch_id,
                paymentOptions.notify_url,
                paymentOptions.ip,
                body,
                out_trade_no,
                total_fee)
            {
                device_info = device_info,
                detail=detail,
                attach = attach,
                time_start= time_start,
                time_expire= time_expire,
                product_id=product_id,
                limit_pay = limit_pay,
                openid=openid,
                receipt = receipt,
                scene_info = scene_info
            };
            var str = data.ToXml();
            var client = clientFactory.CreateClient(WeChatPaymentConsts.HttpClientName);
            var ct = new StringContent(str);
            var response = await client.PostAsync(paymentOptions.UnifyOrderUrl, ct, cancellationToken);
            return await weChatPaymentUnifyOrderResultFactory.LoadAsync(await response.Content.ReadAsStreamAsync(), cancellationToken);
        }

        /*
         * 主动查询订单、关闭订单、申请退款.....等接口将陆续添加
         * https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=9_2
         */
        #endregion
    }
}
