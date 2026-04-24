using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BXJG.WeChat.Pay.Entities;
using System.IO;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http.Json;
using BXJG.Common.Contracts;

namespace BXJG.WeChat.Pay
{
    /// <summary>
    /// 默认的微信支付平台证书提供器<br/>
    /// 参考文档：<see cref="" href="https://wechatpay-api.gitbook.io/wechatpay-api-v3/qian-ming-zhi-nan-1/wei-xin-zhi-fu-ping-tai-zheng-shu-geng-xin-zhi-yin" />
    /// <br/>历史修复：
    /// <br/>1. 原使用Select解密证书但Select延迟执行导致cert始终为null，已改为foreach强制执行
    /// <br/>2. 原在构造函数中使用new Task启动后台任务，生命周期不受控且无法优雅停止，已拆分为CertificateRefreshService（BackgroundService）
    /// <br/>3. wxCertificateResult字段添加volatile，确保后台线程更新后请求线程立即可见
    /// <br/>4. GetAsync中Single()改为FirstOrDefault()，避免找不到证书时抛出不友好的InvalidOperationException
    /// </summary>
    public class CertificateDefaultProvider : ICertificateProvider
    {
        /// <summary>
        /// 微信平台证书获取接口返回的原始数据
        /// 使用volatile确保后台线程更新后其他线程立即可见
        /// </summary>
        private volatile WXCertificateResult wxCertificateResult;
        /// <summary>
        /// 微信支付模块选项监控器
        /// </summary>
        private readonly IOptionsMonitor<Option> wxPaymentOption;
        /// <summary>
        /// 时钟 用于获取准确的当前时间
        /// </summary>
        private readonly IClock clock;
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger logger { get; set; } = NullLogger.Instance;
        /// <summary>
        /// 微信支付模块使用的HttpClientFactory
        /// </summary>
        private readonly IHttpClientFactory httpClientFactory;
        /// <summary>
        /// 加解密、签名、验签等
        /// </summary>
        private readonly SecretHelper secretHelper;
        /// <summary>
        /// 存储微信平台证书的文件
        /// </summary>
        private readonly string wxCertPath;
        /// <summary>
        /// 实例化WXCertificateDefaultProvider
        /// </summary>
        /// <param name="wxPaymentOption"></param>
        /// <param name="wxClientFactory"></param>
        /// <param name="secretHelper"></param>
        /// <param name="clock"></param>
        /// <param name="secureDirectory"></param>
        /// <param name="logger"></param>
        public CertificateDefaultProvider(IOptionsMonitor<Option> wxPaymentOption,
                                          IHttpClientFactory wxClientFactory,
                                          SecretHelper secretHelper,
                                          IClock clock,
                                          IEnv secureDirectory)
        {
            this.wxPaymentOption = wxPaymentOption;
            this.httpClientFactory = wxClientFactory;
            this.clock = clock;
            this.logger = logger;
            this.secretHelper = secretHelper;
            this.wxCertPath = Path.Combine(secureDirectory.SecureDirectory, "wx", "wxpaycert.json");

            var txt = File.ReadAllText(wxCertPath);
            wxCertificateResult = JsonSerializer.Deserialize<WXCertificateResult>(txt);
            foreach (var c in wxCertificateResult.data)
            {
                c.cert = secretHelper.AesGcmDecrypt(c.encrypt_certificate.associated_data, c.encrypt_certificate.nonce, c.encrypt_certificate.ciphertext);
            }
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
            var zs = wxCertificateResult.data.FirstOrDefault(c => c.serial_no == wechatPaySerial);
            if (zs == null)
                throw new Exception("未找到证书" + wechatPaySerial + "！");
            if (zs.effective_time > now)
                throw new Exception("证书" + wechatPaySerial + "尚未生效！");
            if (zs.expire_time <= now)
                throw new Exception("证书" + wechatPaySerial + "已过期！");
            return zs.cert;
        }
        /// <summary>
        /// 检查证书是否即将过期，若即将过期则更新
        /// </summary>
        /// <param name="cancellationToken"></param>
        public async Task RefreshIfNeededAsync(CancellationToken cancellationToken = default)
        {
            var xzs = wxCertificateResult.data.OrderBy(c => c.effective_time).First();
            var now = await clock.GetNowAsync();
            if (xzs.expire_time.AddDays(-9) <= now)
                await UpdateCertAsync(cancellationToken);
        }
        private async Task UpdateCertAsync(CancellationToken cancellationToken = default)
        {
            var temp = await GetCertAsync(cancellationToken);
            var str = JsonSerializer.Serialize(temp);
            await File.WriteAllTextAsync(wxCertPath, str, cancellationToken);
            foreach (var c in temp.data)
            {
                c.cert = secretHelper.AesGcmDecrypt(c.encrypt_certificate.associated_data, c.encrypt_certificate.nonce, c.encrypt_certificate.ciphertext);
            }
            this.wxCertificateResult = temp;
        }
        /// <summary>
        /// 调用微信接口 获取 微信支付平台证书
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<WXCertificateResult> GetCertAsync(CancellationToken cancellationToken = default)
        {
            var apiUrl = "certificates";
            var response = await httpClientFactory.CreateClientPay().GetAsync(apiUrl, cancellationToken);
            return await response.Content.ReadFromJsonAsync<WXCertificateResult>(cancellationToken);
        }
    }
}