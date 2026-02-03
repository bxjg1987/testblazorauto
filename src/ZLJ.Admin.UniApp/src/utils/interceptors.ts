import type { AjaxResponse, ErrorInfo, BatchOperationOutput, BatchOperationErrorMessage } from '@/types/ajax-response'

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

export class BatchOperationException extends Error {
  errors: BatchOperationErrorMessage[]

  constructor(message: string, errors: BatchOperationErrorMessage[]) {
    super(message)
    this.name = 'BatchOperationException'
    this.errors = errors
  }
}

export class UnauthorizedException extends Error {
  constructor(message: string = '未登录') {
    super(message)
    this.name = 'UnauthorizedException'
  }
}

export class ForbiddenException extends Error {
  constructor(message: string = '无权限') {
    super(message)
    this.name = 'ForbiddenException'
  }
}

function handleUnauthorized(): void {
  uni.removeStorageSync('token')
  uni.removeStorageSync('userInfo')
  uni.showToast({
    title: '请先登录',
    icon: 'none',
    duration: 2000,
  })
  setTimeout(() => {
    uni.reLaunch({
      url: '/pages/login/index',
    })
  }, 1500)
}

function handleForbidden(): void {
  uni.showModal({
    title: '权限不足',
    content: '您没有权限执行此操作，请联系管理员',
    showCancel: false,
    confirmText: '知道了',
  })
}

export function responseUnpackInterceptor(response: any): any {
  if (!response || typeof response !== 'object' || !response.__abp) {
    return response
  }

  const ajaxResponse = response as AjaxResponse

  if (!ajaxResponse.success) {
    const error = ajaxResponse.error as ErrorInfo
    
    if (error.code === 401 || ajaxResponse.unAuthorizedRequest) {
      throw new UnauthorizedException(error.message)
    }
    if (error.code === 403) {
      throw new ForbiddenException(error.message)
    }
    
    throw new UserFriendlyException(
      error.code,
      error.message,
      error.details
    )
  }

  const result = ajaxResponse.result
  if (result && typeof result === 'object' && 'ids' in result && 'errorMessage' in result) {
    const batchResult = result as BatchOperationOutput

    if (!batchResult.ids || batchResult.ids.length === 0) {
      const errorMessage = batchResult.errorMessage?.map(e => e.message).join('\n') || '操作失败'
      throw new UserFriendlyException(500, '操作失败', errorMessage)
    }

    if (batchResult.errorMessage && batchResult.errorMessage.length > 0) {
      throw new BatchOperationException('部分操作失败', batchResult.errorMessage)
    }
  }

  return ajaxResponse.result
}

export function errorInterceptor(error: any): never {
  if (error instanceof UnauthorizedException) {
    handleUnauthorized()
    throw error
  }

  if (error instanceof ForbiddenException) {
    handleForbidden()
    throw error
  }

  if (error instanceof UserFriendlyException) {
    uni.showToast({
      title: error.message,
      icon: 'none',
      duration: 3000,
    })
    throw error
  }

  if (error instanceof BatchOperationException) {
    const message = error.errors.map(e => e.message).join('\n')
    uni.showModal({
      title: '部分操作失败',
      content: message,
      showCancel: false,
    })
    throw error
  }

  if (error.statusCode) {
    switch (error.statusCode) {
      case 401:
        handleUnauthorized()
        break
      case 403:
        handleForbidden()
        break
      case 404:
        uni.showToast({
          title: '请求的资源不存在',
          icon: 'none',
        })
        break
      case 500:
        uni.showToast({
          title: '服务器内部错误',
          icon: 'none',
        })
        break
      default:
        uni.showToast({
          title: `请求失败(${error.statusCode})`,
          icon: 'none',
        })
    }
    throw error
  }

  if (error.errMsg && error.errMsg.includes('request:fail')) {
    uni.showToast({
      title: '网络连接失败，请检查网络',
      icon: 'none',
    })
    throw error
  }

  uni.showToast({
    title: error.message || '请求失败',
    icon: 'none',
  })
  throw error
}
