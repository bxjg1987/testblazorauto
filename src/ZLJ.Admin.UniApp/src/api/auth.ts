import { post } from '@/utils/http'

export interface AuthenticateModel {
  userNameOrEmailAddress: string
  password: string
  tenancyName?: string
  rememberClient?: boolean
  yzmKey?: string
  yzmValue?: string
}

export interface AuthenticateResultModel {
  accessToken: string
  encryptedAccessToken: string
  expireInSeconds: number
  userId: number
}

export function authenticate(input: AuthenticateModel): Promise<AuthenticateResultModel> {
  return post<AuthenticateResultModel>('/api/TokenAuth/Authenticate', input, { loading: false })
}
