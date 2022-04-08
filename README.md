<h1>迁移说明</h1>
当前项目中的模块是可以用的，通用树、附件管理、工单模块、微信小程序登陆等等..<br />
但项目整体编译会报错，因为它目前只是用来存放通用模块，另外对应的前端项目也没有在更新。<br />
目前在研究abp vNext + vue3 + element plus，会逐步把当前项目的模块迁移过去。<br />
[后端源码仓库](https://gitee.com/bxjg1987_admin/abp-vnext-api/) <br />
[前端源码仓库](https://gitee.com/bxjg1987_admin/abp-vnext-vue3-element-plus/)


[sdfsfd](https://www.baidu.com)

<h1>基本介绍</h1>
在线演示地址：http://web1.cqyuzuji.com:9000 账号密码：admin 123qwe<br />
这是一个基于.net6 abp7.x的系统，非vNext版本，目的是累积日常开发中需要的模块 实现快速变现的目的。模块尽量按abp的方式实现。目前已经有几个模块了(下面会说明)，后续会慢慢累积<br />
前后端分开，前端比较老的easyui，后端webApi接口。因为我对前端不熟，你可以按你喜欢的方式去实现<br />
需要熟悉asp.net core 6.x  abp<br />

<h1>说明</h1>
1. 早期项目是可以完整运行的，但由于个人精力有限，现在完整的项目已经无法运行。
1. 但是在其它项目中使用到的通用模块依然会在此仓库中新增和维护，然后发布为nuget包供其它项目使用。一来是累积模块，再者也在生产中得到验证。
1. 后期将切换到abp vNext上，那时会为每个可用模块编写完整的文档，目前你只能通过视频或源码了解设计思路，或直接联系我。

<h1>各模块基本介绍</h1>
目前没有整理文档，都是配套设计思路的视频

[工单管理模块](https://www.bilibili.com/video/BV1ky4y1b79u/)  
[商城模块->顾客](https://www.bilibili.com/video/BV1zy4y1k7uR/)  
[结合supersocket的设备控制功能](https://www.bilibili.com/video/BV1M5411j7Lo/)   
[微信小程序登录模块](https://www.bilibili.com/video/BV1Uk4y1d7Be/)  
[微信小程序支付模块](https://www.bilibili.com/video/BV1Ac411h7Jo/)  
[大文件分片上传/附件模块](https://www.bilibili.com/video/BV1m7411m7Es/)  
[通用树形结构模块](https://www.bilibili.com/video/BV1m7411m7Es?p=7)  

<h1>期待你的加入</h1>
abp是一个程序的框架，按它的模块化方式我们可以设计出适合常规业务的通用代码。如果你也在用abp，或在学习，那么我们可以一起来 **累积和分享 这些模块**，模块的设计我们可以一起探讨。
