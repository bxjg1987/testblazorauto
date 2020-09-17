using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BXJG.Equipment.Protocol;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OxygenChamber.Server.Db;
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
        private readonly fszt fszt;
        public StateResult(ConnectionProvider connectionProvider, fszt fszt, ILogger<StateResult> logger = default)
        {
            this.Logger = logger ?? NullLogger<StateResult>.Instance;
            this.connectionProvider = connectionProvider;
            this.fszt = fszt;
        }

        public async ValueTask ExecuteAsync(IAppSession session, OxygenChamberPackage package)
        {
            Logger.LogInformation($"状态上报。设备ID：{package.EquipmentId}");
            var ztsb = package as ztsb;
            //无论是否成功存储设备状态都先回复设备
            await session.SendAsync(fszt, new OxygenChamberStatePackage { EquipmentId = package.EquipmentId, Key = 5, State = true });

            //存储设备状态信息
            var conn = connectionProvider.GetConnection();
            using (var cmd = conn.CreateCommand())
            {
                //这里没啥必要用参数化
                cmd.CommandText = $@"insert into BXJGEquipmentState(CreationTime,EquipmentId,DoorState,ElectricState,ValveState,Pressure,OxygenConcentration) 
                                     values({DateTime.Now},{package.EquipmentId},{ztsb.DoorState},{ ztsb.ElectricState},{ztsb.ValveState},{ztsb.Pressure},{ ztsb.OxygenConcentration})";
                await cmd.ExecuteNonQueryAsync();
            }
            //int cc = 2;
            //for (int i = 0; i < cc; i++)
            //{
            //    try
            //    {
            //        var conn = connectionProvider.GetConnection();
            //        using (var cmd = conn.CreateCommand())
            //        {
            //            //这里没啥必要用参数化
            //            cmd.CommandText = $@"insert into BXJGEquipmentState(CreationTime,EquipmentId,DoorState,ElectricState,ValveState,Pressure,OxygenConcentration) 
            //                            values({DateTime.Now},{package.EquipmentId},{ztsb.DoorState},{ ztsb.ElectricState},{ztsb.ValveState},{ztsb.Pressure},{ ztsb.OxygenConcentration})";
            //            await cmd.ExecuteNonQueryAsync();
            //            break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        if (i < cc)
            //            continue;
            //        throw;
            //    }
            //    finally
            //    {
            //        await session.SendAsync(new fszt(),new OxygenChamberStatePackage { EquipmentId= package.EquipmentId, Key=5, State=true });//回复设备
            //    }
            //}
        }
    }
}
