// See https://aka.ms/new-console-template for more information

//每个任务一个委托，然后统一执行，这样做是便于排序步骤

using AutoScript;
using Masuit.Tools;
using Masuit.Tools.Win32;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
Console.WriteLine($"src：{ymml}");

var nugetkey = string.Empty;
#endregion

#region 准备任务
//顶级任务列表
var topTasks = new List<(string, Action)>
{
    new("退出", null),
    new("重新发布所有公共包", FabuSuoyouGonggongBao)
};
#endregion

#region 选择并执行任务
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
//重新发布所有公共包
void FabuSuoyouGonggongBao()
{
    FabuXindeBXJGCommon();
    FabuXindeBXJGCommonEFCore();
    FabuXindeBXJGCommonRCL();
    FabuXindeBXJGCommonWeb();

    FabuXindeBXJGUtilsShare();
    FabuXindeBXJGUtils();
    FabuXindeBXJGUtilsEFCore();
    FabuXindeBXJGUtilsApplicationShare();
    FabuXindeBXJGUtilsApplication();
    FabuXindeBXJGUtilsRCL();
    FabuXindeBXJGUtilsWeb();
}
//打包BXJG.Utils.Web
void FabuXindeBXJGUtilsWeb()
{
    DabaoNuget("BXJG.Utils.Web");
    FabuNuget("BXJG.Utils.Web");
}
//打包BXJG.Utils.Share
void FabuXindeBXJGUtilsShare()
{
    DabaoNuget("BXJG.Utils.Share");
    FabuNuget("BXJG.Utils.Share");
}
//打包BXJG.Utils.RCL
void FabuXindeBXJGUtilsRCL()
{
    DabaoNuget("BXJG.Utils.RCL");
    FabuNuget("BXJG.Utils.RCL");
}
//打包BXJG.Utils.EFCore项目
void FabuXindeBXJGUtilsEFCore()
{
    DabaoNuget("BXJG.Utils.EFCore");
    FabuNuget("BXJG.Utils.EFCore");
}
//打包BXJG.Utils.Application.Share项目
void FabuXindeBXJGUtilsApplicationShare()
{
    DabaoNuget("BXJG.Utils.Application.Share");
    FabuNuget("BXJG.Utils.Application.Share");
}
//打包BXJG.Utils.Application项目
void FabuXindeBXJGUtilsApplication()
{
    DabaoNuget("BXJG.Utils.Application");
    FabuNuget("BXJG.Utils.Application");
}
//打包BXJG.Utils项目
void FabuXindeBXJGUtils()
{
    DabaoNuget("BXJG.Utils");
    FabuNuget("BXJG.Utils");
}
//打包BXJG.Common.Web项目
void FabuXindeBXJGCommonWeb()
{
    DabaoNuget("BXJG.Common.Web");
    FabuNuget("BXJG.Common.Web");
}
//打包BXJG.Common.RCL项目
void FabuXindeBXJGCommonRCL()
{
    DabaoNuget("BXJG.Common.RCL");
    FabuNuget("BXJG.Common.RCL");
}
//打包BXJG.Common.EFCore项目
void FabuXindeBXJGCommonEFCore()
{
    DabaoNuget("BXJG.Common.EFCore");
    FabuNuget("BXJG.Common.EFCore");
}
//打包BXJGCommon项目
void FabuXindeBXJGCommon()
{
    DabaoNuget("BXJG.Common");
    FabuNuget("BXJG.Common");
}
//打包nuget
void DabaoNuget(string xmm)
{
    Console.WriteLine($"开始打包{xmm}...");
    //项目文件
    var xmwj = Directory.GetFiles(ymml, $"{xmm}.csproj", SearchOption.AllDirectories).Single();
    //项目目录
    var projDir = Path.GetDirectoryName(xmwj);

    #region 更新csproj中的nuget版本号

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
    cmdexecute($"{panfu}: && cd {projDir} && dotnet pack");
    Console.WriteLine($"{xmm}打包完成！");
}
//发布nuget到私有仓库
void FabuNuget(string xmm)
{
    //项目文件
    var xmwj = Directory.GetFiles(ymml, $"{xmm}.csproj", SearchOption.AllDirectories).Single();
    //项目目录
    var projDir = Path.GetDirectoryName(xmwj);
    var bao = Directory.GetFiles(projDir, $"{xmm}.*.nupkg", SearchOption.AllDirectories).Order().Last();
    Console.WriteLine($"正在将nuget包{bao}发布到私有包源...");
    yaoqiushurunugetkey();
    cmdexecute($"dotnet nuget push -s http://222.178.145.148:19904/v3/index.json -k {nugetkey} {bao}");
    Console.WriteLine($"{xmm}已成功发送到私有包仓库！");
}
//执行cmd命令
void cmdexecute(string cmd)
{
    //p.StandardInput.WriteLine(cmd);
    CMDHelper.Execute(cmd, P_DataReceived);
}
void P_DataReceived(object sender, DataReceivedEventArgs e)
{
    Console.WriteLine(e.Data);
}
void yaoqiushurunugetkey()
{
    if (nugetkey.IsNullOrEmpty())
    {
        Console.WriteLine("请输入nuget上传需要的key：");
        nugetkey = Console.ReadLine();
    }
}