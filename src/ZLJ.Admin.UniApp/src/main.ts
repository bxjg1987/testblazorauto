import { createSSRApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import { globalErrorHandler } from './utils/error-handler'

export function createApp() {
  const app = createSSRApp(App)
  const pinia = createPinia()

  app.use(pinia)

  app.config.errorHandler = (err, instance, info) => {
    globalErrorHandler.handleVueError(err as Error, instance, info)
  }

  window.addEventListener('unhandledrejection', (event) => {
    globalErrorHandler.handleUnhandledRejection(event)
  })

  window.addEventListener('error', (event) => {
    globalErrorHandler.handleGlobalError(event)
  })

  return {
    app,
    pinia,
  }
}
