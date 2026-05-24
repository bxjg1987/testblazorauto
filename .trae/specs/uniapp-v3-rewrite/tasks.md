# Tasks

## 第一阶段：项目初始化与技术栈搭建

- [ ] Task 1: 使用 wot-starter v2 模板创建项目
  - [ ] 1.0 确认开发环境：Node.js >= 20.19.0 || >= 22.12.0 || >= 24.0.0（推荐 LTS 偶数版本），pnpm 已安装
  - [ ] 1.1 执行 `pnpx degit wot-ui/wot-starter#v2`（推荐）或 `pnpm create uni@latest <name> -t wot-starter-v2` 创建项目骨架
  - [ ] 1.2 确认生成的基础文件结构正确（src/pages/、src/layouts/、src/store/、src/router/、src/composables/、uno.config.ts、pages.config.ts、manifest.config.ts 等）
  - [ ] 1.3 确认 package.json 中所有依赖为稳定版本（无 alpha/beta）

- [ ] Task 2: 调整依赖与安装额外工具库
  - [ ] 2.1 移除 wot-starter 自带的 Alova：卸载 `alova`、`@alova/adapter-uniapp`、`@alova/mock`、`@alova/shared`、`@alova/wormhole`，删除 `alova.config.ts`，从 `vite.config.ts` 的 AutoImport 中移除 `usePagination`、`useRequest` 等 `alova/client` 相关导入，检查 `eslint.config.mjs` 和 `tsconfig.json` 中是否有 alova 相关类型引用并移除，删除 `package.json` 中 `alova-gen` 脚本
  - [ ] 2.2 确认 @wot-ui/ui 版本为 V2（wot-starter v2.0.0 已预装，无需手动替换）
  - [ ] 2.3 将 @wot-ui/unocss-preset 从 beta 版升级到 1.0.0 稳定版，升级后在 H5 和小程序端分别验证 wot- 前缀的原子类（如 wot-bg-primary）能正常渲染
  - [ ] 2.4 确认 Sass 版本为 ^1.99.0（wot-starter v2.0.0 已预装，满足 Wot UI V2 依赖要求）
  - [ ] 2.5 安装网络库：`@uni-helper/uni-network`。注意：npm 公共仓库最新版本为 0.21.x，如安装失败需确认版本可用性或指定可用版本号
  - [ ] 2.6 安装工具库：`dayjs`（用于 ABP 后端时间戳格式化、相对时间显示等场景，简单格式化优先使用 @vueuse/core）、`lodash-es`（仅用于 @vueuse 未覆盖的深拷贝、复杂对象操作等场景，非通用工具需求优先使用 @vueuse/core 或原生方法）
  - [ ] 2.7 确认图表库已预装：echarts ^6.x、uni-echarts（wot-starter v2.0.0 已预装，无需手动安装）
  - [ ] 2.8 确认组件自动导入配置正确指向 @wot-ui/ui（wot-starter v2.0.0 已预配置，无需手动修改）
  - [ ] 2.9 确认 tsconfig.json 中 Volar 类型引用正确，并添加 `uni-echarts/global`
  - [ ] 2.10 确认 `src/resolver.ts`（Wot UI 组件解析器，供 vite.config.ts 中 @uni-helper/vite-plugin-uni-components 引用，实现 Wot UI 组件的自动按需注册）已由 wot-starter 预置，且 vite.config.ts 中 WotResolver 引用路径正确
  - [ ] 2.11 在 vite.config.ts 中配置 uni-echarts：1) 从 `uni-echarts/vite` 导入 `UniEcharts` 并在 plugins 中添加 `UniEcharts()`；2) 从 `uni-echarts/resolver` 导入 `UniEchartsResolver` 并在 UniHelperComponents 的 resolvers 中添加 `UniEchartsResolver()`；3) 在 optimizeDeps.exclude 中添加 `uni-echarts`
  - [ ] 2.12 在 vite.config.ts 中配置 Sass modern-compiler API 以消除 legacy-js-api 弃用警告
  - [ ] 2.13 执行 `pnpm install` 安装所有依赖

- [ ] Task 3: 配置项目基础设置
  - [ ] 3.1 在 tsconfig.json 中确认启用 `strict: true`
  - [ ] 3.2 确认 `@/` 路径别名在 vite.config.ts 和 tsconfig.json 中均正确配置
  - [ ] 3.3 创建/更新 `src/env.d.ts` 类型声明文件（包含 uni-network 模块声明）

