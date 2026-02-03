export interface ErrorInfo {
  code: number
  message: string
  details: string | null
  validationErrors: ValidationErrorInfo[] | null
}

export interface ValidationErrorInfo {
  message: string
  members: string[]
}

export interface AjaxResponse<T = any> {
  success: boolean
  result: T | null
  error: ErrorInfo | null
  targetUrl: string | null
  unAuthorizedRequest: boolean
  __abp: boolean
}

export interface BatchOperationErrorMessage {
  id: string | number
  message: string
}

export interface BatchOperationOutput {
  ids: (string | number)[]
  errorMessage: BatchOperationErrorMessage[]
}
