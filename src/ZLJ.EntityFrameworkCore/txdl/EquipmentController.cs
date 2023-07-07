using Abp.Dependency;
using Abp.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using txdl;
using System.Collections.Concurrent;
using System;

namespace ZLJ.txdl
{
    /// <summary>
    /// 设备控制器。提供向设备发出各种指令的方法
    /// </summary>
    public class EquipmentController1 : IEquipmentController
    {
        public ICancellationTokenProvider CancellationTokenProvider { get; set; }

        private readonly TXDLHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        //当期类是单例注册的

        static ConcurrentBag<string> configChangedKeys = new ConcurrentBag<string>();
        public EquipmentController1(TXDLHttpClientFactory httpClientFactory, ILogger<EquipmentController1> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);


                    var ids = new List<string>();
                    string p = string.Empty;

                    foreach (var item in configChangedKeys)
                    {
                        if (configChangedKeys.TryTake(out p))
                        {
                            if (!ids.Contains(p))
                                ids.Add(p);
                        }
                    }
                    if (ids.Count == 0)
                        continue;
                    try
                    {
                        await _httpClientFactory.CreateHttpClient().PostAsJsonAsync("configChanged", ids, CancellationTokenProvider.Token);
                    }
                    catch (Exception ex)
                    {
                        this._logger.LogWarning("更新设备通信配置发送失败！");
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public async Task<IEnumerable<string>> LockAsync(params string[] ids)
        {
            var r = await _httpClientFactory.CreateHttpClient().PostAsJsonAsync("lock", ids, CancellationTokenProvider.Token);
            return await r.Content.ReadFromJsonAsync<IEnumerable<string>>(
                cancellationToken: CancellationTokenProvider.Token);
        }

        public async Task<IEnumerable<string>> UnLockAsync(params string[] ids)
        {
            var r = await _httpClientFactory.CreateHttpClient().PostAsJsonAsync("unlock", ids, CancellationTokenProvider.Token);
            return await r.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: CancellationTokenProvider.Token);
        }

        public async Task<IEnumerable<string>> UnOrLockAsync(bool isLock, params string[] ids)
        {
            var r = await _httpClientFactory.CreateHttpClient().PostAsJsonAsync(isLock ? "lock" : "unlock", ids, CancellationTokenProvider.Token);
            return await r.Content.ReadFromJsonAsync<IEnumerable<string>>(cancellationToken: CancellationTokenProvider.Token);
        }

        public ValueTask ConfigChangedAsync(params string[] ids)
        {
            foreach (var item in ids)
            {
                configChangedKeys.Add(item);
            }
            return ValueTask.CompletedTask;
            //configChangedKeys.Clear();
            //return _httpClientFactory.CreateHttpClient().PostAsJsonAsync("configChanged", new { a = 0 }, CancellationTokenProvider.Token);
        }

        public async Task<EquipmentBaseInfoSnapshot> InitGetAsync(string communicationType, IDictionary<string, object> communicationParameter)
        {
            var initGetInput = new InitGetInput
            {
                CommunicationType = communicationType,
                CommunicationParameters = communicationParameter,
            };
            var r = await _httpClientFactory.CreateHttpClient().PostAsJsonAsync("initGet", initGetInput, CancellationTokenProvider.Token);

            return await r.Content.ReadFromJsonAsync<EquipmentBaseInfoSnapshot>(cancellationToken: CancellationTokenProvider.Token);
        }

        public Task<IEnumerable<string>> GetCommunicationTypes()
        {
            return _httpClientFactory.CreateHttpClient().GetFromJsonAsync<IEnumerable<string>>("getCommunicationTypes", CancellationTokenProvider.Token);
        }

        public async Task<IEnumerable<string>> CheckOnlineAsync(params string[] ids)
        {
            var query = string.Join("&", ids.Select(x => $"ids={x}"));
            return await _httpClientFactory.CreateHttpClient().GetFromJsonAsync<IEnumerable<string>>($"CheckOnline?{query}", CancellationTokenProvider.Token);
        }
    }
}