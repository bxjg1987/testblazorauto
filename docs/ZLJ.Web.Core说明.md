原本abp中web.core是考虑 多个Host情况下，web.core是写跟web相关的、但与具体host无关的功能内容。

在我们的项目中，是一个项目托管动态webapi和blazor，所以Host项目才是项目的起点。
所以它失去了原本的意义，

跟web相关的初始化工作 移动到host项目中，

现在它变成了    跟web相关的公共库  +   rcl，每个应用的ui层都可以引用它。
注意rcl并不是特指blazor，而 是包含razorpage mvc 和blazor的。
注意此项目不引用具体的ui框架，比如 zlj.web.admin 引用mudblazor，但web.core不引用具体ui，因为不是所有ui项目 或者永远 使用mudblazor