using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SuperSocket;
using SuperSocket.Server;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SuperSocket.ProtoBase;
using System.Threading.Tasks;

namespace OxygenChamber.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IHost host = null;
            host = MultipleServerHostBuilder.Create()
               .AddServer<Class1, Class2>(builder =>
                {
                    builder.UsePackageHandler(async (s, p) =>
                    {
                        await s.SendAsync(null);
                    })
                    .ConfigureSuperSocket(opt =>
                    {
                        opt.Listeners = new ListenOptions[] {
                            new ListenOptions
                            {
                                Ip = "Any",
                                Port = 4040
                            }
                        };
                    });
                })
               .AddServer<Class3, Class4>(builder =>
               {
                   builder.UsePackageHandler(async (session, p) =>
                   {
                       var servers = host.Services.GetRequiredService<SuperSocketService<Class1>>();
                       var sessions = servers.GetAsyncSessionContainer();
                       // sessions.GetSessionByIDAsync
                       await session.SendAsync(null);
                   }).UseSessionHandler(session=> {
                       //session.SessionID
                       return new ValueTask();
                   })
                   .ConfigureSuperSocket(opt =>
                   {
                       opt.Listeners = new ListenOptions[] {
                            new ListenOptions
                            {
                                Ip = "Any",
                                Port = 5050
                            }
                       };
                   });
               })
               .ConfigureLogging(opt => opt.AddConsole())
               .Build();

            await host.RunAsync();
        }
        //public static async ValueTask h1(IAppSession session, Class3 class3) {
        //   // session.Channel.
        //   // await session.SendAsync(null);
        //}
    }
}
