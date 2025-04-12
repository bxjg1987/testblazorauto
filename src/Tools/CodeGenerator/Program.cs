using CodeGenerator;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RazorLight;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Immutable;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
//builder.Services.Configure<List<ProjectDefine>>(builder.Configuration.GetSection("Projects"));
//builder.Services.AddHostedService<MainService>();
//IHost host = builder.Build();
//host.Run();

/*
 * 整体流程
 * 先通过json配置项目和模型
 * 启动
 * 用户做各种设置
 * 生成
 * 查看结果
 * 
 * 注意倾向于在一开始设定好所有配置，后续直接跑生成任务
 * 
 * 此外ExecuteContext及其属性主要传递数据到模板中，也为后续步骤提供数据，总体来说是传递数据的作用
 * 所以不要在它们内部定义过多逻辑
 * 
 */

var services = new ServiceCollection();

IConfigurationBuilder builder = new ConfigurationBuilder();
var mxwjj = InitProject();//返回模型文件夹

var configuration = builder.Build();



services.Configure<ExecuteContext>(configuration);

IServiceProvider serviceProvider = services.BuildServiceProvider();


var ctx = serviceProvider.GetService<IOptionsMonitor<ExecuteContext>>().CurrentValue;



ctx.Models = new List<ModelDefine>();
var ms = Directory.GetFiles(Path.Combine("projects", mxwjj));
foreach (var item in ms)
{
    var b2 = new ConfigurationBuilder().AddJsonFile(item).Build();
    var m = new ModelDefine();
    b2.Bind(m);
    m.ExecuteContext = ctx;
    m.Fields.ForEach(y => y.Model = m);
    ctx.Models.Add(m);
    //var sdfsdf = b2.
}

ctx.Services = serviceProvider;
//ctx.Models.ForEach(x =>
//{

//});
foreach (var item in ctx.Apps)
{
    item.ExecuteContext = ctx;
}

var engine = new RazorLightEngineBuilder()
    .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
    .UseMemoryCachingProvider()
    .Build();

List<Action<ExecuteContext>> actions = new List<Action<ExecuteContext>>
{
    CoreEntity,
    CoreConsts ,
    EFMap,
    EFDbContext ,
    AppCommomDto,
    AppCommomCondition,
    AppCommomAppService,
    AppCommomObjMap

};

//var model = new { Name = "John Doe" };
//string result = await engine.CompileRenderAsync("Subfolder/View.cshtml", model);
string q = "y";
while (q.ToLower() == "y")
{

    //xuanzexiangm(ctx);
    SelectModel(ctx);
    List<Action<ExecuteContext>> actions2 = new List<Action<ExecuteContext>>
    {
        SelectConnectionString,
        AppConsts,
        AppCondition,
        AppDto,
        AppEditDto,
        AppCreateDto,
        AppPermissionProvider,
        AppNavigationProvider,
        AppLocal,
        AppObjMap,
        UIObjMap,
        SetPermission
    };

    if (ctx.Model.IsTree)
    {
        actions2.Add(AppServiceTree);
        //actions2.Add(UITreeObjMap);
        actions2.Add(UITreeList);
        actions2.Add(UITreeListCs);
        actions2.Add(UITreeCreate);
        actions2.Add(UITreeCreateCs);
        actions2.Add(UITreeDetailUpdate);
        actions2.Add(UITreeDetailUpdateCs);
    }
    else
    {
        actions2.Add(AppService);
        //actions2.Add(UIObjMap);
        actions2.Add(UIList);
        actions2.Add(UIListCs);
        actions2.Add(UICreate);
        actions2.Add(UICreateCs);
        actions2.Add(UIDetailUpdate);
        actions2.Add(UIDetailUpdateCs);
    }

    //  xuanzemoban(ctx);
    Console.WriteLine("是否生成核心部分？y生成，其它任意键跳过");
    var schx = Console.ReadLine().Trim().ToLower() == "y";
    var apps = SelectApps(ctx);

    //Console.WriteLine("环境初始化完成，开始生成...");

    //根据模板多线程执行，各模板生成的后续处理都在自己的流程中进行
    //另一种思路是，子任务总体按线性执行，这样看起来步骤清晰，进度明确，还可以与用户交互。

    //由于这里只是实现简单的代码生成，所以不太有需要有多快。
    //一个模板，一个子任务，某些子任务可能没有模板

    if (schx)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            Console.WriteLine($"核心步骤进度：{i + 1}/{actions.Count}");
            actions[i].Invoke(ctx);
        }
    }

    foreach (var item in apps)
    {
        ctx.App = item;

        for (int i = 0; i < actions2.Count; i++)
        {
            Console.WriteLine($"应用步骤进度：{i + 1}/{actions2.Count}");
            actions2[i].Invoke(ctx);
        }



    }

    Console.WriteLine("是否继续？按y键继续，按任意键退出...");
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

