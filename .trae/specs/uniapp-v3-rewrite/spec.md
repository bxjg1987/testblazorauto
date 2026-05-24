# UniApp 完整重构规范

## Why
现有 ZLJ.Admin.UniApp 项目基于 uniapp 3.x 早期版本，存在依赖版本不稳定、大量 `any` 类型滥用、组件职责不清、目录结构不够规范等问题。需要对整个 uniapp 项目进行完整重构，采用稳定版本的依赖和现代化的工程实践，打造可维护、可扩展的企业级跨平台应用基础框架。

## What Changes
- 使用 uniapp 最新版本（Vue3 + Vite + TypeScript），不使用 uniappx。注意：uniapp Vue3 版本号格式为 `3.0.0-数字编号`，属正常发布格式，社区广泛使用。项目创建后继承 wot-starter 锁定的确切版本号（如 `3.0.0-4080520251106001`），不写模糊的 `latest` 或 `*`，后续升级需经验证
- 基于 wot-starter v2（`wot-ui/wot-starter#v2`）模板创建项目，该模板基于 vitesse-uni-app 深度整合 Wot UI
- 采用 Wot UI（@wot-ui/ui，V2 版本）作为 UI 组件库
- 采用 UnoCSS（原子化 CSS）作为样式方案，配合 wot-ui 的 CSS 变量主题系统
- 网络请求继续使用 @uni-helper/uni-network（已在项目中验证，axios 风格 API）
- 采用 wot-starter 的目录结构约定（layouts、router、store、composables 等）
- 使用 @wot-ui/router 提供 Vue Router 风格的编程式导航
- 对 ABP 后端特有的 AjaxResponse 格式、JWT 认证、菜单系统做标准化封装
- 建立 TypeScript 严格类型体系，消除 `any` 类型
- 统一错误处理、请求拦截、权限控制等横切关注点
- **BREAKING**: 完全重建项目，不兼容旧版代码结构

## Impact
- Affected specs: 无（全新项目）
- Affected code: `src/ZLJ.Admin.UniApp/` 整个目录将被替换

## 技术选型方案

### 选型原则
1. 优先选择官方/社区维护活跃的库
2. 必须支持 Vue3 + TypeScript
3. 必须兼容 uniapp 多端（H5、微信小程序、Android、iOS）。注意：wot-starter v2.0.0 的 package.json 中已包含 `@dcloudio/uni-app-harmony` 和 `@dcloudio/uni-mp-harmony`，即已支持鸿蒙平台，如项目不需要鸿蒙支持可在清理示例代码时移除相关依赖
4. 优先选择已在现有项目中验证过的库
5. 参考 wot-starter 的生态整合经验

### 确定的技术栈

