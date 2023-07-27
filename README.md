# 介绍
基于.net7.x abp blazor的快速开发框架基础，
先提供了帮助类的通用功能库，然后使用abp提供的业务框架能力，再进一步提供基于abp的扩展模块，最后使用mudblazor提供界面，参考下面的特征列表。

>此仓库一直在更新，你此前看到的视频或文章可能已经过时。wiki中的文档也可能与最新代码不匹配。


# 技术和特征
1. [dotnet7.x](https://learn.microsoft.com/zh-cn/aspnet/core/?view=aspnetcore-7.0)
[abpzero](https://aspnetboilerplate.com/) (也就是老版本，不是vNext)
mudblazor6.x
sqlserver2012+ 
redis7.x 
1. 单体多应用。一套系统多种用户类型，需要多个端进入系统做不同业务处理，每个端作为一个应用。
1. 通用树抽象。轻松实现一个新的无线层次结构的数据的crud。默认实现通用的数据字典。
1. 小程序登录、小程序支付。
1. 简单的通用附件
1. cap集成
1. 同时支持blazor和webapi
1. 扩展abp权限实现依赖权限，A权限有个子权限A1,当给用户授予A权限时，自动授权A1权限


# 文档
见[wiki](https://gitee.com/bxjg1987_admin/abp/wikis)


# 以前的部分模块介绍
>此仓库一直在更新，你此前看到的视频或文章可能已经过时。wiki中的文档也可能与最新代码不匹配。
[工单管理模块](https://www.bilibili.com/video/BV1ky4y1b79u/)  
[商城模块->顾客](https://www.bilibili.com/video/BV1zy4y1k7uR/)  
[结合supersocket的设备控制功能](https://www.bilibili.com/video/BV1M5411j7Lo/)   
[微信小程序登录模块](https://www.bilibili.com/video/BV1Uk4y1d7Be/)  
[微信小程序支付模块](https://www.bilibili.com/video/BV1Ac411h7Jo/)  
[大文件分片上传/附件模块](https://www.bilibili.com/video/BV1m7411m7Es/)  
[通用树形结构模块](https://www.bilibili.com/video/BV1m7411m7Es?p=7)  

