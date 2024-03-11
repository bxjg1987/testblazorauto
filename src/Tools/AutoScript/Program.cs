// See https://aka.ms/new-console-template for more information

//每个任务一个委托，然后统一执行，这样做是便于排序步骤

using Masuit.Tools.Win32;
using System.Reflection;
using System.Text.RegularExpressions;

//当在d盘下直接运行 完整路径的程序时，输出D：\
//string currentWorkingDirectory = Directory.GetCurrentDirectory();
//Console.WriteLine("Current Working Directory: " + currentWorkingDirectory);

// currentWorkingDirectory = Environment.CurrentDirectory;
//Console.WriteLine("Current Working Directory: " + currentWorkingDirectory);

//程序可执行文件的目录
//string baseDirectory = AppContext.BaseDirectory;
//Console.WriteLine("Base Directory: " + baseDirectory);
//string entryAssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
//Console.WriteLine("Entry Assembly Directory: " + entryAssemblyDirectory);

#region 准备环境

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var panfu = AppContext.BaseDirectory.Substring(0, 1);
Console.WriteLine($"盘符：{panfu}");

var jjfaml = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("src"));
Console.WriteLine($"解决方案目录：{jjfaml}");

var ymml = Path.Combine(jjfaml, "src");
Console.WriteLine($"src：{jjfaml}");
#endregion

#region 准备任务
//顶级任务列表
var topTasks = new List<(string, Action)>();

topTasks.Add(new("退出", null));

topTasks.Add(new("打包：BXJG.Common", PackBXJGCommon));


#endregion

#region 任务选择


while (true)
{


    Console.WriteLine("请选择要执行的任务：");



    for (int i = 0; i < topTasks.Count; i++)
    {
        var item = topTasks[i];
        Console.WriteLine($"{i}\t{item.Item1}");
    }

    var zx = int.Parse(Console.ReadLine().Trim());
    if (zx == 0)
        return;

    var act = topTasks[zx];

    act.Item2();

}
#endregion


//void BuildBXJGCommon(){

//    WindowsCommand.Execute("");
//}

//打包BXJGCommon项目
void PackBXJGCommon()
{
    DabaoNuget("BXJG.Common");
}
void shangchuanBXJGCommon()
{
    //  var r = WindowsCommand.Execute($"{panfu}: && cd {projDir} && dotnet pack");
}
//打包nuget
void DabaoNuget(string xmm)
{
    Console.WriteLine($"开始打包{xmm}...");
    var projDir = Path.Combine(ymml, "libs", xmm);

    #region 更新csproj中的nuget版本号

    var xmwj = Directory.GetFiles(projDir, "*.csproj").Single();
    Console.WriteLine($"修改项目文件：{xmwj}的版本号");

    var txt = File.ReadAllText(xmwj);

    var ff = Regex.Match(txt, @"<VersionPrefix>\s*(\d+\.\d+\.\d+)\s*</VersionPrefix>").Groups[1];
    //Console.WriteLine($"{ff.Value}");


    var bbary = ff.Value.Split('.').ToList();

    var zuihoubanben = int.Parse(bbary.Last()) + 1;
    bbary.RemoveAt(bbary.Count - 1);
    bbary.Add(zuihoubanben.ToString());
    var xbb = string.Join(".", bbary);
    
    Console.WriteLine($"{ff.Value}\t===>\t{xbb}");

    txt = Regex.Replace(txt, @"<VersionPrefix>\s*\d+\.\d+\.\d+\s*</VersionPrefix>", $"<VersionPrefix>{xbb}</VersionPrefix>");
    File.WriteAllText(xmwj, txt);

    #endregion
    //var xcvxcv = WindowsCommand.Execute($"{panfu}: && cd {projDir} && dir && exit");
    //Console.WriteLine($"{xcvxcv}");

    Console.WriteLine($"开始打包...");
    var r = WindowsCommand.Execute($"{panfu}: && cd {projDir} && dotnet pack && exit",15000);
    Console.WriteLine($"{r}");

    Console.WriteLine($"{xmm}打包完成！");
}
//发布nuget到私有仓库
void FabuNuget(string xmm)
{
    Console.WriteLine($"正在将{xmm}的nuget包发布到私有包服务器...");
    var projDir = Path.Combine(ymml, "libs", xmm, "bin", "Release");

    //dotnet nuget push -s http://222.178.145.148:19904/v3/index.ison -k xxx .\BXJG.Wechat.Abp.1.2.13.nupkg
    var r = WindowsCommand.Execute($"{panfu}: && cd {projDir} && dotnet pack");
    Console.WriteLine($"{xmm}已成功发送到私有包仓库！");
}
///获取项目目录物理路径，也就是项目csproj所在物理路径
//string huoquxiangmumulu(string xmm) => Path.Combine(ymml,);