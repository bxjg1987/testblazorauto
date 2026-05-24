# Checklist

## 项目初始化
- [ ] 开发环境满足要求：Node.js >= 20.19.0 || >= 22.12.0 || >= 24.0.0，pnpm 已安装
- [ ] 项目通过 wot-starter v2 模板创建成功（`pnpx degit wot-ui/wot-starter#v2` 推荐）
- [ ] package.json 中所有依赖均为稳定版本（无 alpha/beta）
- [ ] 项目可正常执行 `pnpm dev` 启动 H5 开发服务器
- [ ] wot-starter 自带的 Alova 已完整移除（依赖包、alova.config.ts、vite.config.ts AutoImport 中的 usePagination/useRequest、eslint/tsconfig 中的 alova 类型引用、alova-gen 脚本）
- [ ] 确认 @wot-ui/ui 版本为 V2（wot-starter v2.0.0 已预装，无需手动替换）
- [ ] @wot-ui/unocss-preset 已升级到 1.0.0 稳定版（非自带 beta 版），wot- 前缀原子类在 H5 和小程序端均正常渲染
- [ ] Sass 版本为 ^1.99.0（与 wot-starter v2.0.0 锁定版本一致），vite.config.ts 已配置 modern-compiler API
- [ ] 组件自动导入配置确认指向 @wot-ui/ui（wot-starter v2.0.0 已预配置）
- [ ] tsconfig.json 中 Volar 类型引用正确，并添加 `uni-echarts/global`
- [ ] @uni-helper/uni-network、dayjs、lodash-es 已安装。注意：@uni-helper/uni-network npm 公共仓库最新版本为 0.21.x，如安装失败需确认版本可用性；dayjs 仅用于需要链式操作/插件扩展的场景，简单格式化优先使用 @vueuse/core；lodash-es 仅用于 @vueuse 未覆盖的深拷贝/复杂对象操作等场景
- [ ] echarts ^6.x、uni-echarts 已安装

## wot-starter 核心依赖
- [ ] vue-i18n 已预装，国际化功能可用
- [ ] @vueuse/core 已预装，AutoImport 自动导入的 composables 可正常使用
- [ ] @uni-helper/unocss-preset-uni 与 @wot-ui/unocss-preset 在 uno.config.ts 中均正确配置

## wot-starter 核心特性
- [ ] UnoCSS（@wot-ui/unocss-preset 1.0.0 稳定版）正确配置，原子类可正常使用
- [ ] 基于文件的路由（@uni-helper/vite-plugin-uni-pages）正常工作
- [ ] 组件自动化加载（unplugin-vue-components）正常工作
- [ ] auto-import（unplugin-auto-import）正常工作，Composition API 无需手动 import
- [ ] @wot-ui/router 路由库正常工作

## TypeScript 配置
- [ ] TypeScript 版本 ~5.5.4（与 wot-starter 锁定版本一致）
- [ ] tsconfig.json 启用 strict: true
- [ ] `@/` 路径别名在 vite.config.ts 和 tsconfig.json 中均正确配置
- [ ] `src/env.d.ts` 包含 `.vue` 文件和 uni-network 模块声明

## 类型定义
- [ ] AjaxResponse 类型包含 success、result、error、targetUrl、unAuthorizedRequest、__abp 字段
- [ ] MenuDto 类型支持 MainMenu、AdminMenu、TenantMenu、UserMenu 分组
- [ ] 所有导出的 interface 和 type 无 `any` 类型

## 异常体系
- [ ] UserFriendlyException、UnauthorizedException、ForbiddenException、NetworkException、TimeoutException、BatchOperationException 均定义
- [ ] 每种异常有对应的 name 属性和中文默认消息

## HTTP 模块
- [ ] 基于 @uni-helper/uni-network 创建实例，配置 baseUrl 和 timeout
- [ ] 请求拦截器自动从 storage 读取 Token 添加到 Authorization Header
- [ ] 请求去重功能（相同请求自动取消前一次）
- [ ] 响应拦截器自动识别 `__abp` 标记并解包 AjaxResponse.result
- [ ] 响应拦截器统一处理错误 UI 提示（UserFriendlyException 弹 Toast、BatchOperationException 弹 Modal、401 跳登录、403 弹权限提示、网络/超时弹对应提示）
- [ ] 请求选项支持 `silent: true`（需在 RequestOptions 接口中新增 `silent?: boolean` 字段），开启后拦截器不自动弹错误提示，覆盖所有异常类型（UserFriendlyException、NetworkException、TimeoutException 等）
- [ ] 401/403 响应正确抛出 UnauthorizedException/ForbiddenException
- [ ] 非 success 响应正确抛出 UserFriendlyException
- [ ] 批量操作响应正确抛出 BatchOperationException
- [ ] 导出 get、post、put、del 泛型方法
- [ ] 导出 setApiBaseUrl、getApiBaseUrl、cancelAllRequests