| 类别 | 选择 | 版本要求 | 理由 |
|------|------|----------|------|
| 项目模板 | wot-starter v2 | v2.0.0 | 基于 vitesse-uni-app + Wot UI 深度整合，拥抱现代前端工具链 |
| 框架 | uniapp (Vue3版) | 由 wot-starter 锁定（如 3.0.0-4080520251106001） | 不写 latest，版本升级需经验证 |
| 构建 | Vite | ^5.x | uniapp 官方 Vite 插件支持 |
| 语言 | TypeScript | ~5.5.4（与 wot-starter 锁定版本一致） | 严格模式，提升代码质量 |
| UI 组件 | Wot UI（@wot-ui/ui） | V2 latest | 80+ 高质量组件，AI 友好设计，支持国际化/暗黑模式/CSS 变量主题定制 |
| CSS 方案 | UnoCSS | latest | 原子化 CSS，wot-starter 默认方案，高性能 |
| CSS 预处理器 | Sass | ^1.99.0 | Wot UI V2 依赖，SCSS 语法用于 uni.scss 全局变量。与 wot-starter v2.0.0 保持一致 |
| 网络请求 | @uni-helper/uni-network | latest | axios 风格 API，已在项目中验证，TypeScript 支持良好。注意：npm 公共仓库最新版本为 0.21.x，现有项目使用的 0.23.x 可能来自其他注册源，新项目安装时需确认版本可用性 |
| 状态管理 | Pinia | ^2.x | Vue3 官方推荐 |
| 路由增强 | @wot-ui/router | latest | 基于 uni-app 路由能力，提供近 Vue Router 风格的编程式导航与路由拦截 |
| 日期处理 | dayjs | latest | 轻量级日期库，用于 ABP 后端时间戳格式化、相对时间显示、日期范围计算等场景。简单格式化可使用 @vueuse/core 的 useDateFormat 或原生 Intl.DateTimeFormat，dayjs 仅在需要链式操作、插件扩展（如 relativeTime、utc）时引入 |
| 图表库 | uni-echarts + echarts | uni-echarts ^2.x, echarts ^6.x | 适用于 uni-app 的 ECharts 组件，多端兼容，TypeScript 友好 |
| 国际化 | vue-i18n | ^9.x | Wot UI 国际化能力的基础依赖，wot-starter 已预装 |
| 组合式工具 | @vueuse/core | ^11.x | Vue Composition API 工具集，wot-starter 已预装且 AutoImport 自动导入 |
| 平台 UnoCSS 规则 | @uni-helper/unocss-preset-uni | latest | uni-app 平台特定的 UnoCSS 规则，与 @wot-ui/unocss-preset 配合使用 |
| Wot UI UnoCSS 预设 | @wot-ui/unocss-preset | 1.0.0 稳定版 | Wot UI 官方 UnoCSS 预设，将 Wot UI 设计 token 映射为原子类。wot-starter 自带 beta 版，需升级到 1.0.0 稳定版 |
| 根组件插件 | @uni-ku/root | latest | wot-starter 用于模拟 App.vue 能力的根组件插件（devDependency） |
| 分包优化 | @uni-ku/bundle-optimizer | latest | uni-app 分包优化与异步跨包引用插件，支持分包优化、模块异步跨包调用、组件异步跨包引用（devDependency） |
| 工具函数 | lodash-es | latest | 按需引入，tree-shakeable。注意：项目已引入 @vueuse/core 提供大量实用工具函数，lodash-es 仅用于 @vueuse 未覆盖的深拷贝（cloneDeep）、复杂对象操作（merge、pick）等场景，非通用工具需求优先使用 @vueuse/core 或原生方法 |
| 代码质量 | ESLint + @uni-helper/eslint-config | latest | wot-starter 默认配置 |
| 文件路由 | @uni-helper/vite-plugin-uni-pages | latest | 基于文件系统自动生成路由配置，wot-starter 已预装（devDependency） |
| 布局系统 | @uni-helper/vite-plugin-uni-layouts | latest | 类 Nuxt 的布局系统，wot-starter 已预装（devDependency） |
| 组件自动导入 | @uni-helper/vite-plugin-uni-components | latest | 按需自动引入组件，配合 resolver.ts 实现 Wot UI 组件自动注册（devDependency） |
| API 自动导入 | unplugin-auto-import | latest | Composition API 无需手动 import，wot-starter 已预装（devDependency） |
| Manifest 生成 | @uni-helper/vite-plugin-uni-manifest | latest | 由 manifest.config.ts 自动生成 manifest.json，wot-starter 已预装（devDependency） |

### 不推荐的技术（已排除）

| 库 | 不推荐理由 |
|------|-------------|
| @dcloudio/uni-ui | 功能弱于 Wot UI，组件数量和定制性不足 |
| uView Plus | 社区维护不够稳定，与 Wot UI 功能重叠 |
| wot-design-uni（V1） | 已被 @wot-ui/ui（V2）取代，V1 不再推荐 |
| Alova | wot-starter 默认使用，但 @uni-helper/uni-network 更熟悉且已在项目中验证 |
| SCSS（作为主要方案） | UnoCSS 原子化 CSS 开发效率更高，但保留 uni.scss 用于全局变量 |

### wot-starter 核心特性（直接继承）

以下特性由 wot-starter 模板自带，无需额外配置：
- 基于文件的路由（@uni-helper/vite-plugin-uni-pages）
- 组件自动化加载（unplugin-vue-components）
- 布局系统（src/layouts/）
- API 自动导入（unplugin-auto-import，Composition API 无需手动 import）
- UnoCSS 原子化 CSS 引擎（uno.config.ts 同时使用 @wot-ui/unocss-preset 和 @uni-helper/unocss-preset-uni 双重预设，前者提供 Wot UI 主题原子类，后者处理 uni-app 平台的 UnoCSS 兼容规则。启动模板自带的 @wot-ui/unocss-preset 为 beta 版，需升级到 1.0.0 稳定版）
- 图标集支持（@unocss/preset-icons，100000+ 图标可用）
- 图表组件（uni-echarts，基于 Apache ECharts，多端兼容。wot-starter 在 vite.config.ts 中同时配置了 Vite 插件 `UniEcharts()`（从 `uni-echarts/vite` 导入）和组件 Resolver `UniEchartsResolver()`（从 `uni-echarts/resolver` 导入），前者负责 ECharts 实例创建，后者负责 uni-echarts 组件的自动按需注册）
- CSS 变量主题定制（theme.json）
- ESM 配置文件（pages.config.ts、manifest.config.ts、uno.config.ts、vite.config.ts）

