import { un, type CreateAxiosDefaults, type InternalAxiosRequestConfig } from '@uni-helper/uni-network'
import type { AjaxResponse, ErrorInfo, BatchOperationOutput } from '@/types/ajax-response'
import {
  UserFriendlyException,
  BatchOperationException,
  UnauthorizedException,
  ForbiddenException,
  NetworkException,
  TimeoutException,
} from '@/utils/exceptions'

const pendingRequests = new Map<string, AbortController>()

const handleUnauthorized = (): void => {
  uni.removeStorageSync('token')
  uni.removeStorageSync('userInfo')
  uni.showToast({ title: '请先登录', icon: 'none', duration: 2000 })
  setTimeout(() => {
    const pages = uni.getCurrentPages()
    const currentPage = pages[pages.length - 1]
    const returnUrl = currentPage ? `/${currentPage.route}` : ''
    uni.reLaunch({ 
      url: returnUrl ? `/pages/login/index?returnUrl=${encodeURIComponent(returnUrl)}` : '/pages/login/index' 
    })
  }, 1500)
}

const handleForbidden = (): void => {
  uni.showModal({
    title: '权限不足',
    content: '您没有权限执行此操作，请联系管理员',
    showCancel: false,
    confirmText: '知道了',
  })
}

const generateRequestKey = (config: InternalAxiosRequestConfig): string => {
  return `${config.method}-${config.url}-${JSON.stringify(config.data || {})}`
}

const addPendingRequest = (config: InternalAxiosRequestConfig): void => {
  const key = generateRequestKey(config)
  if (pendingRequests.has(key)) {
    pendingRequests.get(key)?.abort()
  }
  const controller = new AbortController()
  config.signal = controller.signal
  pendingRequests.set(key, controller)
}

const removePendingRequest = (config: InternalAxiosRequestConfig): void => {
  pendingRequests.delete(generateRequestKey(config))
}

export const cancelAllRequests = (): void => {
  pendingRequests.forEach((controller) => controller.abort())
  pendingRequests.clear()
}

const getBaseUrl = (): string => uni.getStorageSync('apiBaseUrl') || '/api'

export const getApiBaseUrl = (): string => getBaseUrl()

export const setApiBaseUrl = (url: string): void => {
  uni.setStorageSync('apiBaseUrl', url)
}

const network = un.create({
  baseUrl: getBaseUrl(),
  timeout: 30000,
} as CreateAxiosDefaults)

network.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  addPendingRequest(config)

  const token = uni.getStorageSync('token')
  if (token) {
    config.headers = config.headers || {}
    config.headers.Authorization = `Bearer ${token}`
  }

  if (config.method === 'GET' && config.data && typeof config.data === 'object') {
    config.data = { ...config.data, _t: Date.now() }
  }

  return config
})

network.interceptors.response.use(
  (response) => {
    removePendingRequest(response.config)

    const data = response.data
    if (!data || typeof data !== 'object' || !data.__abp) {
      return data
    }

    const ajaxResponse = data as AjaxResponse

    if (!ajaxResponse.success) {
      const error = ajaxResponse.error as ErrorInfo

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
      const batchResult = result as BatchOperationOutput

      if (!batchResult.ids || batchResult.ids.length === 0) {
        throw new UserFriendlyException(500, '操作失败', batchResult.errorMessage?.map((e) => e.message).join('\n') || '操作失败')
      }

      if (batchResult.errorMessage && batchResult.errorMessage.length > 0) {
        throw new BatchOperationException('部分操作失败', batchResult.errorMessage)
      }
    }

    return ajaxResponse.result
  },
  (error) => {
    if (error.config) {
      removePendingRequest(error.config)
    }

    if (error.name === 'AbortError' || error.name === 'CanceledError') {
      return Promise.reject(error)
    }

    if (error instanceof UnauthorizedException) {
      handleUnauthorized()
    } else if (error instanceof ForbiddenException) {
      handleForbidden()
    } else if (error instanceof UserFriendlyException) {
      uni.showToast({ title: error.message, icon: 'none', duration: 3000 })
    } else if (error instanceof BatchOperationException) {
      uni.showModal({
        title: '部分操作失败',
        content: error.errors.map((e) => e.message).join('\n'),
        showCancel: false,
      })
    } else if (error.statusCode) {
      switch (error.statusCode) {
        case 401: handleUnauthorized(); break
        case 403: handleForbidden(); break
        case 404: uni.showToast({ title: '请求的资源不存在', icon: 'none' }); break
        case 500: uni.showToast({ title: '服务器内部错误', icon: 'none' }); break
        default: uni.showToast({ title: `请求失败(${error.statusCode})`, icon: 'none' })
      }
    } else if (error.errMsg?.includes('request:fail')) {
      throw new NetworkException('网络连接失败，请检查网络')
    } else if (error.errMsg?.includes('timeout')) {
      throw new TimeoutException('请求超时，请重试')
    } else {
      uni.showToast({ title: error.message || '请求失败', icon: 'none' })
    }

    return Promise.reject(error)
  }
)

export interface RequestOptions {
  url: string
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH'
  data?: any
  headers?: Record<string, string>
  loading?: boolean
  loadingText?: string
}

export const request = <T = any>(options: RequestOptions): Promise<T> => {
  const { url, method = 'GET', data, headers = {}, loading = true, loadingText = '加载中...' } = options

  if (loading) {
    uni.showLoading({ title: loadingText, mask: true })
  }

  return network
    .request({ url, method, data, headers })
    .then((res) => {
      if (loading) uni.hideLoading()
      return res as T
    })
    .catch((err) => {
      if (loading) uni.hideLoading()
      throw err
    })
}

export const get = <T = any>(url: string, params?: any, options?: Partial<RequestOptions>): Promise<T> =>
  request<T>({ url, method: 'GET', data: params, ...options })

export const post = <T = any>(url: string, data?: any, options?: Partial<RequestOptions>): Promise<T> =>
  request<T>({ url, method: 'POST', data, ...options })

export const put = <T = any>(url: string, data?: any, options?: Partial<RequestOptions>): Promise<T> =>
  request<T>({ url, method: 'PUT', data, ...options })

export const del = <T = any>(url: string, params?: any, options?: Partial<RequestOptions>): Promise<T> =>
  request<T>({ url, method: 'DELETE', data: params, ...options })

export default { request, get, post, put, delete: del, cancelAllRequests }
