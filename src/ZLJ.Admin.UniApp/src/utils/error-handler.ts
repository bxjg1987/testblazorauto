import {
  UnauthorizedException,
  ForbiddenException,
  UserFriendlyException,
  BatchOperationException,
  NetworkException,
  TimeoutException,
} from '@/utils/exceptions'

/** 错误上下文信息 */
interface ErrorContext {
  componentName?: string
  lifecycle?: string
  errorMessage?: string
  stackTrace?: string
  timestamp?: number
}

/** 全局错误处理器，单例模式，统一捕获和处理应用中的各类错误 */
class GlobalErrorHandler {
  private static instance: GlobalErrorHandler
  private errorLog: ErrorContext[] = []
  private readonly MAX_LOG_SIZE = 50

  private constructor() {}

  /** 获取全局错误处理器单例实例 */
  static getInstance(): GlobalErrorHandler {
    if (!GlobalErrorHandler.instance) {
      GlobalErrorHandler.instance = new GlobalErrorHandler()
    }
    return GlobalErrorHandler.instance
  }

  /** 处理Vue组件错误 */
  handleVueError(error: Error, instance: unknown, info: string): void {
    const componentName = this.getComponentName(instance)
    this.addLog({
      componentName,
      lifecycle: info,
      errorMessage: error.message,
      stackTrace: error.stack,
      timestamp: Date.now(),
    })
    this.showUserMessage(error)
  }

  /** 处理H5平台未捕获的Promise拒绝 */
  handleUnhandledRejection(event: PromiseRejectionEvent): void {
    const reason = event.reason
    const error = reason instanceof Error ? reason : new Error(String(reason))
    this.addLog({
      errorMessage: error.message,
      stackTrace: error.stack,
      timestamp: Date.now(),
    })
    this.showUserMessage(error)
  }

  /** 处理H5平台全局错误 */
  handleGlobalError(event: ErrorEvent): void {
    const error = event.error instanceof Error ? event.error : new Error(event.message || String(event))
    this.addLog({
      errorMessage: error.message,
      stackTrace: error.stack,
      timestamp: Date.now(),
    })
    this.showUserMessage(error)
  }

  /** 处理小程序平台全局错误 */
  handleMiniProgramError(errorMessage: string): void {
    this.addLog({
      errorMessage,
      timestamp: Date.now(),
    })
    this.showUserMessage(new Error(errorMessage))
  }

  /** 上报错误 */
  reportError(error: Error, context?: unknown): void {
    this.addLog({
      errorMessage: error.message,
      stackTrace: error.stack,
      timestamp: Date.now(),
    })
    this.showUserMessage(error)
  }

  /** 获取错误日志列表 */
  getLogs(): ErrorContext[] {
    return [...this.errorLog]
  }

  /** 清空错误日志 */
  clearLogs(): void {
    this.errorLog = []
  }

  private addLog(entry: ErrorContext): void {
    this.errorLog.push(entry)
    if (this.errorLog.length > this.MAX_LOG_SIZE) {
      this.errorLog.shift()
    }
  }

  private getComponentName(instance: unknown): string {
    if (instance && typeof instance === 'object') {
      const obj = instance as Record<string, unknown>
      if (obj.$options && typeof obj.$options === 'object') {
        const options = obj.$options as Record<string, unknown>
        if (options.name && typeof options.name === 'string') {
          return options.name
        }
      }
      if (obj._name && typeof obj._name === 'string') {
        return obj._name
      }
    }
    return 'Unknown'
  }

  private getUserMessage(error: Error): string | null {
    if (error instanceof UnauthorizedException) {
      return '未登录，请重新登录'
    }
    if (error instanceof ForbiddenException) {
      return '无权限执行此操作'
    }
    if (error instanceof UserFriendlyException) {
      return error.message
    }
    if (error instanceof BatchOperationException) {
      return '部分操作失败，请检查详细信息'
    }
    if (error instanceof NetworkException) {
      return '网络连接失败，请检查网络设置'
    }
    if (error instanceof TimeoutException) {
      return '请求超时，请重试'
    }
    if (error.message && error.message.includes('Network')) {
      return '网络连接失败，请检查网络设置'
    }
    if (error.message && error.message.toLowerCase().includes('timeout')) {
      return '请求超时，请重试'
    }
    return null
  }

  private showUserMessage(error: Error): void {
    const message = this.getUserMessage(error)
    if (message) {
      uni.showToast({
        title: message,
        icon: 'none',
        duration: 3000,
      })
    }
  }
}

/** 全局错误处理器单例实例 */
export const globalErrorHandler = GlobalErrorHandler.getInstance()
