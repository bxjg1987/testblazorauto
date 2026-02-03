import { un, type CreateAxiosDefaults, type InternalAxiosRequestConfig } from '@uni-helper/uni-network'

interface RequestOptions {
  url: string
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH'
  data?: any
  headers?: Record<string, string>
  loading?: boolean
  loadingText?: string
}

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
  errors: any[]

  constructor(message: string, errors: any[]) {
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

const handleUnauthorized = () => {
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

const handleForbidden = () => {
  uni.showModal({
    title: '权限不足',
    content: '您没有权限执行此操作，请联系管理员',
    showCancel: false,
    confirmText: '知道了',
  })
}

const responseUnpackInterceptor = (response: any) => {
  if (!response || typeof response !== 'object' || !response.__abp) {
    return response
  }

  const ajaxResponse = response as any

  if (!ajaxResponse.success) {
    const error = ajaxResponse.error
    
    if (error.code === 401 || ajaxResponse.unAuthorizedRequest) {
      throw new UnauthorizedException(error.message)
    }
    if (error.code === 403) {
      throw new ForbiddenException(error.message)
    }
    
    throw new UserFriendlyException(error.code, error.message, error.details)
  }

  const result = ajaxResponse.result
  if (result && typeof result === 'object' && 'ids' in result && 'errorMessage' in result) {
    const batchResult = result

    if (!batchResult.ids || batchResult.ids.length === 0) {
      const errorMessage = batchResult.errorMessage?.map((e: any) => e.message).join('\n') || '操作失败'
      throw new UserFriendlyException(500, '操作失败', errorMessage)
    }

    if (batchResult.errorMessage && batchResult.errorMessage.length > 0) {
      throw new BatchOperationException('部分操作失败', batchResult.errorMessage)
    }
  }

  return ajaxResponse.result
}

const errorInterceptor = (error: any) => {
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
    const message = error.errors.map((e: any) => e.message).join('\n')
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

const getBaseUrl = (): string => {
  const savedUrl = uni.getStorageSync('apiBaseUrl')
  if (savedUrl) {
    return savedUrl
  }
  return '/api'
}

export const getApiBaseUrl = () => getBaseUrl()

export const setApiBaseUrl = (url: string) => {
  uni.setStorageSync('apiBaseUrl', url)
}

const defaultConfig: CreateAxiosDefaults = {
  baseUrl: getBaseUrl(),
  timeout: 10000,
}

const network = un.create(defaultConfig)

network.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const token = uni.getStorageSync('token')
  
  if (token) {
    config.headers = config.headers || {}
    config.headers.Authorization = `Bearer ${token}`
  }
  
  if (config.method === 'GET' && config.data && typeof config.data === 'object') {
    config.data = {
      ...config.data,
      _t: Date.now(),
    }
  }
  
  return config
})

network.interceptors.response.use(
  (response) => responseUnpackInterceptor(response.data),
  (error) => {
    errorInterceptor(error)
    return Promise.reject(error)
  }
)

export const request = <T = any>(options: RequestOptions): Promise<T> => {
  const { url, method = 'GET', data, headers = {}, loading = true, loadingText = '加载中...' } = options
  
  if (loading) {
    uni.showLoading({
      title: loadingText,
      mask: true,
    })
  }
  
  return network.request({
    url,
    method,
    data,
    headers,
  }).then((res) => {
    if (loading) {
      uni.hideLoading()
    }
    return res as T
  }).catch((err) => {
    if (loading) {
      uni.hideLoading()
    }
    errorInterceptor(err)
    throw err
  })
}

export const get = <T = any>(url: string, params?: any, options?: Partial<RequestOptions>): Promise<T> => {
  return request<T>({
    url,
    method: 'GET',
    data: params,
    ...options,
  })
}

export const post = <T = any>(url: string, data?: any, options?: Partial<RequestOptions>): Promise<T> => {
  return request<T>({
    url,
    method: 'POST',
    data,
    ...options,
  })
}

export const put = <T = any>(url: string, data?: any, options?: Partial<RequestOptions>): Promise<T> => {
  return request<T>({
    url,
    method: 'PUT',
    data,
    ...options,
  })
}

export const del = <T = any>(url: string, params?: any, options?: Partial<RequestOptions>): Promise<T> => {
  return request<T>({
    url,
    method: 'DELETE',
    data: params,
    ...options,
  })
}

export default {
  request,
  get,
  post,
  put,
  delete: del,
}
