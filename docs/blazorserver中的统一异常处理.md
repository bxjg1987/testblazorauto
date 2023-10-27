
https://www.cnblogs.com/jionsoft/p/17783675.html

背景
用户做一个操作往往对应一个方法的执行，而方法内部会调用别的方法，内部可能又会调用别的方法，从而形成一个调用链。我们一般是在最顶层的方法去加try，而不是调用链的每一层都去加try。

在web开发中，用户的一个操作通常对应一个http请求，常见的mvc中一个controller的action会来执行这个处理。由于asp.net core是基于中间件管道的，很常见的方式就是定义一个“异常处理中间件”，它用try包住后续中间件的执行，在catch中捕获异常，记录日志，并返回一个统一的异常json结构返回给调用方。
一般我们会认为“全局异常处理中间件”是一个兜底的方式，我们仍然应该在action中去try，因为那是具体业务逻辑处理的起点，不过我觉得木有必要，完全可以定义一个UserFriendlyException代表业务逻辑异常，而系统默认的Exception认为是系统级异常，在action的调用链中，不需要try，只是发现不满足业务规则时： throw new UserFriendlyException 即可，而其它系统异常我们完全可以不考虑，最后在“全局异常处理中间件”中判断异常类型，若是UserFriendlyException则直接返回ex.Message给前端，不用做日志记录；系统级异常则应该记录异常堆栈日志，并返回一个友好的异常消息给前端。

Blazor server中的默认异常处理及其问题
在blazor server中，首次请求后服务端和浏览器建立了长连接websocket，后续的浏览器和服务端的交互没有类似http这种请求响应了，那在哪里做全局异常拦截呢？

blazor中提供了ErrorBoundary组件，基本格式如下：

<ErrorBoundary @ref="errorBoundary">
    <ChildContent>
        @Body
    </ChildContent>
    <ErrorContent>
        <p class="errorUI">😈 A rotten gremlin got us. Sorry!</p>
        <button @onclieck="fanhui">返回</button>
    </ErrorContent>
</ErrorBoundary>
@code { 
　　private ErrorBoundary? errorBoundary; 
　　protected void fanhui() { 
　　　　errorBoundary?.Recover(); 
　　} 
}
当ChildContent中的未发生异常时就显示ChildContent中的组件，若ChildContent中的组件发生了异常，则ChildContent隐藏ErrorContent会被显示出来。可以点击返回重写渲染ChildContent。具体的参考它的文档：处理 ASP.NET Core Blazor 应用中的错误

它的问题是当errorBoundary?.Recover(); 后，ChildContent中的组件是重新渲染的，之前的临时状态会丢失，比如我在填写一个表单，提交时异常了，返回后又得重新填写。

BootstrapBlazor中的异常处理及其问题
BootstrapBlazor是一套bootstrap风格的blazor ui组件库，它提供了全局异常处理方式，参考文档

它的方式是在根组件上截获异常，这样应用中的任何组件发生异常时它都能处理，与blazor自带的ErrorBoundary的最大区别是，bootstarpblazor会记录日志，并使用消息提示窗口提示用户发生了异常，而原本界面不会被替换掉。

尽管如此，它的设计思路仍然是兜底方式，也就是说异常发生时，它不会尝试恢复控件原本的临时状态，比如：一个按钮 点击后 显示正在加载...且按钮被禁用，然后加载数据时发生异常，全局异常会执行日志记录，并弹出提示用户，但按钮会一直显示“正在加载”，且按钮时禁用的，此时用户只能关闭此组件，重新打开。

解决思路
综上所述，无论是blazor自带的还是ui库提供的全局异常处理方案，在异常发生时用户要么需要刷新页面，要么需要重新打开组件。关键问题在于blazor server不像mvc那种基于http请求响应的方式，无法像asp.net core本身的那种定义全局异常处理中间件，所以我们还是需要手动处理blazor server中的每一个顶层方法，这样整个应用层序中会出现大量的try...,，顶多我们可以定义一个执行委托的方法，它类似这样的代码：

 1 public T Execute<T>(Action act){
 2       try{
 3             act();
 4       } 
 5       catch(UserFriendlyException ex)
 6       {
 7             //提示用户
 8       }
 9       catch(Exception ex)
10       {
11           //记录日志
12           //提示用户 
13      }
14 }
原本要执行的方法使用此方法来执行，从而实现统一异常处理，但这种方式仍然繁琐。

razor组件中的顶层方法一般有两种，一种是生命周期事件函数，另一种是我们为控件定义的事件处理程序，我们可以使用aop的方式拦截这两种方法，这样大部分代码都不需要手动处理异常了。

修改IL的AOP之肉夹馍
 Fody是一个开源库，它简化了IL植入代码的方式，肉夹馍是基于dofy实现的aop框架，我们也不需要关系Fody，直接按肉夹馍方式做拦截器即可。

假设你有个类库项目叫：ClassLibrary1，那么它通过如下方式引用肉夹馍的nuget
<PackageReference Include="Rougamo.Fody" Version="2.0.0" IncludeAssets="all" PrivateAssets="contentfiles;analyzers" />
然后在ClassLibrary1中定义肉夹馍拦截器，其它项目在引用ClassLibrary1时，就无需再引用肉夹馍的包了，仅仅引用ClassLibrary1即可。

