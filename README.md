<h1>基本介绍</h1>
在线演示地址：http://git.weilaikj.cn 账号密码：admin 123qwe<br />
这是一个基于asp.net core abp5.x的系统，非vNext版本，目的是累积日常开发中需要的模块 实现快速变现的目的。模块尽量按abp的方式实现。目前已经有几个模块了(下面会说明)，后续会慢慢累积<br />
前后端分开，前端比较老的easyui，后端webApi接口。因为我对前端不熟，你可以按你喜欢的方式去实现<br />
需要熟悉asp.net core 3.x  abp<br />

<h1>快速上手</h1>
下载源码后vs2019打开<br />
项目结构说明：没啥好说的，就是abp结构。卸载xxx.Web.Mvc、xxx.WeChat.ABP 前者不要了，后者以后也许会用<br />
迁移：根据情况决定是否修改xxx.Migrator中的数据库连接字符串，设置xxx.Migrator为启动项目，直接F5运行<br />
启动：设置xxx.Web.Host为启动项目（注意连接字符串修改），启动它，这就是后端接口<br />

前端源码地址：[https://gitee.com/bxjg1987/front](https://gitee.com/bxjg1987/front)
启动UI：右键WebAdmin在浏览器中运行 admin 123qwe登录<br />

<h1>各模块基本介绍</h1>
目前没有整理文档，都是配套设计思路的视频<br />
<h3>微信小程序登录模块：</h3>
https://www.bilibili.com/video/BV1Uk4y1d7Be/<br />
<h3>微信小程序支付模块：</h3>
https://www.bilibili.com/video/BV1Ac411h7Jo/<br />
<h3>大文件分片上传/附件模块：</h3>
https://www.bilibili.com/video/BV1m7411m7Es/<br />
<h3>通用树形结构模块：</h3>
https://www.bilibili.com/video/BV1m7411m7Es?p=7<br />
<h3>简单商城模块</h3>
shop文件夹中是目前正在做商城模块，大致思路是尽量保持独立模块，主程序能尽量简单的引入和去除商城功能。主要是参考nopCommerce..<br />

<h1>期待你的加入</h1>
abp是一个程序的框架，按它的模块化方式我们可以设计出适合常规业务的通用代码。如果你也在用abp，或在学习，那么我们可以一起来 **累积和分享 这些模块**，模块的设计我们可以一起探讨。
