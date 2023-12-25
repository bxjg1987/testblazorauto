一个接口多个实现参考：
https://github.com/castleproject/Windsor/blob/master/docs/registering-components-one-by-one.md#register-more-components-for-the-same-service
https://github.com/castleproject/Windsor/blob/master/docs/registering-components-one-by-one.md#supplying-the-component-for-a-dependency-to-use-service-override

https://github.com/castleproject/Windsor/issues/313

named注册
使用windsor api 手动解析

# Aop
ABP框架本身使用了动态代理的AOP方式，参考：https://aspnetboilerplate.com/Pages/Documents/Articles/Aspect-Oriented-Programming-using-Interceptors/index.html
相对于静态植入来说，它的性能更低。

另外经过测试，也许是测试方式不对，在blazor中使用无法拦截组件方法。

postsharp metalam收费不开源，但是最好的方式；
source genarator搞不定；

基于fody 静态植入的库，综合对比选择 肉夹馍https://github.com/inversionhourglass/Rougamo

由于是静态编织，猜测是哪个项目需要定义拦截器，哪个项目就需要引用 肉夹馍和fody配置文件

## blazor server中的 异常处理拦截器
日志记录：由于abp中日志记录是直接使用log4的，所以从日志记录来看，拦截器可以定义在BXJG.Utils中，因为它是最底层引用abp的库。
界面提示是根具体ui相关的，但在拦截器中 获取snakbar是不行的，消息提示的snakebar经过测试必须 inject到某个组件上才能提示，且只能用inject方式。
这里考虑仅仅是用户做某个操作异常了，界面提示，所以用abp的通知系统也不合适。


>可能考虑用blazor前后端分离方式，所以blazor中不要使用abp的事件总线。

>如何在电路中存储状态：https://github.com/dotnet/aspnetcore/issues/21738

没有找到比较合适的办法在电路中存储数据，变通办法是在App中生成一个guid

blazor server 中的组件事件 并不一定在主线程中，因此AsynLocal存储全局状态不行
注意：目前只关注界面部分的拦截器

组件中全局状态可以通过App传递级联参数

拦截器可以通过IocManager.Instance.CreateScope()进而获取服务

那如果拦截器需要访问 组件中的状态 或者 想获取当前界面的全局状态呢？
上面说了通过AsynLocal是不行的，拦截器本身仅仅是一段逻辑，它自身是无法在两次调用间共享数据的，
能否获取一个服务，然后通过服务获取状态呢？
貌似不行，还是因为AsynLocal不可用，拦截器也无法存储标识。

那只能通过context.Target入手了。通过级联传递全局参数到各组件，组件通过context.Target传入状态

批量注册拦截器
参考肉夹馍文档，还不够，我们的需求是 生命周期方法和事件处理函数才需要做异常处理拦截，其它方法不需要
所以不做批量注册，在抽象类中的生命周期方法上加入拦截器，而其它事件处理函数因为是用户定义的，因此不做拦截