这里是将原来的mssql替换为mysql，如果要回退就反向操作即可

# 安装mysql
安装mysql 社区吧 server版本 8.1  用安装程序方式更简单。
一路下一步，配置root密码

配置时选择开发模式，这样mysql服务占用内存更少。
正式部署时肯定选择服务器模式

# abp集成mysql
按官方教程一步步搞：https://aspnetboilerplate.com/Pages/Documents/EF-Core-MySql-Integration
文档中的这个小节：A workaround   没做任何处理

hangfire mysql不好整，目前还是保留的sqlserver方式，
将来换成：https://github.com/marcoCasamento/Hangfire.Redis.StackExchange
或花钱买官方的，或者换成quartz

分布式锁，由于我们希望只做单体，所以原本的分布式锁DistributedLock.SqlServer引用了包，但是没使用
下载并配置DistributedLock.MySql也很简单，不过目前确实不打算使用分布式锁

# 数据库迁移
删除ZLJ.EntityFrameworkCore中的迁移Migrations文件夹
先把ZLJ.Web.Host设为启动项，迁移前选择ZLJ.EntityFrameworkCore
执行add-migration xxx  生成适合mysql的迁移文件
之后启动ZLJ.Migrator项目迁移数据库

# 启动
ZLJ.WEB.Host启动前，
>注意ZLJ.Core中的用户机密是否设置了连接字符串，本项目中的用户机密是在这里定义的