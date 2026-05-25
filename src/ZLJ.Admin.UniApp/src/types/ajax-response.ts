/** 验证错误详情 */
export interface ValidationErrorInfo {
  /** 验证错误消息 */
  message: string
  /** 关联的成员（字段）列表 */
  members: string[]
}

/** ABP 错误信息 */
export interface ErrorInfo {
  /** 错误码 */
  code: number
  /** 错误消息 */
  message: string
  /** 错误详情 */
  details: string | null
  /** 验证错误列表 */
  validationErrors: ValidationErrorInfo[] | null
}

/** ABP 标准响应结构 */
export interface AjaxResponse<T> {
  /** 请求是否成功 */
  success: boolean
  /** 响应结果数据 */
  result: T | null
  /** 错误信息 */
  error: ErrorInfo | null
  /** 目标跳转地址 */
  targetUrl: string | null
  /** 是否为未授权请求 */
  unAuthorizedRequest: boolean
  /** ABP 标识 */
  __abp: boolean
}

/** 批量操作中的单条错误信息 */
export interface BatchOperationErrorMessage {
  /** 关联的记录标识 */
  id: string | number
  /** 错误消息 */
  message: string
}

/** 批量操作输出结果 */
export interface BatchOperationOutput {
  /** 成功处理的记录标识列表 */
  ids: (string | number)[]
  /** 失败记录的错误信息列表 */
  errorMessage: BatchOperationErrorMessage[]
}
