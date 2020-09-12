using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OxygenChamber.Server.Db;
using OxygenChamber.Server.Protocol;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;

namespace OxygenChamber.Server.Command
{
    /// <summary>
    /// 设备发送过来的状态上报
    /// </summary>
    [Command(Key = (byte)5)]
    public class StateResult : IAsyncCommand<OxygenChamberPackage>
    {
        public ILogger Logger { get; set; }
        private readonly ConnectionProvider connectionProvider;

        public StateResult(ConnectionProvider connectionProvider, ILogger<StateResult> logger = default)
        {
            this.Logger = logger ?? NullLogger<StateResult>.Instance;
            this.connectionProvider = connectionProvider;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            Logger.LogInformation($"状态上报。设备ID：{package.EquipmentId}");
            int cc = 2;
            for (int i = 0; i < cc; i++)
            {
                try
                {
                    var conn = connectionProvider.GetConnection();
                    using (var cmd = conn.CreateCommand())
                    {
                        //这里没啥必要用参数化
                        cmd.CommandText = $@"insert into BXJGEquipmentState(CreationTime,EquipmentId,DoorState,ElectricState,ValveState,Pressure,OxygenConcentration) 
                                        values({DateTime.Now},{package.EquipmentId},{package.DoorState},{ package.ElectricState},{package.ValveState},{package.Pressure},{ package.OxygenConcentration})";
                        await cmd.ExecuteNonQueryAsync();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (i < cc)
                        continue;
                    throw;
                }
                finally
                {
                    await session.SendEquipmentStateAsync(package.EquipmentId, 5, true);//回复设备
                }
            }
        }
    }
}