## ADDED Requirements

### Requirement: 项目初始化与技术栈搭建
系统 SHALL 基于 wot-starter v2 模板创建项目，安装并配置所有确定的技术栈依赖。

#### Scenario: 环境要求
- **WHEN** 开发者准备创建项目
- **THEN** 必须确保 Node.js >= 20.19.0 || >= 22.12.0 || >= 24.0.0（推荐 LTS 偶数版本），包管理器使用 pnpm

#### Scenario: 通过 wot-starter 创建项目
- **WHEN** 执行 `pnpx degit wot-ui/wot-starter#v2`（推荐，直接从 GitHub 拉取，版本确定）或 `pnpm create uni@latest <name> -t wot-starter-v2`
- **THEN** 生成包含 Vite + Vue3 + TypeScript + Wot UI + UnoCSS + Pinia 的完整项目骨架
- **NOTE** `pnpm create uni` 通过 create-uni 工具创建，`pnpx degit` 直接从 GitHub 拉取 v2 分支代码。两种方式得到的项目结构应一致，degit 方式对版本控制更精确（直接指向 GitHub 分支/标签）

#### Scenario: 清理示例代码
- **WHEN** 项目创建完成后
- **THEN** 删除 wot-starter 自带的演示页面和示例代码（subPages/、subEcharts/、subAsyncEcharts/ 等分包目录，以及对应的示例 API 和 composables），仅保留核心框架代码（layouts、router、store 基础结构）。同时从 vite.config.ts 的 subPackages 配置中移除已删除的分包路径

#### Scenario: 替换网络库
- **WHEN** 项目创建完成后
- **THEN** 移除 wot-starter 自带的 Alova，安装并配置 @uni-helper/uni-network。具体步骤：1) 卸载 `@alova/adapter-uniapp`、`@alova/mock`、`@alova/shared`、`alova`、`@alova/wormhole` 等相关依赖；2) 删除根目录下的 `alova.config.ts` 配置文件；3) 安装 `@uni-helper/uni-network`；4) 重写 `src/utils/http.ts`，将所有基于 Alova 的 API 调用方式替换为 @uni-helper/uni-network 的 axios 风格 API；5) 重写 `src/api/` 下所有接口文件，使用新的 http 工具函数

#### Scenario: 安装额外工具依赖
- **WHEN** 项目创建完成后
- **THEN** 安装 dayjs、lodash-es 等工具库，且版本为稳定版

### Requirement: 目录结构设计
系统 SHALL 遵循 wot-starter 的目录结构约定，按功能分层组织。

```
项目根目录/
├── .env.development     # 开发环境变量
├── .env.production      # 生产环境变量
├── .env.staging         # 预发布环境变量
├── eslint.config.mjs    # ESLint 配置
├── manifest.config.ts   # 应用配置（生成 manifest.json）
├── pages.config.ts      # 页面路由配置（生成 pages.json）
├── tsconfig.json        # TypeScript 配置
├── tsconfig.base.json   # TypeScript 基础配置
├── uno.config.ts        # UnoCSS 配置
├── vite.config.ts       # Vite 构建配置
├── package.json         # 项目依赖
└── src/
    ├── api/              # API 接口层（按业务模块拆分）
    │   ├── auth.ts
    │   ├── session.ts
    │   ├── config.ts
    │   └── ...
    ├── business/          # 业务组件（wot-starter 约定，与 components/ 平级，vite.config.ts 中 dirs 配置为 ['src/components', 'src/business']）
    ├── components/        # 可复用通用组件（自动按需导入）
    │   ├── common/        # 通用组件（PageLayout, Modal 等）
    ├── composables/      # Vue Composables（可组合函数）
    ├── layouts/          # 布局组件（wot-starter 布局系统）
    │   └── default.vue   # 默认布局
    ├── pages/            # 页面组件（基于文件的路由）
    │   ├── index.vue
    │   ├── login.vue
    │   └── ...
    ├── router/           # 路由相关（@wot-ui/router 导航守卫等配置，非传统路由表）
    │   └── index.ts
    ├── services/         # 业务服务层（编排多个 API 调用）
    ├── static/           # 静态资源
    ├── store/            # Pinia 状态管理（注意：单数 store）
    │   ├── app.ts
    │   └── user.ts
    ├── types/            # TypeScript 类型定义
    ├── utils/            # 工具函数
    ├── App.vue           # 应用入口
    ├── main.ts           # 启动文件
    ├── resolver.ts       # Wot UI 组件解析器（供 vite.config.ts 中 @uni-helper/vite-plugin-uni-components 引用，实现 Wot UI 组件的自动按需注册，无需手动 import 组件）
    ├── pages.json        # 页面配置（由 pages.config.ts 生成）
    ├── manifest.json     # 应用配置
    ├── theme.json        # Wot UI 主题变量
    └── uni.scss          # 全局 SCSS 变量
```