## 错误处理器
- [ ] GlobalErrorHandler 单例模式正确实现
- [ ] 捕获 Vue 错误（app.config.errorHandler）
- [ ] H5 端通过 `// #ifdef H5` 条件编译包裹 `window.addEventListener` 捕获 unhandledrejection 和 error
- [ ] 小程序端通过 `// #ifndef H5` 条件编译包裹 `uni.onError` 捕获全局错误
- [ ] 不存在未经条件编译的 `window.addEventListener` 裸调用（修复旧版代码 bug）
- [ ] 错误日志内存存储（最多 50 条）

## Pinia Store
- [ ] userStore 包含 token、userInfo、isLoggedIn、setToken、setUserInfo、loadTokenFromStorage、clearUserData、logout
- [ ] userStore 的 Token 和 userInfo 同步到 uni.storage
- [ ] appStore 包含 abpConfig、currentUser、menu、globalLoading、对应 setter 和 clearAppData
- [ ] Store 使用 Setup Store 语法（defineStore + Composition API）
- [ ] Store 文件位于 `src/store/` 目录（遵循 wot-starter 单数命名约定）

## Composables
- [ ] `src/composables/useAuth.ts`：登录（调用 auth API）、登出（调用 userStore.logout）、Token 管理、returnUrl 处理
- [ ] `src/composables/useMenu.ts`：响应式读取菜单数据（从 appStore）、底部/侧边菜单过滤、菜单项导航、激活状态判断
- [ ] 所有 composables 使用 Composition API 风格，有完整 TypeScript 类型
- [ ] Composables 由 AutoImport 自动导入（wot-starter 的 unplugin-auto-import 配置中 `dirs: ['src/composables']`），无需手动 import

## 菜单工具
- [ ] normalizeMenuUrl 正确将 URL 转为平铺页面路由路径，不使用 `/index` 后缀（例如 `/login` → `/pages/login`）
- [ ] filterBottomMenus 正确过滤 mobileShowModel === 2 的菜单项
- [ ] filterSideMenus 正确过滤 mobileShowModel === 4 或未设置的菜单项
- [ ] navigateToMenuItem 使用 @wot-ui/router 正确跳转并处理失败
- [ ] isMenuItemActive 正确判断当前页面是否匹配

## API 接口层
- [ ] auth.ts 定义 AuthenticateModel、AuthenticateResultModel 类型和 authenticate 函数
- [ ] session.ts 定义 GetCurrentLoginInformationsOutput 类型和 getCurrentLoginInformations 函数
- [ ] config.ts 定义 AbpUserConfigurationDto 及所有子类型，getUserConfiguration 函数。AbpUserNavConfigDto 字段名为 `menus: MenuDto`（复数，MenuDto 为键值对接口，等价于 `Record<string, MenuGroupDto | undefined>`，而非数组），已修正现有代码 `menu: any[]`（单数且类型为数组）的 bug，与 ASP.NET Boilerplate 源码中 `Dictionary<string, UserMenu>` 的后端类型一致，调用方无需 `as MenuDto` 强制断言
- [ ] captcha.ts 定义验证码相关 API

## 应用初始化服务
- [ ] AppInitializationService 单例模式正确实现
- [ ] initializeOnLaunch：从 storage 恢复 Token → 获取 ABP 配置 → 恢复用户信息
- [ ] initializeAfterLogin：获取 ABP 配置 → 获取用户信息
- [ ] reinitialize：设置新 API 地址 → 重新初始化 → 跳转首页

