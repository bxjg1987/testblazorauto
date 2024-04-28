


using CodeGenerator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
//builder.Services.Configure<List<ProjectDefine>>(builder.Configuration.GetSection("Projects"));
//builder.Services.AddHostedService<MainService>();
//IHost host = builder.Build();
//host.Run();



var services = new ServiceCollection();

IConfigurationBuilder builder = new ConfigurationBuilder();
builder.AddJsonFile("appsettings.json");
var configuration = builder.Build();

services.Configure<List<ProjectDefine>>(configuration.GetSection("Projects"));

IServiceProvider serviceProvider = services.BuildServiceProvider();


List<ProjectDefine> projects= serviceProvider.GetService<IOptionsMonitor<List<ProjectDefine>>>().CurrentValue;
List<Action<ExecuteContext>> actions = new List<Action<ExecuteContext>> { Entity };

var ctx = new ExecuteContext { Services = serviceProvider };
xuanzexiangm(ctx);
xuanzemoxing(ctx);
//  xuanzemoban(ctx);

Console.WriteLine("环境初始化完成，开始生成...");

//根据模板多线程执行，各模板生成的后续处理都在自己的流程中进行
//另一种思路是，子任务总体按线性执行，这样看起来步骤清晰，进度明确，还可以与用户交互。

//由于这里只是实现简单的代码生成，所以不太有需要有多快。
//一个模板，一个子任务，某些子任务可能没有模板

for (int i = 0; i < actions.Count; i++)
{
    Console.WriteLine($"步骤进度：{i + 1}/{actions.Count}");
    actions[i].Invoke(ctx);
}



#region 初始化
void xuanzexiangm(ExecuteContext ctx)
{
    Console.WriteLine("请选择项目：");
    for (int i = 0; i < projects.Count; i++)
    {
        var item = projects[i];
        Console.WriteLine($"{i}\t\t{item.DisplayName}");
    }
    var projectStr = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(projectStr))
    {
        ctx.Project = projects.LastOrDefault(x => x.IsDefault);
        if (ctx.Project == default)
            ctx.Project = projects.Last();
    }
    else
        ctx.Project = projects[int.Parse(projectStr)];
}
void xuanzemoxing(ExecuteContext ctx)
{
    Console.WriteLine("请选择模型：");
    for (int i = 0; i < ctx.Project.Models.Count; i++)
    {
        var item = ctx.Project.Models[i];
        Console.WriteLine($"{i}\t\t{item.DisplayName}");
    }
    var projectStr = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(projectStr))
        ctx.Model = ctx.Project.Models.Last();
    else
        ctx.Model = ctx.Project.Models[int.Parse(projectStr)];
}
//void xuanzemoban(ExecuteContext ctx)
//{
//    Console.WriteLine("请选择模板：");
//    for (int i = 0; i < ctx.Project.Templates.Count; i++)
//    {
//        var item = ctx.Project.Templates[i];
//        Console.WriteLine($"{i}\t\t{item.DisplayName}");
//    }
//    var projectStr = Console.ReadLine();

//    if (string.IsNullOrWhiteSpace(projectStr))
//        ctx.Templates = ctx.Project.Templates;
//    else
//    {
//        ctx.Templates = projectStr.Split(" ").Select(x=> ctx.Project.Templates[  int.Parse(x)]);
//    }
//}
#endregion

//下面是步骤

#region 处理实体
static void Entity(ExecuteContext ctx)
{
    Console.WriteLine("正在生成实体类...");

    Console.WriteLine("生成实体类完成");
}
#endregion