string InitProject()
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
    return Path.GetFileNameWithoutExtension(projfiles[projIndex]);
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
IEnumerable<AppDefine> SelectApps(ExecuteContext ctx)
{
    Console.WriteLine("请选择应用，y或空字符表示全部，n不选择，数字加逗号分割为多选：");
    for (int i = 0; i < ctx.Apps.Length; i++)
    {
        var item = ctx.Apps[i];
        Console.WriteLine($"{i}\t\t{item.DisplayName}");
    }
    var projectStr = Console.ReadLine()?.Trim().ToLower();
    if (projectStr == "n")
        return [];
    if (string.IsNullOrWhiteSpace(projectStr) || projectStr == "y")
        return ctx.Apps.ToImmutableList();
    else
        return projectStr.Replace("，", "").Split(',').Select(x => ctx.Apps[int.Parse(x.Trim())]);
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


void CoreEntity(ExecuteContext ctx)
{
    Console.WriteLine("正在生成实体类...");
    var str = engine.CompileRenderAsync("core/Entity", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.CoreProjectName, ctx.Model.Name, ctx.Model.EntityName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成实体类完成");
}
void CoreConsts(ExecuteContext ctx)
{
    Console.WriteLine("正在生成Const...");

    var str = engine.CompileRenderAsync("core/Consts", ctx).Result;
    var file = Path.Combine(ctx.SrcDir, ctx.CoreShareProjectName, ctx.Model.Name, ctx.Model.CoreShareConstName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));


    //var old = File.ReadAllText(file);

    //old = Regex.Replace(old, @$"#region {ctx.Model.DisplayName}MaxLength.*?#endregion", string.Empty, RegexOptions.Singleline);




    ////File.Replace(file, ctx.CodeGeneratorReplace, str);
    //str += Environment.NewLine;
    //str += $"\t\t{ExecuteContext.CodeGeneratorReplace}";

    //str = old.Replace(ExecuteContext.CodeGeneratorReplace, str);

    File.WriteAllText(file, str);


    Console.WriteLine("生成Consts完成");
}
void EFMap(ExecuteContext ctx)
{
    Console.WriteLine("正在生成ef映射...");

    var str = engine.CompileRenderAsync("ef/EFMap", ctx).Result;
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
////替换主类字符串的方式
//void EFDbContextBak(ExecuteContext ctx)
//{
//    Console.WriteLine("正在生成efDbContext映射...");

//    //var str = engine.CompileRenderAsync("DbContext", ctx).Result;
//    ////用这个路径，方便后续添加dbcontext、seed等
//    //var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, ctx.Model.Name, "DbContext.cs");
//    //Directory.CreateDirectory(Path.GetDirectoryName(file));

//    var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, "EntityFrameworkCore", ctx.Name + "DbContext.cs");
//    var old = File.ReadAllText(file);

//    //old = Regex.Replace(old, @$"#region {ctx.Model.DisplayName}dbset.*?#endregion", string.Empty, RegexOptions.Singleline);



//    var str = $"public DbSet<{ctx.Model.EntityName}> {ctx.Model.Name} {{ get;set; }}";
//    if (!old.Contains(str))
//    {
//        ////File.Replace(file, ctx.CodeGeneratorReplace, str);
//        str += Environment.NewLine;
//        str += $"\t\t{ExecuteContext.CodeGeneratorReplace}";

//        old = old.Replace(ExecuteContext.CodeGeneratorReplace, str);
//    }

//    str = $"using {ctx.Model.CoreNamespace};";
//    if (!old.Contains(str))
//    {
//        str += Environment.NewLine;
//        old = str + old;
//    }

//    File.WriteAllText(file, old);


//    Console.WriteLine("生成efDbContext映射完成");
//}
//部分类的方式，编译时部分类反正会合并，木有性能问题，各模型的文件都放在自己的文件夹中方便管理
void EFDbContext(ExecuteContext ctx)
{
    Console.WriteLine("正在生成DbContext...");
    var str = engine.CompileRenderAsync("ef/EFDbContext", ctx).Result;
    var file = Path.Combine(ctx.SrcDir, ctx.EFCoreProjectName, ctx.Model.Name, ctx.Name + "DbContext.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));
    File.WriteAllText(file, str);
    Console.WriteLine("生成efDbContext映射完成");
}
void AppCommomDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成CommomShareDto...");
    var str = engine.CompileRenderAsync("appcommon/Dto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonShareProjectName, ctx.Model.Name, ctx.Model.ProviderDtoName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成CommomShareDto完成");
}
void AppCommomCondition(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppCommomCondition...");
    var str = engine.CompileRenderAsync("appcommon/Condition", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonShareProjectName, ctx.Model.Name, ctx.Model.ProviderCondition + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppCommomCondition完成");
}
void AppCommomAppService(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppCommomAppService...");
    var str = engine.CompileRenderAsync("appcommon/AppService", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonProjectName, ctx.Model.Name, ctx.Model.ProviderAppService + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppCommomAppService完成");
}
void AppCommomObjMap(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppCommomObjMap...");
    var str = engine.CompileRenderAsync("AppCommon/ObjMap", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.ApplicationCommonProjectName, ctx.Model.Name, "AutoMapperProfile.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppCommomObjMap完成");
}
void AppConsts(ExecuteContext ctx)
{
    Console.WriteLine("正在生成admin应用中，应用服务共享层的常量...");
    var str = engine.CompileRenderAsync("app/Consts", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.ApplicationShareConstName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成admin应用中，应用服务共享层的常量完成");
}
void AppCondition(ExecuteContext ctx)
{
    Console.WriteLine("正在生成应用共享层中的条件类...");
    var str = engine.CompileRenderAsync("app/Condition", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.ConditionName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成应用共享层中的条件类完成");
}
void AppDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppDto...");
    var str = engine.CompileRenderAsync("app/Dto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.DtoName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppDto完成");
}
void AppEditDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppEditDto...");
    var str = engine.CompileRenderAsync("app/EditDto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.EditDtoName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppEditDto完成");
}
void AppCreateDto(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppCreateDto...");
    var str = engine.CompileRenderAsync("app/CreateDto", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationShareProjectName, ctx.Model.Name, ctx.Model.CreateDtoName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppCreateDto完成");
}


void AppPermissionProvider(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppPermissionProvider...");
    var str = engine.CompileRenderAsync("app/PermissionProvider", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationProjectName, ctx.Model.Name, "PermissionProvider.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppPermissionProvider完成");
}
void AppNavigationProvider(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppNavigationProvider...");
    var str = engine.CompileRenderAsync("app/NavigationProvider", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationProjectName, ctx.Model.Name, "NavigationProvider.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppNavigationProvider完成");
}
void AppLocal(ExecuteContext ctx)
{
    Console.WriteLine("正在处理应用层本地化...");

    //目标文本
    var mbwb = $"<text name=\"{ctx.Model.PermissionNameGet}\" value=\"{ctx.Model.DisplayName}\" />{Environment.NewLine}<!--codegenerator-->";
    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationProjectName, "Localization", "SourceFiles", $"{ctx.App.Name}-zh-Hans.xml");
    var str = File.ReadAllText(file);
    if (!str.Contains(mbwb))
    {
        str = str.Replace("<!--codegenerator-->", mbwb);
        File.WriteAllText(file, str);
    }

    mbwb = $"<text name=\"{ctx.Model.PermissionNameGet}\" value=\"{ctx.Model.Name}\" />{Environment.NewLine}<!--codegenerator-->";
    file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationProjectName, "Localization", "SourceFiles", $"{ctx.App.Name}-en.xml");
    str = File.ReadAllText(file);
    if (!str.Contains(mbwb))
    {
        str = str.Replace("<!--codegenerator-->", mbwb);
        File.WriteAllText(file, str);
    }

    Console.WriteLine("处理应用层本地化完成");
}
void AppObjMap(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppObjMap...");
    var str = engine.CompileRenderAsync("app/ObjMap", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationProjectName, ctx.Model.Name, "AutoMapperProfile.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppObjMap完成");
}
void AppService(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppService...");
    var str = engine.CompileRenderAsync("app/AppService", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationProjectName, ctx.Model.Name, ctx.Model.ApplicationServiceName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppService完成");
}
void AppServiceTree(ExecuteContext ctx)
{
    Console.WriteLine("正在生成AppServiceTree...");
    var str = engine.CompileRenderAsync("app/AppServiceTree", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.ApplicationProjectName, ctx.Model.Name, ctx.Model.ApplicationServiceName + ".cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成AppServiceTree完成");
}
void UIObjMap(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UIObjMap...");
    var str = engine.CompileRenderAsync("UI/ObjMap", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "ObjMap.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UIObjMap完成");
}
void UIList(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UIList...");
    var str = engine.CompileRenderAsync("UI/simple/List", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "List.razor");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UIList完成");
}
void UIListCs(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UIListCs...");
    var str = engine.CompileRenderAsync("UI/simple/ListCs", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "List.razor.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UIListCs完成");
}
void UICreate(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UICreate...");
    var str = engine.CompileRenderAsync("ui/simple/Create", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "Create.razor");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UICreate完成");
}
void UICreateCs(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UICreateCs...");
    var str = engine.CompileRenderAsync("ui/simple/CreateCs", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "Create.razor.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UICreateCs完成");
}


void UIDetailUpdate(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UIDetailUpdate...");
    var str = engine.CompileRenderAsync("ui/simple/DetailUpdate", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "DetailUpdate.razor");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UIDetailUpdate完成");
}
void UIDetailUpdateCs(ExecuteContext ctx)
{
    Console.WriteLine("正在生成DetailUpdateCs...");
    var str = engine.CompileRenderAsync("ui/simple/DetailUpdateCs", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "DetailUpdate.razor.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成DetailUpdateCs完成");
}
//void UITreeObjMap(ExecuteContext ctx)
//{
//    Console.WriteLine("正在生成UITreeObjMap...");
//    var str = engine.CompileRenderAsync("UITreeObjMap", ctx).Result;

//    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "ObjMap.cs");
//    Directory.CreateDirectory(Path.GetDirectoryName(file));

//    File.WriteAllText(file, str);


//    Console.WriteLine("生成UITreeObjMap完成");
//}

void UITreeList(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UITreeList...");
    var str = engine.CompileRenderAsync("ui/tree/List", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "List.razor");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UITreeList完成");
}
void UITreeListCs(ExecuteContext ctx)
{
    Console.WriteLine("正在生成UITreeListCs...");
    var str = engine.CompileRenderAsync("ui/tree/ListCs", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "List.razor.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成UITreeListCs完成");
}



void UITreeCreate(ExecuteContext ctx)
{
    Console.WriteLine("正在生成TreeCreate...");
    var str = engine.CompileRenderAsync("ui/tree/Create", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "Create.razor");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成TreeCreate完成");
}
void UITreeCreateCs(ExecuteContext ctx)
{
    Console.WriteLine("正在生成TreeCreateCs...");
    var str = engine.CompileRenderAsync("ui/tree/CreateCs", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "Create.razor.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成TreeCreateCs完成");
}


void UITreeDetailUpdate(ExecuteContext ctx)
{
    Console.WriteLine("正在生成TreeDetailUpdate...");
    var str = engine.CompileRenderAsync("ui/tree/DetailUpdate", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "DetailUpdate.razor");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成TreeDetailUpdate完成");
}
void UITreeDetailUpdateCs(ExecuteContext ctx)
{
    Console.WriteLine("正在生成TreeDetailUpdateCs...");
    var str = engine.CompileRenderAsync("ui/tree/DetailUpdateCs", ctx).Result;

    var file = Path.Combine(ctx.SrcDir, ctx.App.BlazorClientProjectName, ctx.Model.Name, "DetailUpdate.razor.cs");
    Directory.CreateDirectory(Path.GetDirectoryName(file));

    File.WriteAllText(file, str);


    Console.WriteLine("生成TreeDetailUpdateCs完成");
}

void SelectConnectionString(ExecuteContext ctx)
{
    //var strConn = "";
    var jsons = Directory.GetFiles(Path.Combine(ctx.SrcDir, ctx.App.ApiHostProjectName)).Where(x => x.Contains(".json"));
    var list = new List<string>();
    foreach (var item in jsons)
    {
        var cb = new ConfigurationBuilder();
        cb.AddJsonFile(item);
        var cf = cb.Build();
        // 或者如果你想获取某个部分的所有配置
        var section = cf.GetSection("ConnectionStrings");
        if (section == default)
            continue;

        foreach (var subKey in section.GetChildren())
        {
            if (list.Contains(subKey.Value))
                continue;
            list.Add(subKey.Value);
        }
    }

    Console.WriteLine("请选择连接字符串：");
    for (int i = 0; i < list.Count; i++)
    {
        Console.WriteLine($"{i}\t{list[i]}");
    }

    int idx = 0;
    var input = Console.ReadLine();
    if (input.IsNotNullOrWhiteSpaceBXJG())
        idx = int.Parse(input);
    ctx.App.ConnectionString = list[idx];
}
void SetPermission(ExecuteContext ctx)
{
    Console.WriteLine("正在为租户管理员授权...");
    //这里的判断不严谨
    IDbConnection conn;
    if (ctx.App.ConnectionString.Contains("root;"))
        conn = new MySql.Data.MySqlClient.MySqlConnection(ctx.App.ConnectionString);
    else
        conn = new SqlConnection(ctx.App.ConnectionString);
    using (conn)
    {
        SetTenantPermission(conn, ctx.Model.PermissionNameGet, ctx.Model.MultiTenantMode);
        SetTenantPermission(conn, ctx.Model.PermissionNameCreate, ctx.Model.MultiTenantMode);
        SetTenantPermission(conn, ctx.Model.PermissionNameUpdate, ctx.Model.MultiTenantMode);
        SetTenantPermission(conn, ctx.Model.PermissionNameDelete, ctx.Model.MultiTenantMode);
    }
    Console.WriteLine("为租户管理员授权已完成");

}
void SetTenantPermission(IDbConnection conn, string permissionName, MultiTenantMode multiTenantMode)
{
    var r = conn.ExecuteScalar<int>("select count(name) from AbpPermissions where name=@name", new { name = permissionName });
    if (r < 1)
    {
        var sql = "insert into AbpPermissions(tenantId,name,isGranted,discriminator,roleId,creationTime) values(@tenantId,@name,@isGranted,@discriminator,@roleId,@creationTime)";

        if (multiTenantMode == MultiTenantMode.Must || multiTenantMode == MultiTenantMode.Have)
        {
            conn.Execute(sql,
                    new
                    {
                        tenantId = 1,
                        name = permissionName,
                        isGranted = true,
                        discriminator = "RolePermissionSetting",
                        roleId = 2,
                        creationTime = DateTime.Now
                    });
        }
        if (multiTenantMode == MultiTenantMode.No || multiTenantMode == MultiTenantMode.Have)
        {
            conn.Execute(sql,
                    new
                    {
                        tenantId = (long?)null,
                        name = permissionName,
                        isGranted = true,
                        discriminator = "RolePermissionSetting",
                        roleId = 1,
                        creationTime = DateTime.Now
                    });
        }

    }
}