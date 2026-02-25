import type { BatchOperationErrorMessage } from '@/types/ajax-response'

/**
 * 用户友好异常，用于显示后端返回的业务错误信息
 */
export class UserFriendlyException extends Error {
  code: number
  details: string | null

  constructor(code: number, message: string, details: string | null = null) {
    super(message)
    this.name = 'UserFriendlyException'
    this.code = code
    this.details = details
  }
}

/**
 * 批量操作异常，用于处理批量操作中部分失败的情况
 */
export class BatchOperationException extends Error {
  errors: BatchOperationErrorMessage[]

  constructor(message: string, errors: BatchOperationErrorMessage[]) {
    super(message)
    this.name = 'BatchOperationException'
    this.errors = errors
  }
}

/**
 * 未授权异常，用于处理401未登录情况
 */
export class UnauthorizedException extends Error {
  constructor(message: string = '未登录') {
    super(message)
    this.name = 'UnauthorizedException'
  }
}

/**
 * 禁止访问异常，用于处理403无权限情况
 */
export class ForbiddenException extends Error {
  constructor(message: string = '无权限') {
    super(message)
    this.name = 'ForbiddenException'
  }
}

/**
 * 网络异常，用于处理网络连接失败
 */
export class NetworkException extends Error {
  constructor(message: string = '网络连接失败') {
    super(message)
    this.name = 'NetworkException'
  }
}

/**
 * 超时异常，用于处理请求超时
 */
export class TimeoutException extends Error {
  constructor(message: string = '请求超时') {
    super(message)
    this.name = 'TimeoutException'
  }
}
