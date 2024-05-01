using CodeGenerator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RazorLight;
using System.Text.RegularExpressions;
using System.Text;

//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
//builder.Services.Configure<List<ProjectDefine>>(builder.Configuration.GetSection("Projects"));
//builder.Services.AddHostedService<MainService>();
//IHost host = builder.Build();
//host.Run();

var services = new ServiceCollection();

IConfigurationBuilder builder = new ConfigurationBuilder();
InitProject();

var configuration = builder.Build();

services.Configure<ExecuteContext>(configuration);

IServiceProvider serviceProvider = services.BuildServiceProvider();


var ctx = serviceProvider.GetService<IOptionsMonitor<ExecuteContext>>().CurrentValue;
ctx.Services = serviceProvider;
ctx.Models.ForEach(x => { 
    x.ExecuteContext = ctx;
    x.Fields.ForEach(y => y.Model = x);
});

var engine = new RazorLightEngineBuilder()
    .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
    .UseMemoryCachingProvider()
    .Build();

List<Action<ExecuteContext>> actions = new List<Action<ExecuteContext>> { Entity, CoreShareConst , EFMap, EFDbContext , ProviderDto, ProviderCondition };


//var model = new { Name = "John Doe" };
//string result = await engine.CompileRenderAsync("Subfolder/View.cshtml", model);
string q = "c";
while (q.ToLower() == "c")
{

    //xuanzexiangm(ctx);
    SelectModel(ctx);
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


    Console.WriteLine("按c键继续，按任意键退出...");
    q = Console.ReadLine();

}





#region 初始化
//void xuanzexiangm(ExecuteContext ctx)
//{
//    Console.WriteLine("请选择项目：");
//    for (int i = 0; i < projects.Count; i++)
//    {
//        var item = projects[i];
//        Console.WriteLine($"{i}\t\t{item.DisplayName}");
//    }
//    var projectStr = Console.ReadLine();

//    if (string.IsNullOrWhiteSpace(projectStr))
//    {
//        ctx.Project = projects.LastOrDefault(x => x.IsDefault);
//        if (ctx.Project == default)
//            ctx.Project = projects.Last();
//    }
//    else
//        ctx.Project = projects[int.Parse(projectStr)];
//}

void InitProject()
{
    Console.WriteLine("请选择项目：");

    var projfiles = Directory.GetFiles("Projects");
    for (int i = 0; i < projfiles.Length; i++)
    {
        Console.WriteLine($"{i}\t\t{projfiles[i]}");
    }
    var sdf = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(sdf))
        sdf = "0";
    var projIndex = int.Parse(sdf);
    builder.AddJsonFile(projfiles[projIndex]);
}
void SelectModel(ExecuteContext ctx)
{
    Console.WriteLine("请选择模型：");
    for (int i = 0; i < ctx.Models.Count; i++)
    {
        var item = ctx.Models[i];
        Console.WriteLine($"{i}\t\t{item.DisplayName}");
    }
    var projectStr = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(projectStr))
        ctx.Model = ctx.Models.Last();
    else
        ctx.Model = ctx.Models[int.Parse(projectStr)];
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


void Entity(ExecuteContext ctx)
{
    Console.WriteLine("正在生成实体类...");
    var str = engine.CompileRenderAsync("Entity", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.CoreProjectName, ctx.Model.Name, ctx.Model.EntityName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成实体类完成");
}
void CoreShareConst(ExecuteContext ctx)
{
    Console.WriteLine("正在生成CoreConst...");

    var str = engine.CompileRenderAsync("CoreShareConsts", ctx).Result;
    var file = Path.Combine(ctx.SrcDir, ctx.CoreShareProjectName, ctx.Model.Name, ctx.Model.CoreShareConst + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));


    //var old = File.ReadAllText(file);

    //old = Regex.Replace(old, @$"#region {ctx.Model.DisplayName}MaxLength.*?#endregion", string.Empty, RegexOptions.Singleline);




    ////File.Replace(file, ctx.CodeGeneratorReplace, str);
    //str += Environment.NewLine;
    //str += $"\t\t{ExecuteContext.CodeGeneratorReplace}";

    //str = old.Replace(ExecuteContext.CodeGeneratorReplace, str);

    File.WriteAllText(file, str);


    Console.WriteLine("生成CoreConst完成");
}
void EFMap(ExecuteContext ctx)
{
    Console.WriteLine("正在生成ef映射...");

    var str = engine.CompileRenderAsync("EFMap", ctx).Result;
    //用这个路径，方便后续添加dbcontext、seed等
    var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName,  ctx.Model.Name , "EFMap.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));


    //var old = File.ReadAllText(file);

    //old = Regex.Replace(old, @$"#region {ctx.Model.DisplayName}MaxLength.*?#endregion", string.Empty, RegexOptions.Singleline);




    ////File.Replace(file, ctx.CodeGeneratorReplace, str);
    //str += Environment.NewLine;
    //str += $"\t\t{ExecuteContext.CodeGeneratorReplace}";

    //str = old.Replace(ExecuteContext.CodeGeneratorReplace, str);

    File.WriteAllText(file, str);


    Console.WriteLine("生成ef映射完成");
}
void EFDbContext(ExecuteContext ctx)
{
    Console.WriteLine("正在生成efDbContext映射...");

    var str = engine.CompileRenderAsync("DbContext", ctx).Result;
    //用这个路径，方便后续添加dbcontext、seed等
    var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, ctx.Model.Name, "DbContext.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));


    //var old = File.ReadAllText(file);

    //old = Regex.Replace(old, @$"#region {ctx.Model.DisplayName}MaxLength.*?#endregion", string.Empty, RegexOptions.Singleline);




    ////File.Replace(file, ctx.CodeGeneratorReplace, str);
    //str += Environment.NewLine;
    //str += $"\t\t{ExecuteContext.CodeGeneratorReplace}";

    //str = old.Replace(ExecuteContext.CodeGeneratorReplace, str);

    File.WriteAllText(file, str);


    Console.WriteLine("生成efDbContext映射完成");
}
void ProviderDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成ProviderDto...");
    var str = engine.CompileRenderAsync("ProviderDto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonShareProjectName, ctx.Model.Name, ctx.Model.ProviderDto + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成ProviderDto完成");
}
void ProviderCondition(ExecuteContext ctx)
{
    Console.WriteLine("正在生成ProviderCondition...");
    var str = engine.CompileRenderAsync("ProviderCondition", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonShareProjectName, ctx.Model.Name, ctx.Model.ProviderCondition + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成ProviderCondition完成");
}