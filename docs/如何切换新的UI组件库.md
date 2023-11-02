我们最初用的mudblazor，不过它组件不够丰富，比如表格树、下拉树都没有
后来试了下bootstrapblazor，组件丰富，不过封装得有点狠，比如那个表格 curd全封装了，不过在我们的项目中，完全用不上，
虽然它支持自定义，不过我们都得自定义，反而显得它不干净了。
下面试试antblazor，顺便记录如何去掉原来的，使用新的。

bxjg.utils.rcl是抽象的组件，只包含逻辑部分，不包含ui部分。

原本的BXJG.AbpBootstrapBlazor中的组件集成 bxjg.utils.rcl的组件，在抽象的基础上加了与 bootstrap相关的东东，如：操作提示
所以删除原来的，然后新增一个 BXJG.AbpAntBlazor，这次里面的类命名为统一的，刚才的库默认命名空间改为：BXJG.AbpBlazor，方便以后替换，里面的类也按这个规则命名，而不是按之前的UI框架命名
然后 添加肉夹馍、定义全局异常拦截器，然后引用bxjg.utils.rcl等等
这个库 中定义 crud抽象组件，具体项目的crud往往继承这里的组件



然后创建后台管理的


common ui部分定义下拉框组件