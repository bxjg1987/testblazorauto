import { createSSRApp } from 'vue'
import App from './App.vue'
import router from './router'
import { globalErrorHandler } from '@/utils/error-handler'
import 'uno.css'

/** 创建 Pinia 实例 */
const pinia = createPinia()

/** 创建应用 */
export function createApp() {
  const app = createSSRApp(App)
  app.use(router)
  app.use(pinia)

  app.config.errorHandler = (error, instance, info) => {
    globalErrorHandler.handleVueError(error as Error, instance, info)
  }

  return { app }
}

// #ifdef H5
/** H5 端未处理的 Promise 拒绝捕获 */
window.addEventListener('unhandledrejection', (event) => {
  globalErrorHandler.handleUnhandledRejection(event)
})

/** H5 端全局错误捕获 */
window.addEventListener('error', (event) => {
  globalErrorHandler.handleGlobalError(event)
})
// #endif

// #ifndef H5
/** 小程序端全局错误捕获 */
uni.onError((errorMessage) => {
  globalErrorHandler.handleMiniProgramError(errorMessage)
})
// #endif
