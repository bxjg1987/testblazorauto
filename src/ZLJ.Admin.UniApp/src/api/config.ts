import { get } from '@/utils/http'

export interface AbpMultiTenancyConfigDto {
  isEnabled: boolean
}

export interface AbpUserSessionConfigDto {
  userId?: number
  userName?: string
  tenantId?: number
  tenantName?: string
}

export interface AbpUserLocalizationConfigDto {
  currentCulture: {
    displayName: string
    name: string
  }
  languages: Array<{
    displayName: string
    name: string
  }>
}

export interface AbpUserFeatureConfigDto {
  allFeatures: Record<string, any>
}

export interface AbpUserAuthConfigDto {
  policy: string
}

export interface AbpUserNavConfigDto {
  menu: any[]
}

export interface AbpUserSettingConfigDto {
  theme: string
}

export interface AbpUserClockConfigDto {
  enabled: boolean
}

export interface AbpUserTimingConfigDto {
  enabled: boolean
}

export interface AbpUserSecurityConfigDto {
  enabled: boolean
}

export interface AbpUserConfigurationDto {
  multiTenancy: AbpMultiTenancyConfigDto
  session: AbpUserSessionConfigDto
  localization: AbpUserLocalizationConfigDto
  features: AbpUserFeatureConfigDto
  auth: AbpUserAuthConfigDto
  nav: AbpUserNavConfigDto
  setting: AbpUserSettingConfigDto
  clock: AbpUserClockConfigDto
  timing: AbpUserTimingConfigDto
  security: AbpUserSecurityConfigDto
  custom: Record<string, any>
}

export function getUserConfiguration(): Promise<AbpUserConfigurationDto> {
  return get<AbpUserConfigurationDto>('/AbpUserConfiguration/GetAll', {}, { loading: false })
}
