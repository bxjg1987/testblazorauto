# aop拦截器

abp的动态拦截器方式不行，貌似blazor组件木有注册到IocManager里，可能仅仅是注册到微软自己的ioc容器了。

所以postsharp metalama之类的才可以

变动的设计思路是提供抽象的组件类，重写所有事件方法，如：

init方法{
执行前
initCore();
执行后
}

public virtual void initCore()
{
}