- [ ] Task 4: 配置 Vite 与 UnoCSS
  - [ ] 4.1 在 vite.config.ts 中确认 unocss 插件正确配置
  - [ ] 4.2 在 vite.config.ts 中配置开发服务器代理（/api 代理到后端）
  - [ ] 4.3 确认 uno.config.ts 同时包含 @wot-ui/unocss-preset 和 @uni-helper/unocss-preset-uni 两个预设
  - [ ] 4.4 创建/配置 `src/uni.scss`：全局 SCSS 变量文件，引入 Wot UI CSS 变量（品牌色、圆角、间距、字体等），供项目全局使用

- [ ] Task 5: 配置 pages.config.ts 与 manifest.config.ts
  - [ ] 5.1 配置 pages.config.ts（全局样式、导航栏、tabBar 等全局页面设置）。注意：wot-starter 使用文件路由（@uni-helper/vite-plugin-uni-pages），pages.config.ts 中 `pages: []` 保持为空——页面路由由 `src/pages/` 目录下的 .vue 文件结构自动生成，layout 通过页面组件中的 `<route>` 块声明，不需要在 pages.config.ts 中手动定义页面列表
  - [ ] 5.2 配置 manifest.config.ts 基础信息（应用名称、appid、权限、vueVersion 等）
  - [ ] 5.3 配置全局样式（navigationStyle 等）

- [ ] Task 5.5: 配置环境变量文件
  - [ ] 5.5.1 确认 wot-starter 自带的 `.env.development`、`.env.production`、`.env.staging` 文件存在
  - [ ] 5.5.2 在 `.env.development` 中添加 `VITE_API_BASE_URL` 变量，指向开发环境后端地址
  - [ ] 5.5.3 在 `.env.production` 和 `.env.staging` 中配置对应环境的 API 地址
  - [ ] 5.5.4 在 `src/env.d.ts` 中添加 `VITE_API_BASE_URL` 的类型声明

- [ ] Task 26: 清理 wot-starter 自带示例代码（依赖 Task 1，可与第一阶段其它任务并行）
  - [ ] 26.1 删除 wot-starter 自带的示例页面（subPages/、subEcharts/、subAsyncEcharts/ 等）
  - [ ] 26.2 删除示例 API 和 composables
  - [ ] 26.3 在 vite.config.ts 的 UniHelperPages 配置中移除 `subPages`、`subEcharts`、`subAsyncEcharts` 等已删除的分包路径，或在删除目录后确认 subPackages 配置自动失效
  - [ ] 26.4 保留核心框架代码（layouts、router、store 基础结构）

## 第二阶段：基础框架封装

- [ ] Task 6: 创建类型定义体系（`src/types/`）
  - [ ] 6.1 创建 `ajax-response.ts`：AjaxResponse、ErrorInfo、ValidationErrorInfo、BatchOperationOutput 等 ABP 标准响应类型
  - [ ] 6.2 创建 `menu.ts`：MenuDto、MenuItemDto、MenuGroupDto、MenuItemCustomData、MenuShowModel 等菜单类型
  - [ ] 6.3 确保所有类型导出，无 `any` 类型

- [ ] Task 7: 创建异常类体系（`src/utils/exceptions.ts`）
  - [ ] 7.1 UserFriendlyException（业务异常，含 code/message/details）
  - [ ] 7.2 UnauthorizedException（401 未登录）
  - [ ] 7.3 ForbiddenException（403 无权限）
  - [ ] 7.4 NetworkException（网络故障）
  - [ ] 7.5 TimeoutException（请求超时）
  - [ ] 7.6 BatchOperationException（批量操作部分失败）

- [ ] Task 8: 封装 HTTP 请求模块（`src/utils/http.ts`）
  - [ ] 8.1 基于 @uni-helper/uni-network 创建实例，对接现有 ABP 后端的 HTTP 封装逻辑
  - [ ] 8.2 请求拦截器：自动添加 JWT Token、请求去重
  - [ ] 8.3 响应拦截器：AjaxResponse 自动解包（识别 `__abp` 标记）、异常类型转换
  - [ ] 8.4 响应拦截器统一处理错误 UI 提示：UserFriendlyException 弹 Toast、BatchOperationException 弹 Modal、401 跳登录、403 弹权限提示、网络/超时弹对应提示
  - [ ] 8.5 请求选项支持 `silent: true`（需在 RequestOptions 接口中新增 `silent?: boolean` 字段），开启后拦截器不自动弹错误提示，由调用方自行处理
  - [ ] 8.6 导出 `request`、`get`、`post`、`put`、`del` 泛型方法
  - [ ] 8.7 导出 `setApiBaseUrl`、`getApiBaseUrl`、`cancelAllRequests` 方法
  - [ ] 8.8 所有方法有完整的 TypeScript 泛型类型