#### Scenario: 新开发者查找 API 定义
- **WHEN** 开发者需要查看认证相关 API
- **THEN** 直接找到 `src/api/auth.ts`，所有接口函数集中在一个文件中

#### Scenario: 自动生成的文件
- **WHEN** 项目构建或开发服务器启动后
- **THEN** 以下文件由 Vite 插件自动生成，不需要手动创建：`src/uni-pages.d.ts`（文件路由类型声明）、`src/components.d.ts`（组件自动导入类型声明）、`src/auto-imports.d.ts`（API 自动导入类型声明）

#### Scenario: composables 与 services 的职责边界
- **WHEN** 开发者需要封装可复用逻辑
- **THEN** `composables/`：存放 Vue Composition API 可组合函数（如 useAuth、useMenu），具有响应式状态，由 AutoImport 自动导入无需手动 import。复用粒度是"跨页面/跨组件的 Vue 逻辑"。
- **AND** `services/`：存放业务编排层（如 app-initialization.ts），负责编排多个 API 调用和 Store 操作，需手动 import。复用粒度是"跨模块的业务流程"。如果某一逻辑需要在多个页面中响应式使用且需要自动导入，放在 composables 中；如果只是一个入口级别的初始化/编排流程，不需要自动导入，放在 services 中

### Requirement: ABP 后端对接封装
系统 SHALL 封装与 ABP 后端的标准交互模式，包括 AjaxResponse 解析、JWT 认证、ABP 配置获取、动态菜单处理。

ABP 配置字段名说明：ABP 后端 `AbpUserConfigurationDto` 中的导航菜单 JSON 字段名为 `menus`（复数）。根据 ASP.NET Boilerplate 源码（`AbpUserNavConfigDto.cs`），后端类型为 `Dictionary<string, UserMenu>`，即键值对结构（键为菜单分组名称如 MainMenu、AdminMenu 等，值为 UserMenu 对象）。现有代码中 `AbpUserNavConfigDto` 错误地定义了 `menu: any[]`（单数且类型为数组），导致调用方被迫使用 `config.nav?.menus as MenuDto` 的强制断言来弥补类型不一致。重构时必须修正为 `menus: MenuDto`（MenuDto 为键值对接口，等价于 `Record<string, MenuGroupDto | undefined>`，而非数组），使类型定义与后端数据一致，消除不必要的类型断言。

#### Scenario: HTTP 响应拦截器自动解包 AjaxResponse
- **WHEN** 后端返回 `{ success: true, result: {...}, __abp: true }` 格式的数据
- **THEN** 拦截器自动提取 `result` 字段，上层 API 函数直接获得业务数据

#### Scenario: 请求自动携带 JWT Token
- **WHEN** 用户已登录且 Token 存储在本地
- **THEN** 所有请求自动在 Header 中添加 `Authorization: Bearer {token}`

#### Scenario: 401 响应自动跳转登录
- **WHEN** 接口返回 401 状态码或 `unAuthorizedRequest: true`
- **THEN** 清除本地 Token，跳转登录页，保留当前页面路径作为 returnUrl

### Requirement: 异常处理体系
系统 SHALL 建立分层异常体系，区分网络异常、业务异常、权限异常，并提供统一的用户提示。

- UserFriendlyException：后端业务错误，友好提示
- UnauthorizedException：未登录，跳转登录页
- ForbiddenException：无权限，提示联系管理员
- NetworkException：网络故障，提示检查网络
- TimeoutException：请求超时，提示重试
- BatchOperationException：批量操作部分失败，弹窗展示详情

错误提示的展示职责由 HTTP 响应拦截器统一承担，而非 API 辅助方法或调用方。拦截器作为默认行为处理器，识别到异常后自动弹出对应的 UI 提示（Toast/Modal）。调用方通过请求选项 `silent: true` 可关闭自动提示，自行处理错误。注意：`silent` 为新增功能，需在 RequestOptions 接口中新增 `silent?: boolean` 字段，并在拦截器的异常处理逻辑中增加对应判断——当 `silent` 为 true 时跳过自动 UI 提示，仅抛出异常。

