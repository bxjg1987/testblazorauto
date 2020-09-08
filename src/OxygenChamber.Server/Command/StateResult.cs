using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        readonly ILogger<StateResult> logger;
        readonly IConfiguration configuration;
        public StateResult(ILogger<StateResult> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            logger.LogInformation($"状态上报。设备ID：{package.EquipmentId}");
            var strConn = configuration.GetConnectionString("Default");
            //这里的场景可以考虑全局连接
            using (var conn = new SqlConnection(strConn))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"insert into BXJGEquipmentState(CreationTime,EquipmentId,DoorState,ElectricState,ValveState,Pressure,OxygenConcentration) 
                                        values(@CreationTime,@EquipmentId,@DoorState,@ElectricState,@ValveState,@Pressure,@OxygenConcentration)";
                    cmd.Parameters.AddWithValue("CreationTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("EquipmentId", package.EquipmentId);
                    cmd.Parameters.AddWithValue("DoorState", package.DoorState);
                    cmd.Parameters.AddWithValue("ElectricState", package.ElectricState);
                    cmd.Parameters.AddWithValue("ValveState", package.ValveState);
                    cmd.Parameters.AddWithValue("Pressure", package.Pressure);
                    cmd.Parameters.AddWithValue("OxygenConcentration", package.OxygenConcentration);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            await session.SendEquipmentStateAsync(package.EquipmentId, 5, true);
        }
    }
}