- [ ] Task 9: 创建全局错误处理器（`src/utils/error-handler.ts`）
  - [ ] 9.1 全局 Vue 错误捕获（app.config.errorHandler）
  - [ ] 9.2 H5 端：通过 `window.addEventListener('unhandledrejection')` 和 `window.addEventListener('error')` 捕获（使用 `#ifdef H5` 条件编译或运行时环境检测）
  - [ ] 9.3 小程序端：通过 `uni.onError` 等 uniapp API 捕获
  - [ ] 9.4 异常类型到用户提示的映射
  - [ ] 9.5 错误日志内存缓存（最多 50 条）
  - [ ] 9.6 单例模式

- [ ] Task 10: 创建 Pinia Store（`src/store/`）
  - [ ] 10.1 `user.ts`：Token、UserInfo、isLoggedIn、setToken、setUserInfo、loadTokenFromStorage、clearUserData、logout
  - [ ] 10.2 `app.ts`：ABP 配置、菜单数据、全局加载状态、setter 和 clearAppData
  - [ ] 10.3 所有状态和操作有完整 TypeScript 类型
  - [ ] 10.4 Token 和 UserInfo 同步到 uni.storage

- [ ] Task 11: 创建菜单工具函数（`src/utils/menu.ts`）
  - [ ] 11.1 `normalizeMenuUrl`：将 ABP 后端返回的菜单 URL 转为 wot-starter 平铺页面路由路径。注意：本项目采用平铺页面文件命名（如 `pages/login.vue`）而非子目录命名（如 `pages/login/index.vue`），因此 URL 转换不应追加 `/index` 后缀。例如 `/login` → `/pages/login`，`/settings/profile` → `/pages/settings/profile`
  - [ ] 11.2 `filterBottomMenus`：过滤底部导航菜单（mobileShowModel === 2）
  - [ ] 11.3 `filterSideMenus`：过滤侧边菜单（mobileShowModel === 4）
  - [ ] 11.4 `navigateToMenuItem`：菜单项导航（使用 @wot-ui/router）
  - [ ] 11.5 `isMenuItemActive`：判断菜单是否激活

- [ ] Task 12: 封装 ABP API 接口层（`src/api/`）
  - [ ] 12.1 `auth.ts`：登录认证（AuthenticateModel、AuthenticateResultModel、authenticate 函数）
  - [ ] 12.2 `session.ts`：获取当前登录信息（GetCurrentLoginInformationsOutput 及子类型、getCurrentLoginInformations 函数）
  - [ ] 12.3 `config.ts`：获取 ABP 配置（AbpUserConfigurationDto 及所有子类型、getUserConfiguration 函数）。注意：根据 ASP.NET Boilerplate 源码（`AbpUserNavConfigDto.cs`），后端类型为 `Dictionary<string, UserMenu>`，即键值对结构。现有代码中 AbpUserNavConfigDto 错误地定义为 `menu: any[]`（单数且类型为数组），重构时需修正为 `menus: MenuDto`（MenuDto 为键值对接口，等价于 `Record<string, MenuGroupDto | undefined>`，而非数组），与后端实际返回的 JSON 字段名和结构一致，消除调用方的 `as MenuDto` 强制断言
  - [ ] 12.4 `captcha.ts`：验证码相关
  - [ ] 12.5 所有 API 函数有完整 TypeScript 类型

