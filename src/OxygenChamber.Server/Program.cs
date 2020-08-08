using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using SuperSocket.Server;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SuperSocket.ProtoBase;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using SuperSocket.Command;
using OxygenChamber.Server.Protocol;

namespace OxygenChamber.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //尽可能将配置放到配置文件中
            IHost host = null;
            host = SuperSocketHostBuilder
                .Create<OxygenChamberPackage, OxygenChamberPackagePipelineFilter>()
                //.UseSessionHandler(session =>
                //{
                //    //(session as OxygenChamberSession).EquipmentId 
                //    //map.TryAdd(session.SessionID, 0);
                //    return new ValueTask();
                //})
                .UseCommand(commandOptions =>
                {
                    // 一个一个的注册命令
                    //commandOptions.AddCommand<ADD>();

                    // 注册程序集重的所有命令
                    commandOptions.AddCommandAssembly(Assembly.GetExecutingAssembly());
                })
                //.UsePackageHandler(async (s, p) =>
                //{
                //如果是设备上报
                //可以考虑public class LOGIN : IAsyncCommand<StringPackageInfo>
                //if (p.Key == 6)
                //{
                //    map[s.SessionID] = p.Id;
                //    return;
                //}
                //var servers = host.Services.GetRequiredService<SuperSocketService<OxygenChamberPackage>>();
                //var sessions = servers.GetAsyncSessionContainer();
                //var equipment = await sessions.GetSessionByIDAsync(p.Id.ToString());
                //await equipment.SendAsync(p.OriginalCMD);
                //})
                .UsePackageDecoder<PackageConverter>()
                .UseSession<OxygenChamberSession>()
                .UseInProcSessionContainer()//必须使用这个，否则通信过程中获取session容器为空
                .Build();
          
            await host.RunAsync();

            //下面是多服务器配置

            //IHost host = null;
            //host = MultipleServerHostBuilder.Create()
            //.AddServer<OxygenChamberPackage, Class2>(builder =>
            // {
            //     builder.UsePackageHandler(async (s, p) =>
            //     {
            //         await s.SendAsync(null);
            //     })
            //     .ConfigureSuperSocket(opt =>
            //     {
            //         opt.Listeners = new ListenOptions[] {
            //             new ListenOptions
            //             {
            //                 Ip = "Any",
            //                 Port = 4040
            //             }
            //         };
            //     });
            // })
            //.AddServer<Class3, Class4>(builder =>
            //{
            //    builder.UsePackageHandler(async (session, p) =>
            //    {
            //        var servers = host.Services.GetRequiredService<SuperSocketService<OxygenChamberPackage>>();
            //        var sessions = servers.GetAsyncSessionContainer();
            //        // sessions.GetSessionByIDAsync
            //        await session.SendAsync(null);
            //    })
            //    .UseSessionHandler(session=> {
            //        //session.SessionID
            //        return new ValueTask();
            //    })
            //    .ConfigureSuperSocket(opt =>
            //    {
            //        opt.Listeners = new ListenOptions[] {
            //             new ListenOptions
            //             {
            //                 Ip = "Any",
            //                 Port = 5050
            //             }
            //        };
            //    });
            //})
            //.ConfigureLogging(opt => opt.AddConsole())
            //.Build();

            //await host.RunAsync();
        }
        //public static async ValueTask h1(IAppSession session, Class3 class3) {
        //   // session.Channel.
        //   // await session.SendAsync(null);
        //}
    }
}
