import {
  UserFriendlyException,
  BatchOperationException,
  UnauthorizedException,
  ForbiddenException,
  NetworkException,
  TimeoutException,
} from '@/utils/exceptions'

interface ErrorContext {
  componentName?: string
  lifecycle?: string
  errorMessage?: string
  stackTrace?: string
  timestamp?: string
}

/**
 * 全局错误处理器，用于捕获和处理应用中的各类错误
 */
export class GlobalErrorHandler {
  private static instance: GlobalErrorHandler
  private errorLog: ErrorContext[] = []
  private maxLogSize = 50

  private constructor() {}

  static getInstance(): GlobalErrorHandler {
    if (!GlobalErrorHandler.instance) {
      GlobalErrorHandler.instance = new GlobalErrorHandler()
    }
    return GlobalErrorHandler.instance
  }

  private formatError(error: Error, context?: any): ErrorContext {
    return {
      componentName: context?.type?.name || context?.componentName || 'Unknown',
      lifecycle: context?.lifecycle || 'Unknown',
      errorMessage: error.message || 'Unknown error',
      stackTrace: error.stack || '',
      timestamp: new Date().toISOString(),
    }
  }

  private addToLog(context: ErrorContext): void {
    this.errorLog.push(context)
    if (this.errorLog.length > this.maxLogSize) {
      this.errorLog.shift()
    }
  }

  private getErrorLog(): ErrorContext[] {
    return [...this.errorLog]
  }

  private clearErrorLog(): void {
    this.errorLog = []
  }

  private showErrorToUser(message: string): void {
    uni.showToast({
      title: message,
      icon: 'none',
      duration: 3000,
    })
  }

  handleVueError(error: Error, instance: any, info: string): void {
    const context = this.formatError(error, {
      componentName: instance?.$options?.name || instance?.type?.name,
      lifecycle: info,
    })

    this.addToLog(context)

    const userMessage = this.getUserFriendlyMessage(error)
    if (userMessage) {
      this.showErrorToUser(userMessage)
    }
  }

  handleUnhandledRejection(event: PromiseRejectionEvent): void {
    const error = event.reason instanceof Error ? event.reason : new Error(String(event.reason))
    const context = this.formatError(error, {
      lifecycle: 'unhandledrejection',
    })

    this.addToLog(context)

    const userMessage = this.getUserFriendlyMessage(error)
    if (userMessage) {
      this.showErrorToUser(userMessage)
    }

    event.preventDefault()
  }

  handleGlobalError(event: ErrorEvent): void {
    const error = event.error || new Error(event.message)
    const context = this.formatError(error, {
      componentName: event.filename,
      lifecycle: 'error',
    })

    this.addToLog(context)

    const userMessage = this.getUserFriendlyMessage(error)
    if (userMessage) {
      this.showErrorToUser(userMessage)
    }
  }

  private getUserFriendlyMessage(error: Error): string | null {
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

    if (error.message?.includes('Network')) {
      return '网络连接失败，请检查网络设置'
    }

    if (error.message?.includes('timeout')) {
      return '请求超时，请重试'
    }

    return null
  }

  reportError(error: Error, context?: any): void {
    const errorContext = this.formatError(error, context)
    this.addToLog(errorContext)

    const userMessage = this.getUserFriendlyMessage(error)
    if (userMessage) {
      this.showErrorToUser(userMessage)
    }
  }

  getLogs(): ErrorContext[] {
    return this.getErrorLog()
  }

  clearLogs(): void {
    this.clearErrorLog()
  }
}

export const globalErrorHandler = GlobalErrorHandler.getInstance()
