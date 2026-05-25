import { un } from '@uni-helper/uni-network'
import type { AjaxResponse, BatchOperationOutput } from '@/types/ajax-response'
import {
  UserFriendlyException,
  UnauthorizedException,
  ForbiddenException,
  NetworkException,
  TimeoutException,
  BatchOperationException,
} from '@/utils/exceptions'

/** 请求选项 */
export interface RequestOptions {
  url: string
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH'
  data?: unknown
  headers?: Record<string, string>
  silent?: boolean
}

const pendingRequests = new Map<string, AbortController>()

/** 生成请求唯一标识（用于请求去重） */
function generateRequestKey(config: Record<string, unknown>): string {
  const { url, method, data } = config
  const params = config.params as Record<string, unknown> | undefined
  const filteredParams = params
    ? Object.fromEntries(Object.entries(params).filter(([k]) => k !== '_t'))
    : undefined
  return [url, method, JSON.stringify(filteredParams), JSON.stringify(data)].join('&')
}

/** 添加待处理请求，若存在相同请求则取消前一个 */
function addPendingRequest(config: Record<string, unknown>): void {
  const key = generateRequestKey(config)
  if (pendingRequests.has(key)) {
    pendingRequests.get(key)!.abort()
  }
  const controller = new AbortController()
  config.signal = controller.signal
  pendingRequests.set(key, controller)
}

/** 移除待处理请求 */
function removePendingRequest(config: Record<string, unknown>): void {
  const key = generateRequestKey(config)
  pendingRequests.delete(key)
}

/** 处理未授权（401）：清除登录信息并跳转登录页 */
function handleUnauthorized(silent: boolean): void {
  if (silent) return
  uni.removeStorageSync('token')
  uni.removeStorageSync('userInfo')
  uni.showToast({ title: '请先登录', icon: 'none' })
  const pages = getCurrentPages()
  const currentPage = pages[pages.length - 1]
  const route = currentPage ? (currentPage as { route: string }).route : ''
  const returnUrl = route ? `?returnUrl=${encodeURIComponent('/' + route)}` : ''
  setTimeout(() => {
    uni.navigateTo({ url: `/pages/login${returnUrl}` })
  }, 1500)
}

/** 处理无权限（403） */
function handleForbidden(silent: boolean): void {
  if (silent) return
  uni.showModal({ title: '提示', content: '权限不足', showCancel: false })
}

/** 判断是否为批量操作输出结果 */
function isBatchOperationOutput(result: unknown): result is BatchOperationOutput {
  if (!result || typeof result !== 'object') return false
  const obj = result as Record<string, unknown>
  return 'ids' in obj && 'errorMessage' in obj
}

/** 获取API基础地址 */
export function getApiBaseUrl(): string {
  return uni.getStorageSync('apiBaseUrl') || '/api'
}

/** 设置API基础地址并持久化到本地存储 */
export function setApiBaseUrl(url: string): void {
  uni.setStorageSync('apiBaseUrl', url)
  http.defaults.baseUrl = url
}

/** 网络请求实例 */
const http = un.create({
  baseUrl: getApiBaseUrl(),
  timeout: 30000,
})

http.interceptors.request.use(
  (config) => {
    const token = uni.getStorageSync('token')
    if (token) {
      config.headers = {
        ...config.headers,
        Authorization: `Bearer ${token}`,
      } as Record<string, string>
    }
    addPendingRequest(config as unknown as Record<string, unknown>)
    if (config.method?.toUpperCase() === 'GET') {
      config.params = {
        ...(config.params as Record<string, unknown> | undefined),
        _t: Date.now(),
      }
    }
    return config
  },
  (error) => Promise.reject(error),
)

