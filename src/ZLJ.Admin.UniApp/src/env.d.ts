/// <reference types="vite/client" />

/** @uni-helper/uni-network 模块声明 */
declare module '@uni-helper/uni-network'

/** 环境变量类型声明 */
interface ImportMetaEnv {
  /** API 基础地址 */
  readonly VITE_API_BASE_URL: string
  /** 环境名称 */
  readonly VITE_ENV_NAME: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}

/** Vue 单文件组件模块声明 */
declare module '*.vue' {
  import type { DefineComponent } from 'vue'
  const component: DefineComponent<object, object, unknown>
  export default component
}
