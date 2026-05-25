import { get } from '@/utils/http'

/** 应用信息 */
export interface ApplicationInfoDto {
  version: string
  releaseDate: string
  features: Record<string, boolean>
  runTimeVersion: string
  abpVersion: string
}

/** 用户登录信息 */
export interface UserLoginInfoDto {
  id: number
  userName: string
  name: string
  surname: string
  emailAddress: string
  phoneNumber: string
}

/** 租户登录信息 */
export interface TenantLoginInfoDto {
  id: number
  name: string
}

/** 获取当前登录信息输出 */
export interface GetCurrentLoginInformationsOutput {
  application: ApplicationInfoDto
  user: UserLoginInfoDto
  tenant: TenantLoginInfoDto
}

/** 获取当前登录信息 */
export function getCurrentLoginInformations(): Promise<GetCurrentLoginInformationsOutput> {
  return get<GetCurrentLoginInformationsOutput>('api/services/common/Session/GetCurrentLoginInformations')
}
