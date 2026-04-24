# Libs 项目待处理问题清单

> 生成时间：2026-04-20  
> 最后更新：2026-04-21（修复P0方括号hack、TimeNow Interval、LinqExt签名冲突、IOptionsMonitor捕获；枚举Flags问题不处理）
> 基于最新 master 分支代码核对  
> 本文档仅记录**仍需处理**的问题，已修复的问题不再列出

---

## 严重错误（P0 - 需立即修复）

无

---

## 急需优化的问题（P1 - 建议本周修复）

### 4. 项目中大量枚举误用 [Flags] 特性（不处理）
**项目**: 多个项目  
**问题描述**: 项目中有14个枚举带有 [Flags] 特性，其中不少不应是 Flags 枚举，其值并非2的幂或语义上不支持位组合。典型例子：Gender（性别不应支持位组合）、WeekDay（值为0-6递增非2的幂）、AdministrativeLevel（值为0-2递增）、TenantAvailabilityState（值3不是2的幂）、XZQ（值非2的幂且已标Obsolete）。问题14仅修复了微信库中的2个枚举，其余未处理。  
**修复方向**: 对语义上不支持位组合的枚举移除 [Flags] 特性，确保值定义正确。  
**当前状态**: 不处理（涉及项目范围广、改动量大、风险不确定，暂不处理）

---

## 已修复的问题（不再处理）

以下问题在最新代码中已被修复，本文档不再列出：

1. ✅ CertificateDefaultProvider - LINQ Select 延迟执行导致证书解密从未发生
2. ✅ CertificateDefaultProvider - 后台任务生命周期失控（已拆分为CertificateRefreshService，继承BackgroundService，由DI容器管理生命周期，支持优雅停止）
3. ✅ CertificateDefaultProvider - 竞态条件（wxCertificateResult字段已添加volatile，确保后台线程更新后请求线程立即可见）
4. ✅ CertificateDefaultProvider - Single() 后的 null 检查是死代码（已改为FirstOrDefault()，找不到证书时返回null而非抛出InvalidOperationException，null检查不再死代码）
5. ✅ SecretHelper - AesGcmDecrypt 解密结果包含多余字节
6. ✅ SecretHelper - 选项字段配置变更不生效
7. ✅ SystemExtensions.ToHexString - 无限循环
8. ✅ SystemExtensions.CaptureClipFromLoop - 无限递归
9. ✅ SystemExtensions.AesGetKey - 不安全的密钥派生
10. ✅ WeChatMessageService - access_token 已从请求体移至URL查询参数
11. ✅ LoginMiddleware - 反序列化和Handler为null经确认不会发生，响应由实现类内部处理
12. ✅ PayNotifyMiddleware - 验签失败抛异常返回500，属于5XX范围，微信会重发通知不造成业务错误，仅应答格式不够规范
13. ✅ PayNotifyMiddleware - GetService改为GetRequiredService，未注册时抛出清晰异常
14. ✅ Enums - 移除[Flags]，formal设为0作为默认值，添加JsonStringEnumConverter确保序列化为字符串
15. ✅ BXJG.Common.EFCore - 空壳项目暂不处理，虽无源码但仍有下游引用
16. ✅ TimeNow.razor - 实现IAsyncDisposable，DisposeAsync中释放Timer
17. ✅ BXJGBlazorECharts SetOption忙等待轮询暂不处理
18. ✅ BXJGBlazorECharts DisposeAsync中JS互操作添加try-catch保护JSDisconnectedException和ObjectDisposedException
19. ✅ BXJGBlazorECharts.razor.js getInstanceByDom返回值添加null检查
20. ✅ OperationAuthorizationRequirement 权限提供者异常处理暂不处理，ASP.NET Core授权管道有默认异常处理
21. ✅ BrowserConsoleLoggerProvider 整个Logger未启用（DI注册也被注释），暂不处理
22. ✅ LinqExt 动态LINQ字符串拼接为设计需要，开发者已标注不存在注入风险
23. ✅ IEnumerableExtensions.HasChange 无活跃调用，暂不处理
24. ✅ IOptionsMonitor.CurrentValue构造函数捕获问题暂不处理，微信相关配置运行期间不会动态变更
25. ✅ SecretHelper 用Lazy&lt;ICertificateProvider&gt;替代IServiceProvider服务定位器，消除反模式同时规避循环依赖
26. ✅ SignDelegatingHandler 移除手动创建HttpClientHandler，由IHttpClientFactory自动管理
27. ✅ StateDto 静态只读实例属性setter改为private，防止外部修改
28. ✅ MiniProgramApiService Code2Session已传入CancellationToken
29. ✅ ServiceV3 修改入参对象为业务需要（mchid和notify_url由服务端控制），暂不处理
30. ✅ PayNotifyMiddleware 请求头转字典改用OrdinalIgnoreCase和FirstOrDefault安全取值
31. ✅ DIExt IAccessTokenProvider as转换不会返回null，注册和取用在同一方法中实现类型确定
32. ✅ IAuthorizationPolicyProvider注册为Transient仅多些内存分配，不影响功能，暂不处理
33. ✅ MemoryCacheHelper容量检查非原子操作，Blazor WebAssembly单线程环境竞争概率极低，暂不处理
34. ✅ YanchiChuli已标记Obsolete，推荐使用第三方库替代，暂不处理
35. ✅ Zhongjie事件触发异常处理为设计选择，调用方可自行处理，暂不处理
36. ✅ StaticDIAccessor已标记Obsolete，暂不处理
37. ✅ PersistentAuthenticationStateProvider Token不刷新为Blazor WebAssembly已知限制，实现刷新需较大改动，暂不处理
38. ✅ WeChatMessageService - JSON序列化后粗暴替换方括号hack已移除（TemplateDataItem本身序列化为JSON对象格式，无需Replace hack）
39. ✅ TimeNow.razor - Interval参数未生效已修复（Timer创建时使用Interval参数替代硬编码1000）
40. ✅ ConditionFieldDefineExt.cs - 死代码文件已删除（该文件已在csproj中Compile Remove排除，不参与编译，与LinqExt同名方法无实际冲突）
41. ✅ IOptionsMonitor.CurrentValue构造函数捕获问题 - 6处已统一改为IOptions&lt;Option&gt;（CertificateDefaultProvider、PayNotifyMiddleware、ServiceV3、LoginMiddleware、WeChatMessageService、CodeGenerator.MainService），语义更准确，与SecretHelper/AccessTokenProvider的正确做法保持风格一致
