using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BXJG.WeChat.Pay.Entities;
using System.IO;
using BXJG.Common;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BXJG.WeChat.Pay
{
    /// <summary>
    /// 默认的微信支付平台证书提供器<br/>
    /// 参考文档：<see cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/wei-xin-zhi-fu-ping-tai-zheng-shu-geng-xin-zhi-yin" />
    /// </summary>
    public class WXCertificateDefaultProvider : IWXCertificateProvider
    {
        ///// <summary>
        ///// 微信支付平台的证书更新时使用的同步锁
        ///// </summary>
        //object locker = new object();
        /// <summary>
        /// 微信平台证书获取接口返回的原始数据
        /// </summary>
        WXCertificateResult wxCertificateResult;
        /// <summary>
        /// 微信支付模块选项对象
        /// </summary>
        WXPayOption wxPaymentOption;
        /// <summary>
        /// 时钟
        /// 用于获取准确的当前时间
        /// </summary>
        IClock clock;
        private readonly ILogger logger;
        IHttpClientFactory httpClientFactory;
        SecretHelper secretHelper;

        /// <summary>
        /// 存储微信平台证书的文件
        /// </summary>
        string wxCertPath;

        //这里不要定义wxClient变量，因为WXCertificateDefaultProvider本身是单例的，它的变量也是单例的，导致wxClient的单例无效
        //若多个方法都需要使用wxClient，则可以像下面这样定义个属性来简化调用
        //HttpClient wxClient
        //{
        //    get { return wxClientFactory.Get(); }
        //}

        //没有使用默认参数，这样这个类更干净
        //私有的，确保只有内部类WXCertificateDefaultProviderBuilder可以创建此对象的实例
        public WXCertificateDefaultProvider(IOptionsMonitor<WXPayOption> wxPaymentOption,
                                            IHttpClientFactory wxClientFactory,
                                            SecretHelper secretHelper,
                                            IClock clock,
                                            IEnv secureDirectory,
                                            ILogger logger)
        {
            this.wxPaymentOption = wxPaymentOption.CurrentValue;
            this.httpClientFactory = wxClientFactory;
            this.clock = clock;
            this.logger = logger;
            this.secretHelper = secretHelper;
            this.wxCertPath = Path.Combine(secureDirectory.SecureDirectory,"wx", "wxpaycert.json");
        }
        //private string sfd() {
        //    return wxPaymentOption.ApiV3SecretKey;
        //}
        public async Task InitAsync()
        {
            var txt = await File.ReadAllTextAsync(wxCertPath);
            wxCertificateResult = JsonSerializer.Deserialize<WXCertificateResult>(txt);
            wxCertificateResult.data.Select(c => c.cert = secretHelper.AesGcmDecrypt(c.encrypt_certificate.associated_data, c.encrypt_certificate.nonce, c.encrypt_certificate.ciphertext));
            //默认使用全局静态配置，所以这里暂时不配置
            //foreach (var item in wxCertificateResult.data)
            //{
            //    item.apiV3Key = sfd;
            //}
            //var mw = wxCertificateResult.data[0].cert.Value;//测试解密证书正常


            //定时任务检查微信支付平台证书
            var t = new Task(async () =>
            {
                while (true)
                {

                    try
                    {
                        var xzs = wxCertificateResult.data.OrderBy(c => c.effective_time).First();//获取最新的证书
                        var now = await clock.GetNowAsync();
                        if (xzs.expire_time.AddDays(-9) <= now)
                            await UpdateCertAsync();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex,"微信支付证书更新失败！");
                        //记录日志、触发事件等
                    }
                    await Task.Delay(1000 * 60 * 60 * 23);
                }
            }, TaskCreationOptions.LongRunning);
            t.Start();
        }
        /// <summary>
        /// 获取有效的微信支付平台证书
        /// </summary>
        /// <param name="wechatPaySerial">证书的序号</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask<string> GetAsync(string wechatPaySerial, CancellationToken cancellationToken = default)
        {
            //var token = cancellationToken == null ? CancellationToken.None : cancellationToken.Value;
            var now = await clock.GetNowAsync();
            //if (!wxCertificateResult.data.Any(c => c.serial_no == wechatPaySerial))
            //    await UpdateCertAsync(cancellationToken);
            var zs = wxCertificateResult.data.Single(c => c.serial_no == wechatPaySerial);
            if (zs == null)
                throw new Exception("未找到证书" + wechatPaySerial + "！");
            if (zs.effective_time > now)
                throw new Exception("证书" + wechatPaySerial + "尚未生效！");
            if (zs.expire_time <= now)
                throw new Exception("证书" + wechatPaySerial + "已过期！");
            return zs.cert;
        }
        private async Task UpdateCertAsync(CancellationToken cancellationToken = default)
        {
            var temp = await GetCertAsync(cancellationToken);
            var str = JsonSerializer.Serialize(temp);
            await File.WriteAllTextAsync(wxCertPath, str);
            temp.data.Select(c => c.cert = secretHelper.AesGcmDecrypt(c.encrypt_certificate.associated_data, c.encrypt_certificate.nonce, c.encrypt_certificate.ciphertext));
            this.wxCertificateResult = temp;
        }
        //IWXcertificateProvider的不同实现类可能都需要此方法，可以、但暂时没有进行封装
        /// <summary>
        /// 调用微信接口 获取 微信支付平台证书
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<WXCertificateResult> GetCertAsync(CancellationToken cancellationToken = default)
        {
            //api路径基地址在httpClient中设置
            //var apiUrl = "https://api.mch.weixin.qq.com/v3/certificates";
            var apiUrl = "certificates";
            var response = await httpClientFactory.CreateClientPay().GetAsync(apiUrl);
            return await response.Content.ReadAsAsync<WXCertificateResult>(cancellationToken);
        }
    }
}