## 路由配置
- [ ] @wot-ui/router 路由拦截（beforeEach / afterEach）正确配置，登录状态检查生效。注：@wot-ui/router 基于 uni-app 路由能力提供近 Vue Router 风格的编程式导航与路由拦截，非声明式路由守卫
- [ ] router/index.ts 仅包含导航守卫和路由实例导出，不包含路由表定义（路由映射由文件路由自动生成）
- [ ] 页面级 meta（layout、requiresAuth、title）通过页面组件的 `<route>` 块或文件层级声明（wot-starter 的 @uni-helper/vite-plugin-uni-layouts 布局系统通过页面文件所在目录名或 `<route>` 块中的 layout 字段自动匹配布局），不通过 pages.config.ts 定义页面路由

## 布局系统
- [ ] default.vue 布局使用 Wot UI 组件（wd-navbar、wd-tabbar、wd-popup）构建
- [ ] 布局通过路由 meta 支持显示/隐藏导航栏和底部 TabBar
- [ ] 布局通过路由 meta 支持自定义标题
- [ ] 安全区域适配（状态栏高度）

## 通用组件
- [ ] BottomNav 使用 wd-tabbar 组件，动态展示底部导航菜单
- [ ] BottomNav 固定包含"首页"和"我的"两个 Tab 项
- [ ] BottomNav 点击 Tab 正确切换页面并高亮当前项
- [ ] SideMenu 使用 wd-popup 组件，支持菜单分组展示
- [ ] SideMenu 支持子菜单展开/折叠
- [ ] SideMenu 点击菜单项正确导航并关闭侧边栏
- [ ] SideMenu 蒙层点击关闭

## 核心页面
- [ ] App.vue onLaunch 正确判断 API 地址并初始化
- [ ] main.ts 正确创建应用、注册 Pinia、注册错误处理
- [ ] main.ts 中 H5 端使用 `window.addEventListener`（条件编译），小程序端使用 `uni.onError`
- [ ] 登录页使用 Wot UI 组件（wd-input、wd-button 等）构建
- [ ] 登录页支持租户/用户名/密码/验证码输入和登录流程
- [ ] 登录页支持 returnUrl 参数
- [ ] 首页/主页使用 default 布局
- [ ] 个人中心页展示用户信息并提供登出
- [ ] 初始化页可配置 API 地址

## Wot UI 主题与样式
- [ ] Sass 版本为 ^1.99.0（与 wot-starter v2.0.0 锁定版本一致），vite.config.ts 已配置 modern-compiler API，无 legacy-js-api 弃用警告
- [ ] theme.json 定义品牌色和通用 CSS 变量
- [ ] UnoCSS 原子类可正常使用（text-、bg-、flex、p-、m- 等）
- [ ] Wot UI 组件样式正确渲染
- [ ] uni.scss 全局 SCSS 变量已配置

## 图表库
- [ ] uni-echarts Vite 插件（UniEcharts，从 `uni-echarts/vite` 导入）已在 vite.config.ts 中配置
- [ ] uni-echarts 组件 Resolver（UniEchartsResolver，从 `uni-echarts/resolver` 导入）已在 UniHelperComponents 的 resolvers 中配置
- [ ] vite.config.ts 中 optimizeDeps 排除 uni-echarts
- [ ] uni-echarts 组件可在 H5 和小程序端正常渲染图表
- [ ] echarts 已安装（uni-echarts 封装了 echarts 实例创建，无需额外的按需引入配置）

## 代码规范
- [ ] 所有组件使用 `<script setup lang="ts">` 语法
- [ ] 所有 Store 使用 Setup Store 语法
- [ ] 所有导出函数/类/接口/Props 有中文注释
- [ ] 样式优先使用 UnoCSS 原子类
- [ ] 无 `any` 类型滥用

## 清理
- [ ] wot-starter 自带示例页面已删除（subPages/、subEcharts/、subAsyncEcharts/ 等）
- [ ] vite.config.ts 中 UniHelperPages 的 subPackages 配置已移除已删除的分包路径
- [ ] wot-starter 自带示例 API 和 composables 已删除
- [ ] 项目结构整洁，无残留无用代码

## 环境变量
- [ ] .env.development、.env.production、.env.staging 三个环境文件存在
- [ ] VITE_API_BASE_URL 在各环境文件中正确配置
- [ ] src/env.d.ts 中包含 VITE_API_BASE_URL 的类型声明
- [ ] vite.config.ts 中开发代理使用 import.meta.env.VITE_API_BASE_URL