- [ ] Task 12.5: 创建 Composables（`src/composables/`，目录由 wot-starter 预置或手动创建。注：wot-starter 的 unplugin-auto-import 配置中 `dirs: ['src/composables', ...]` 已包含此目录，composables 可被自动导入）
  - [ ] 12.5.1 `useAuth.ts`：登录（调用 auth API）、登出（调用 userStore.logout）、Token 管理、returnUrl 处理
  - [ ] 12.5.2 `useMenu.ts`：菜单数据响应式读取（从 appStore）、菜单过滤（底部/侧边导航）、菜单项导航、激活状态判断。useMenu 组合式函数重封装 menu.ts 工具函数和 appStore，使其可在任何组件中通过 AutoImport 自动导入使用，无需手动 import
  - [ ] 12.5.3 所有 composables 使用 Composition API 风格，有完整 TypeScript 类型

- [ ] Task 13: 创建应用初始化服务（`src/services/app-initialization.ts`）
  - [ ] 13.1 `initializeOnLaunch`：应用启动时从 storage 恢复 Token → 获取 ABP 配置 → 恢复用户信息
  - [ ] 13.2 `initializeAfterLogin`：登录后获取 ABP 配置和用户信息
  - [ ] 13.3 `reinitialize`：切换 API 地址后重新初始化 → 跳转首页
  - [ ] 13.4 单例模式，完整 TypeScript 类型

- [ ] Task 25: 编写路由导航守卫配置（`src/router/index.ts`）
  - [ ] 25.1 配置 @wot-ui/router 路由拦截（`beforeEach` / `afterEach`），检查登录状态。注：@wot-ui/router 基于 uni-app 路由能力提供近 Vue Router 风格的编程式导航与路由拦截，非声明式路由守卫
  - [ ] 25.2 注：wot-starter 使用文件路由（@uni-helper/vite-plugin-uni-pages），路由映射由 pages.config.ts + 文件结构自动生成，`router/index.ts` 仅用于配置导航守卫和导出路由实例，不包含路由表定义

## 第三阶段：布局与通用组件

- [ ] Task 14: 创建布局组件（`src/layouts/default.vue`）
  - [ ] 14.1 使用 wot-starter 布局系统，作为默认布局
  - [ ] 14.2 集成 Wot UI 组件（wd-navbar、wd-tabbar、wd-popup 等）构建布局
  - [ ] 14.3 支持自定义导航栏标题（通过路由 meta 传递）
  - [ ] 14.4 支持显示/隐藏顶部导航栏、底部 TabBar（通过路由 meta 控制）
  - [ ] 14.5 自动获取状态栏高度做安全区域适配
  - [ ] 14.6 使用 `<slot />` 渲染页面内容

- [ ] Task 15: 创建底部导航组件（`src/components/common/BottomNav.vue`）
  - [ ] 15.1 使用 Wot UI 的 wd-tabbar 组件
  - [ ] 15.2 从 appStore 读取菜单数据，过滤底部导航菜单（mobileShowModel === 2）
  - [ ] 15.3 固定包含"首页"和"我的"两个固定 Tab 项
  - [ ] 15.4 点击 Tab 切换页面，高亮当前页面对应 Tab
  - [ ] 15.5 使用 @wot-ui/router 进行页面跳转

- [ ] Task 16: 创建侧边菜单组件（`src/components/common/SideMenu.vue`）
  - [ ] 16.1 使用 Wot UI 的 wd-popup 组件作为侧边栏容器
  - [ ] 16.2 从 appStore 读取完整菜单数据，按分组展示
  - [ ] 16.3 支持子菜单展开/折叠（使用 wd-collapse 或自定义）
  - [ ] 16.4 点击菜单项使用 @wot-ui/router 导航并关闭侧边栏
  - [ ] 16.5 蒙层点击关闭
  - [ ] 16.6 Emit: close 事件

## 第四阶段：核心页面

- [ ] Task 17: 创建 App.vue 应用入口
  - [ ] 17.1 在 onLaunch 中判断 API 地址配置状态
  - [ ] 17.2 根据状态调用 appInitializationService.initializeOnLaunch()
  - [ ] 17.3 根据初始化结果跳转到对应页面（init/login/main）

- [ ] Task 18: 创建 main.ts 应用启动文件
  - [ ] 18.1 使用 createSSRApp 创建应用
  - [ ] 18.2 注册 Pinia
  - [ ] 18.3 注册全局错误处理器
  - [ ] 18.4 H5 端注册 `window.addEventListener('unhandledrejection')` 和 `window.addEventListener('error')`（使用 `#ifdef H5` 条件编译）
  - [ ] 18.5 小程序端通过 `uni.onError` 注册全局错误捕获