http.interceptors.response.use(
  (response) => {
    const config = response.config as unknown as Record<string, unknown>
    removePendingRequest(config)
    const silent = !!config._silent
    const data = response.data as AjaxResponse<unknown>
    if (data?.__abp) {
      if (data.success) {
        const result = data.result
        if (isBatchOperationOutput(result) && result.errorMessage.length > 0) {
          const ex = new BatchOperationException('批量操作部分失败', result.errorMessage)
          if (!silent) {
            uni.showModal({
              title: '批量操作结果',
              content: result.errorMessage.map((e) => e.message).join('\n'),
              showCancel: false,
            })
          }
          throw ex
        }
        return result
      }
      const error = data.error
      if (data.unAuthorizedRequest || error?.code === 401) {
        handleUnauthorized(silent)
        throw new UnauthorizedException(error?.message || '未登录')
      }
      if (error?.code === 403) {
        handleForbidden(silent)
        throw new ForbiddenException(error?.message || '无权限')
      }
      const ex = new UserFriendlyException(
        error?.code || 0,
        error?.message || '操作失败',
        error?.details,
      )
      if (!silent) {
        uni.showToast({ title: ex.message, icon: 'none' })
      }
      throw ex
    }
    return response.data
  },
  (error) => {
    const err = error as {
      config?: Record<string, unknown>
      response?: { status: number }
      message?: string
      code?: string
      name?: string
    }
    const config = err?.config
    if (config) {
      removePendingRequest(config)
    }
    const silent = !!(config?._silent)
    if (un.isCancel(error) || err?.name === 'AbortError') {
      return Promise.reject(error)
    }
    if (err?.code === 'ECONNABORTED' || err?.message?.includes('timeout')) {
      if (!silent) {
        uni.showToast({ title: '请求超时', icon: 'none' })
      }
      return Promise.reject(new TimeoutException())
    }
    if (!err?.response) {
      if (!silent) {
        uni.showToast({ title: '网络连接失败', icon: 'none' })
      }
      return Promise.reject(new NetworkException())
    }
    const status = err.response.status
    if (status === 401) {
      handleUnauthorized(silent)
      return Promise.reject(new UnauthorizedException())
    }
    if (status === 403) {
      handleForbidden(silent)
      return Promise.reject(new ForbiddenException())
    }
    const messages: Record<number, string> = {
      400: '请求参数错误',
      404: '请求资源不存在',
      405: '请求方法不允许',
      408: '请求超时',
      500: '服务器内部错误',
      502: '网关错误',
      503: '服务不可用',
      504: '网关超时',
    }
    const msg = messages[status] || `请求失败(${status})`
    if (!silent) {
      uni.showToast({ title: msg, icon: 'none' })
    }
    return Promise.reject(new UserFriendlyException(status, msg))
  },
)

/** 发起网络请求 */
export function request<T>(options: RequestOptions): Promise<T> {
  const method = (options.method || 'GET').toUpperCase()
  const isQueryMethod = method === 'GET' || method === 'DELETE'
  const config: Record<string, unknown> = {
    url: options.url,
    method,
    headers: options.headers,
    _silent: options.silent,
  }
  if (isQueryMethod) {
    config.params = options.data
  } else {
    config.data = options.data
  }
  return http.request(config as never) as Promise<T>
}

/** 发起GET请求 */
export function get<T>(
  url: string,
  params?: Record<string, unknown>,
  options?: Partial<RequestOptions>,
): Promise<T> {
  return request<T>({ url, method: 'GET', data: params, ...options })
}

/** 发起POST请求 */
export function post<T>(
  url: string,
  data?: unknown,
  options?: Partial<RequestOptions>,
): Promise<T> {
  return request<T>({ url, method: 'POST', data, ...options })
}

/** 发起PUT请求 */
export function put<T>(
  url: string,
  data?: unknown,
  options?: Partial<RequestOptions>,
): Promise<T> {
  return request<T>({ url, method: 'PUT', data, ...options })
}

/** 发起DELETE请求 */
export function del<T>(
  url: string,
  params?: Record<string, unknown>,
  options?: Partial<RequestOptions>,
): Promise<T> {
  return request<T>({ url, method: 'DELETE', data: params, ...options })
}

/** 取消所有待处理请求 */
export function cancelAllRequests(): void {
  pendingRequests.forEach((controller) => controller.abort())
  pendingRequests.clear()
}