aop/肉夹馍不仅仅用在这个场景，我们这里只是简单描述。它详细文档请参考官方文档：inversionhourglass/Rougamo: An AOP component that takes effect at compile time, similar to PostSharp. (github.com)

实现
基于肉夹馍的统一异常处理拦截器定义

/// <summary>
/// 基于abp和bootstrapblazor的全局异常处理拦截器
/// </summary>
public class AbpBBExceptionAttribute : MoAttribute
{
    //public override AccessFlags Flags => AccessFlags.Method;

    /*
     * 省略访问修饰符标识拦截所有方法
     * 返回类型* 就是忽略
     * 继承于Microsoft.AspNetCore.Components.ComponentBase的所有子类
     * 的所有方法
     */
    public override string? Pattern => "method(protected * BXJG.Utils.Components.AbpBaseComponent+.*(..))";

    //public override Feature Features => Feature.Observe;//加了这个就不灵了，不晓得为啥
 //   ComponentBase
 

    const string scopedServicesKey = nameof(scopedServicesKey);// "scopedServices";
    const string loggerKey = nameof(loggerKey);
    const string snackbarKey = nameof(snackbarKey);
    //const string isIntercepKey = nameof(isIntercepKey);//是否拦截

    public override void OnEntry(MethodContext context)
    {
        var services = IocManager.Instance.CreateScope();
        context.Datas.Add(scopedServicesKey, services);

        var loggerFactory = services.Resolve<ILoggerFactory>();
        var logger = loggerFactory.Create(context.TargetType.FullName);

        context.Datas.Add(loggerKey, logger);

        var temp = context.Target.GetType().GetProperty("MessageService", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public).GetValue(context.Target);
        var snackbar = temp as MessageService;
        context.Datas.Add(snackbarKey, snackbar);
   }

    public override void OnException(MethodContext context)
    {
        var snackbar = context.Datas[snackbarKey] as MessageService;
        //Task t;

        if (context.Exception is UserFriendlyException)
        {
            snackbar.Show(new MessageOption
            {
                Content = $"错误！{context.Exception.Message}",
                Color = Color.Danger,
                ShowBorder = true,
                ShowShadow = true
            });
        }
        else
        {
            var logger = context.Datas[loggerKey] as ILogger;
            logger.Error(@"{context.TargetType.FullName }.{context.Method.Name}" + context.Exception.StackTrace);

            //  (  context.Target as ComponentBase).tryinv

            snackbar.Show(new MessageOption
            {
                Content = $"服务端发生未处理异常！请稍后重试，若多次失败，请联系系统管理员11。",
                Color = Color.Danger,
                ShowBorder = true,
                ShowShadow = true
            });
            //snackbar.Add($"服务端发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。", Severity.Error);
        }

        //if (context.ReturnValue is Task xx)
        //{
        //    xx.ContinueWith(async cccc => await t);
        //}
        //else if (context.ReturnValue is ValueTask xx1)
        //{
        //    xx1.AsTask().ContinueWith(async cccc => await t);
        //}

        // 处理异常并将返回值设置为newReturnValue，如果方法无返回值(void)，直接传入null即可
        context.HandledException(this, context.RealReturnType.GetDefaultValue());
    }

    public override void OnExit(MethodContext context)
    {
        (context.Datas[scopedServicesKey] as IScopedIocResolver)!.Dispose();
    }
}

//根据类型Type获取默认值的扩展方法
/// <summary>
/// 获取类型默认值
/// </summary>
/// <param name="type"></param>
/// <returns></returns>
public static object? GetDefaultValue(this Type type)
{
    if (type.Name.ToLower() == "void")
        return null;

    if (type.IsValueType)
        return RuntimeHelpers.GetUninitializedObject(type);

    return null;
}
public override string? Pattern => "method(protected * BXJG.Utils.Components.AbpBaseComponent+.*(..))";这个代码指明此拦截器可以用于：方法、且访问修饰符必须是protected的、返回类型是任意的、且必须是 BXJG.Utils.Components.AbpBaseComponent的子类（因为我的项目定义了统一的这个抽象类，你自己应该根据自己项目去定）、方法名是任意的、参数是任意的。

上面我们仅仅是定义了拦截器，且限定了能用于哪些方法，在需要使用此拦截器的项目中，随便找个类，在namespance上面一行加入：

using System.Text;
using System.Threading.Tasks;

[assembly: AbpBBException]

namespace BXJG.AbpBootstrapBlazor
{
    [DependsOn(typeof(BXJG.Utils.BXJGUtilsRCLModule))]
    public class AbpBootstrapBlazorModule: AbpModule
    {
这表示AbpBootstrapBlazorModule这个类所在的程序集中的类（具体哪些类拦截器中的Pattern规定了）都会应用这个拦截器。若组件中某些方法不是顶层方法，应该定义为private，或加[IgnoreMo]，则这个方法就不会被拦截了。丢几个图，看看效果：







总结
总的来说blazor中，自带的异常处理方式仅仅是个兜底的方式，在实际项目中并不好用，无法减轻我们做异常处理的工作量。我们还是需要手动为每个顶层方法（一般是组件生命周期方法和其它事件处理程序）实施try做异常处理，不过我们可以使用aop来简化这个操作。