- [ ] Task 19: 创建登录页面（`src/pages/login.vue`）
  - [ ] 19.1 使用 Wot UI 组件（wd-input、wd-button、wd-image 等）构建表单
  - [ ] 19.2 租户、用户名、密码、验证码输入
  - [ ] 19.3 调用 authenticate API 登录
  - [ ] 19.4 登录成功后调用 appInitializationService.initializeAfterLogin
  - [ ] 19.5 支持 returnUrl 参数，登录后跳回原页面

- [ ] Task 20: 创建首页（`src/pages/index.vue`）
  - [ ] 20.1 使用 default 布局
  - [ ] 20.2 展示欢迎信息、统计卡片、快捷操作入口

- [ ] Task 21: 创建后台管理主页（`src/pages/main.vue`）
  - [ ] 21.1 使用 default 布局
  - [ ] 21.2 展示后台管理仪表盘内容

- [ ] Task 22: 创建个人中心页面（`src/pages/profile.vue`）
  - [ ] 22.1 使用 default 布局
  - [ ] 22.2 展示用户信息（头像、昵称、角色等）
  - [ ] 22.3 提供登出按钮

- [ ] Task 23: 创建 API 地址配置页面（`src/pages/init.vue`）
  - [ ] 23.1 输入后端 API 地址
  - [ ] 23.2 调用 appInitializationService.reinitialize 连接后端
  - [ ] 23.3 连接成功后跳转首页

- [ ] Task 24: 配置 Wot UI 主题变量（`src/theme.json`）
  - [ ] 24.1 定义品牌色（主色 #1890ff 等）
  - [ ] 24.2 定义通用 CSS 变量（圆角、间距、字体等）

# Task Dependencies

```
第一阶段（Task 1-5、Task 26）
  ├── Task 1: 创建项目（wot-starter）
  ├── Task 26: 清理示例代码（依赖 Task 1，可与 Task 2-5 并行）
  ├── Task 2: 调整依赖（依赖 Task 1）
  ├── Task 3: TS/基础配置（依赖 Task 1, 2）
  ├── Task 4: Vite/UnoCSS 配置（依赖 Task 1, 2）
  ├── Task 5: pages/manifest 配置（依赖 Task 1）
  └── Task 5.5: 环境变量配置（依赖 Task 1）
       ↓
第二阶段（Task 6-13、Task 12.5、Task 25）
  ├── Task 6: 类型定义（无依赖）
  ├── Task 7: 异常类（依赖 Task 6）
  ├── Task 8: HTTP 模块（依赖 Task 6, 7）
  ├── Task 9: 错误处理器（依赖 Task 7）
  ├── Task 10: Pinia Store（依赖 Task 6）
  ├── Task 11: 菜单工具（依赖 Task 6, 10）
  ├── Task 12: API 接口层（依赖 Task 8）
  ├── Task 12.5: Composables（依赖 Task 10, 11, 12）
  ├── Task 13: 应用初始化服务（依赖 Task 10, 12）
  └── Task 25: 路由配置（依赖 Task 1，与 Task 6-13 可并行）
       ↓
第三阶段（Task 14-16）
  ├── Task 14: 布局组件（依赖 Task 10, 11, 25）
  ├── Task 15: 底部导航（依赖 Task 10, 11, 14）
  └── Task 16: 侧边菜单（依赖 Task 10, 11, 14）
       ↓
第四阶段（Task 17-24）
  ├── Task 17: App.vue（依赖 Task 13, 25）
  ├── Task 18: main.ts（依赖 Task 9, 10, 25）
  ├── Task 19: 登录页（依赖 Task 12, 13, 14, 25）
  ├── Task 20: 首页（依赖 Task 14, 25）
  ├── Task 21: 主页（依赖 Task 14, 25）
  ├── Task 22: 个人中心（依赖 Task 10, 14, 25）
  ├── Task 23: 初始化页（依赖 Task 13, 14, 25）
  └── Task 24: 主题变量（无依赖）
```

- Task 6 和 Task 7 可并行
- Task 25 可与 Task 6-13 并行（仅依赖 Task 1）
- Task 26 可与 Task 2-5 并行（仅依赖 Task 1）
- Task 15 和 Task 16 可并行（在 Task 14 完成后）
- Task 17 和 Task 18 可并行
- Task 20-23 可并行（在 Task 14、25 完成后）
- Task 24 可与第一阶段并行