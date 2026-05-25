import type { BatchOperationErrorMessage } from '@/types/ajax-response'

/** 业务异常，后端返回的用户友好错误 */
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

/** 未登录异常（401） */
export class UnauthorizedException extends Error {
  constructor(message: string = '未登录') {
    super(message)
    this.name = 'UnauthorizedException'
  }
}

/** 无权限异常（403） */
export class ForbiddenException extends Error {
  constructor(message: string = '无权限') {
    super(message)
    this.name = 'ForbiddenException'
  }
}

/** 网络连接失败异常 */
export class NetworkException extends Error {
  constructor(message: string = '网络连接失败') {
    super(message)
    this.name = 'NetworkException'
  }
}

/** 请求超时异常 */
export class TimeoutException extends Error {
  constructor(message: string = '请求超时') {
    super(message)
    this.name = 'TimeoutException'
  }
}

/** 批量操作部分失败异常 */
export class BatchOperationException extends Error {
  errors: BatchOperationErrorMessage[]

  constructor(message: string, errors: BatchOperationErrorMessage[]) {
    super(message)
    this.name = 'BatchOperationException'
    this.errors = errors
  }
}
