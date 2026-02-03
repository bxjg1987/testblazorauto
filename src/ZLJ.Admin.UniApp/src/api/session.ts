import { post } from '@/utils/http'

export interface ApplicationInfoDto {
  version: string
  releaseDate: string
  features: Record<string, boolean>
  runTimeVersion: string
  abpVersion: string
}

export interface UserLoginInfoDto {
  id: number
  userName: string
  name: string
  surname: string
  emailAddress: string
  phoneNumber: string
}

export interface TenantLoginInfoDto {
  id: number
  name: string
}

export interface GetCurrentLoginInformationsOutput {
  application: ApplicationInfoDto
  user: UserLoginInfoDto
  tenant: TenantLoginInfoDto
}

export function getCurrentLoginInformations(): Promise<GetCurrentLoginInformationsOutput> {
  return post<GetCurrentLoginInformationsOutput>('api/services/common/Session/GetCurrentLoginInformations', {}, { loading: false })
}
