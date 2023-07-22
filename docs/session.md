abp的session请参考官方文档
标准文档：https://aspnetboilerplate.com/Pages/Documents/Abp-Session
扩展文档：https://aspnetboilerplate.com/Pages/Documents/Articles%5CHow-To%5Cadd-custom-session-field-aspnet-core

我们的框架实现了单体多应用，本文只说明多应用中的session处理，关于多应用，请参考：多应用.md

我们的框架是多种web框架共存的，mvc razorpages webapi和blazor server

# 多应用
比如目前系统有后端管理端、维修师傅端、客户服务平台，通常这种每个端都对应一个应用（app），
注意同一个端的移动端和pc网页端看成一个app

每个app都知道自己的appkey，可以硬编码，某些公共的应用服务功能方commonapp中的，它可能需要使用当前appkey，此时应该由具体的app调用时传递过去。
但某些时候可能真需要在公共地方访问当前appkey，比如某些扩展方法中。

这些端的用户不同，所处的环境不同，比如 客户服务平台，他的几乎所有的接口都需要一个所属客户id这么个字段，这个字段最好是放在session中

不想动到abp自身session的设计。

# 扩展session方式分析
扩展：参考abp提供的扩展session的思路，参考顶部的连接，这个是单独做个session类，需要时注入即可，
原本以为扩展方法IAbpSession，但发现它没暴露claim相关属性，所以不太好扩展。

abpseesion自身是不存储数据的，而是通过类似委托的方式从原始位置获取的
用户的获取是从当前用户的票证获取的，具体如何实现的懒得研究源码，
另外abpsession的租户id的获取是从租户提供器获取的，而不是当前登陆信息中获取，因为某些接口可能调用时用户都没登陆。

我们实现session支持appkey，也是一样的方式. 并且appkey的获取跟租户id有类似的特征，不要从当前登陆信息中获取

# 不要在blazor server的组件中访问HttpContext
参考：
https://learn.microsoft.com/zh-cn/aspnet/core/blazor/security/server/threat-mitigation?view=aspnetcore-7.0#avoid-ihttpcontextaccessorhttpcontext-in-razor-components
blazor server中，加密cookie提交，转换为票证，abp session中用户id是从票证中获取的。
租户id是从请求路径 headers 或当前用户所属租户来的。
所以abp自身的session是木有问题的。


# 原来的实现
参考上面说的，原本的实现有隐患，但是好像能用，看后续分析发现也没问题
应用识别中间件在当前请求上下文存储appkey

独立的session，参考abp扩展session的思路，common中定义抽象的session，各子类实现。
抽象中接收一个Func<AppInfo>的委托，它是在Host Module或Startup中注入的，
```
 IocManager.RegService(services => {
                services.AddSingleton(() => {
                    return httpContextAccessor.HttpContext?.GetApp();
                    //if (httpContextAccessor.HttpContext!.Items.TryGetValue("appKey", out var appKey))
                    //{
                    //    return Configuration.Modules.CommonApplication().Apps[appKey!.ToString()];
                    //}
                    //return default;
                });
            });
```
# 原来实现的分析
appkey跟租户id有很多相似之处，所以也可以使用一样的实现
在登陆时从httpcontext获取完全没问题，以为此时还没有blazor server

登陆后直接从加密cookie中获取，应该也没问题，所以改下，若当前用户存在，则从加密cookie中获取，否则从httpcontext获取


# 租户id的获取 vs appkey的获取
租户id是通过一堆租户id解析器，在需要时去调用的
appkey是通过中间件 先分析出appkey，存储在httpcontext，然后从session获取时，先尝试从当前用户获取，若没有则从httpcontext获取

# 动态变动的session

注：最终决定类似常见前后端分离时由前端存储这种值，blazor server中由 级联值，在顶层组件存储

租户id 应用id是在登录时都需要的，在应用使用过程中不变
用户id是登录后确定的，在应用使用过程中不变
而代码生成项目中的 当前项目，是在登录后，且应用使用过程中可能动态变化的，
木有可能不需要登录 还动态切换的，那个不叫session了

普通的session数据要么在请求url 头 或claim中，这种动态切换的如何存储呢？
这种动态seesion访问频率较高，不要考虑io操作
还得考虑前后端分离，和blazor server

基于这些特点，在commonapp中抽象的session中定义个字典，用户id作为key，dic作为值，
不过最好直接扩展IAbpSession，这样这是个通用功能

如何移除呢？
前后端分离的，只能用过期策略，blazorserver
好麻烦。

# 多用户的分析
abp只得一个User，框架模板中的LoginManager Usermanager都指向的这个User
多应用有自己的User，都实现User，这样可以公用原来的LoginManager和UserManager
由于我们现在用三层，UserManager没啥用了。

重点是后台管理到底是用员工还是原来的User
原来的User保持不动，后台管理可以默认使用员工
