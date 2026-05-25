import { post } from '@/utils/http'

/** 登录认证输入模型 */
export interface AuthenticateModel {
  userNameOrEmailAddress: string
  password: string
  tenancyName?: string
  rememberClient?: boolean
  yzmKey?: string
  yzmValue?: string
}

/** 登录认证结果模型 */
export interface AuthenticateResultModel {
  accessToken: string
  encryptedAccessToken: string
  expireInSeconds: number
  userId: number
}

/** 用户登录认证 */
export function authenticate(input: AuthenticateModel): Promise<AuthenticateResultModel> {
  return post<AuthenticateResultModel>('/api/TokenAuth/Authenticate', input, { loading: false })
}
