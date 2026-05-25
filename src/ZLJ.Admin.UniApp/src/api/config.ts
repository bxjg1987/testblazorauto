import { get } from '@/utils/http'
import type { MenuDto } from '@/types/menu'

/** 多租户配置 */
export interface AbpMultiTenancyConfigDto {
  isEnabled: boolean
}

/** 用户会话配置 */
export interface AbpUserSessionConfigDto {
  userId?: number
  userName?: string
  tenantId?: number
  tenantName?: string
}

/** 用户本地化配置 */
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

/** 用户功能配置 */
export interface AbpUserFeatureConfigDto {
  allFeatures: Record<string, boolean>
}

/** 用户认证配置 */
export interface AbpUserAuthConfigDto {
  policy: string
}

/** 用户导航配置 - menus 为键值对结构，与后端 Dictionary<string, UserMenu> 一致 */
export interface AbpUserNavConfigDto {
  menus: MenuDto
}

/** 用户设置配置 */
export interface AbpUserSettingConfigDto {
  theme: string
}

/** 用户时钟配置 */
export interface AbpUserClockConfigDto {
  enabled: boolean
}

/** 用户时间配置 */
export interface AbpUserTimingConfigDto {
  enabled: boolean
}

/** 用户安全配置 */
export interface AbpUserSecurityConfigDto {
  enabled: boolean
}

/** ABP 用户配置 */
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
  custom: Record<string, unknown>
}

/** 获取 ABP 用户配置 */
export function getUserConfiguration(): Promise<AbpUserConfigurationDto> {
  return get<AbpUserConfigurationDto>('/AbpUserConfiguration/GetAll', {}, { loading: false })
}
