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
ctx.Models.ForEach(x =>
{
    x.ExecuteContext = ctx;
    x.Fields.ForEach(y => y.Model = x);
});

var engine = new RazorLightEngineBuilder()
    .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
    .UseMemoryCachingProvider()
    .Build();

List<Action<ExecuteContext>> actions = new List<Action<ExecuteContext>>
{
    Entity,
    CoreShareConst ,
    EFMap,
    EFDbContext ,
    ProviderDto,
    ProviderCondition,
    ProviderAppService,
    ProviderObjMap,
    ApplicationShareConst,
    Condition,
    Dto,
    EditDto,
    CreateDto
};


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
    var file = Path.Combine(ctx.SrcDir, ctx.CoreShareProjectName, ctx.Model.Name, ctx.Model.CoreShareConstName + ".cs");
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
    var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, ctx.Model.Name, "EFMap.cs");
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
//替换主类字符串的方式
void EFDbContextBak(ExecuteContext ctx)
{
    Console.WriteLine("正在生成efDbContext映射...");

    //var str = engine.CompileRenderAsync("DbContext", ctx).Result;
    ////用这个路径，方便后续添加dbcontext、seed等
    //var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, ctx.Model.Name, "DbContext.cs");
    //Directory.CreateDirectory(Path.GetDirectoryName(file));

    var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, "EntityFrameworkCore", ctx.Name + "DbContext.cs");
    var old = File.ReadAllText(file);

    //old = Regex.Replace(old, @$"#region {ctx.Model.DisplayName}dbset.*?#endregion", string.Empty, RegexOptions.Singleline);



    var str = $"public DbSet<{ctx.Model.EntityName}> {ctx.Model.Name} {{ get;set; }}";
    if (!old.Contains(str))
    {
        ////File.Replace(file, ctx.CodeGeneratorReplace, str);
        str += Environment.NewLine;
        str += $"\t\t{ExecuteContext.CodeGeneratorReplace}";

        old = old.Replace(ExecuteContext.CodeGeneratorReplace, str);
    }

    str = $"using {ctx.Model.CoreNamespace};";
    if (!old.Contains(str))
    {
        str += Environment.NewLine;
        old = str + old;
    }

    File.WriteAllText(file, old);


    Console.WriteLine("生成efDbContext映射完成");
}
//部分类的方式，编译时部分类反正会合并，木有性能问题，各模型的文件都放在自己的文件夹中方便管理
void EFDbContext(ExecuteContext ctx)
{
    Console.WriteLine("正在生成DbContext...");
    var str = engine.CompileRenderAsync("DbContext", ctx).Result;
    var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, ctx.Model.Name, ctx.Name + "DbContext.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));
    File.WriteAllText(file, str);
    Console.WriteLine("生成efDbContext映射完成");
}
void ProviderDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成ProviderDto...");
    var str = engine.CompileRenderAsync("ProviderDto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonShareProjectName, ctx.Model.Name, ctx.Model.ProviderDtoName + ".cs");
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
void ProviderAppService(ExecuteContext ctx)
{
    Console.WriteLine("正在生成ProviderAppService...");
    var str = engine.CompileRenderAsync("ProviderAppService", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonProjectName, ctx.Model.Name, ctx.Model.ProviderAppService + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成ProviderAppService完成");
}
void ProviderObjMap(ExecuteContext ctx)
{
    Console.WriteLine("正在生成provider的automapper...");
    var str = engine.CompileRenderAsync("ProviderObjMap", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonProjectName, ctx.Model.Name, "AutoMapperProfile.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成provider的automapper完成");
}
void ApplicationShareConst(ExecuteContext ctx)
{
    Console.WriteLine("正在生成admin应用中，应用服务共享层的常量...");
    var str = engine.CompileRenderAsync("ApplicationShareConst", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.ApplicationShareConstName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成admin应用中，应用服务共享层的常量完成");
}
void Condition(ExecuteContext ctx)
{
    Console.WriteLine("正在生成应用共享层中的条件类...");
    var str = engine.CompileRenderAsync("Condition", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.ConditionName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成应用共享层中的条件类完成");
}
void Dto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成Dto...");
    var str = engine.CompileRenderAsync("Dto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.DtoName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成Dto完成");
}
void EditDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成EditDto...");
    var str = engine.CompileRenderAsync("EditDto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.EditDtoName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成EditDto完成");
}
void CreateDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成CreateDto...");
    var str = engine.CompileRenderAsync("CreateDto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.CreateDtoName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成CreateDto完成");
}