#### Scenario: 后端返回业务错误（默认行为）
- **WHEN** 后端返回 `{ success: false, error: { code: 500, message: "库存不足" } }`
- **THEN** 拦截器抛出 UserFriendlyException 并自动显示 Toast "库存不足"，调用方无需 try/catch

#### Scenario: 后端返回业务错误（静默模式）
- **WHEN** 调用方发起请求时传入 `{ silent: true }` 选项
- **THEN** 拦截器抛出 UserFriendlyException 但不自动显示 Toast，由调用方自行 catch 处理。`silent: true` 对所有异常类型均生效（包括 NetworkException、TimeoutException 等），确保调用方在需要定制提示文案或静默处理特定场景时有完全的 opt-out 能力

### Requirement: 状态管理规范
系统 SHALL 使用 Pinia 的 Setup Store 语法（Composition API 风格），按职责拆分 Store，存放于 `src/store/` 目录。

- appStore：ABP 配置、当前用户、菜单数据、全局加载状态
- userStore：Token、用户信息、登录状态、登出操作

#### Scenario: 用户登录后初始化状态
- **WHEN** 用户登录成功
- **THEN** userStore 保存 Token 和用户信息到 storage，appStore 加载 ABP 配置和菜单

### Requirement: 布局系统
系统 SHALL 利用 wot-starter 的 layouts 系统提供统一的页面布局。

- default.vue 布局：包含自定义顶部导航栏、底部 TabBar、可唤出的侧边菜单
- 布局支持通过路由 meta 或页面级配置切换
- Wot UI 组件（wd-navbar、wd-tabbar、wd-popup 等）用于布局构建

#### Scenario: 标准页面使用默认布局
- **WHEN** 业务页面使用默认布局
- **THEN** 自动渲染顶部导航栏（含标题和菜单按钮）、底部 TabBar 和可唤出的侧边菜单

### Requirement: TypeScript 严格模式
系统 SHALL 在 tsconfig.json 中启用 strict 模式，所有 API 接口、Store 状态、组件 Props 必须有明确的类型定义，禁止使用 `any`（除必要的第三方库兼容场景）。

#### Scenario: API 函数返回类型
- **WHEN** 定义 API 函数 `authenticate()`
- **THEN** 必须有明确的返回类型 `Promise<AuthenticateResultModel>`，参数类型为 `AuthenticateModel`

### Requirement: 代码风格统一
系统 SHALL 遵循以下代码规范：
- 组件使用 `<script setup lang="ts">` 语法
- Store 使用 Setup Store 语法（defineStore + Composition API）
- 除方法内部外，所有导出函数、类、接口、Store、组件 Props 必须有中文注释
- 样式优先使用 UnoCSS 原子类，复杂样式使用 `<style scoped>` 配合 Wot UI CSS 变量
- 路径别名使用 `@/` 指向 `src/`

### Requirement: 跨端兼容的错误捕获
系统 SHALL 在全局错误捕获中兼容 H5 和小程序端。H5 端使用 `window.addEventListener('unhandledrejection')` 和 `window.addEventListener('error')`，小程序端使用 `uni.onError` 等 uniapp 提供的 API。代码中必须通过条件编译（`// #ifdef H5` / `// #endif`）或运行时环境检测区分平台。注意：旧版代码直接在 `main.ts` 中使用了 `window.addEventListener` 而未加条件编译，在小程序端会因 `window` 不存在而报错，重构时必须修复。

#### Scenario: H5 端错误捕获
- **WHEN** 应用运行在 H5 平台
- **THEN** 通过 `window.addEventListener` 捕获未处理的 Promise 拒绝和全局错误。代码结构示例：在 `main.ts` 或 error-handler.ts 中使用 `// #ifdef H5` 包裹 `window.addEventListener` 调用

#### Scenario: 小程序端错误捕获
- **WHEN** 应用运行在小程序平台
- **THEN** 通过 `uni.onError` 等 uniapp API 捕获全局错误。代码结构示例：在 `main.ts` 或 error-handler.ts 中使用 `// #ifndef H5` 包裹 `uni.onError` 调用

### Requirement: 环境变量管理
系统 SHALL 使用 Vite 环境变量文件管理不同环境的配置。wot-starter 自带 `.env.development`、`.env.production`、`.env.staging` 三个环境文件，API 基础地址等环境相关配置应通过 `VITE_` 前缀的环境变量管理。

#### Scenario: 开发环境 API 代理
- **WHEN** 开发者执行 `pnpm dev`
- **THEN** 读取 `.env.development` 中的 `VITE_API_BASE_URL`，Vite 开发服务器代理 `/api` 请求到后端

## MODIFIED Requirements
无（全新项目）

## REMOVED Requirements
无（全新项目）