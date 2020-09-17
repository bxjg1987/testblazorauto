using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OxygenChamber.Server.Db
{
    /// <summary>
    /// 开个线程清理比较老的设备状态数据
    /// 注意单例注入容器
    /// 也可以考虑直接在abp中使用它提供的定时任务功能
    /// </summary>
    public class Cleaner
    {
        const int blsc = 1000 * 60 * 60 * 24;//设备的状态数据默认保留时长
        private readonly ConnectionProvider connectionProvider;
        public ILogger Logger { get; set; }
        public Cleaner(ConnectionProvider connectionProvider, ILogger<Cleaner> logger = default)
        {
            this.connectionProvider = connectionProvider;
            Logger = logger ?? NullLogger<Cleaner>.Instance;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Task.Delay(blsc / 5).Wait();
                    var t = DateTime.Now.AddMilliseconds(-blsc);
                    try
                    {
                        var conn = connectionProvider.GetConnection();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = $@"delete from BXJGEquipmentState where CreationTime<@t";
                            cmd.Parameters.Add("t");
                            cmd.Parameters["t"].Value = t;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "清除设备历史状态失败！");
                    }
                }
            });
        }
    }
}
