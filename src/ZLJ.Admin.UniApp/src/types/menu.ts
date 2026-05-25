/** 菜单显示模式枚举 */
export enum MenuShowModel {
  /** 不显示 */
  None = 1,
  /** 主菜单（底部导航） */
  Main = 2,
  /** 普通（侧边菜单） */
  Normal = 4,
}

/** 菜单项自定义数据 */
export interface MenuItemCustomData {
  /** 移动端显示模式：1=不显示, 2=主菜单(底部导航), 4=普通(侧边菜单) */
  mobileShowModel?: number
  /** 移动端显示文本 */
  mobileText?: string
  /** 移动端图标 */
  mobileIcon?: string
  /** 其他扩展属性 */
  [key: string]: unknown
}

/** 菜单项 DTO */
export interface MenuItemDto {
  /** 菜单名称（唯一标识） */
  name: string
  /** 菜单图标 */
  icon: string
  /** 菜单显示名称 */
  displayName: string
  /** 排序序号 */
  order: number
  /** 菜单链接地址 */
  url: string
  /** 自定义数据 */
  customData?: MenuItemCustomData
  /** 子菜单项 */
  items?: MenuItemDto[]
}

/** 菜单分组 DTO */
export interface MenuGroupDto {
  /** 分组名称 */
  name: string
  /** 分组显示名称 */
  displayName: string
  /** 自定义数据 */
  customData: MenuItemCustomData
  /** 分组下的菜单项列表 */
  items: MenuItemDto[]
}

/** 已知的菜单分组名称 */
export type KnownMenuGroupName = 'MainMenu' | 'AdminMenu' | 'TenantMenu' | 'UserMenu'

/** 菜单数据 DTO（键值对结构，键为分组名称） */
export interface MenuDto {
  /** 主菜单 */
  MainMenu?: MenuGroupDto
  /** 管理菜单 */
  AdminMenu?: MenuGroupDto
  /** 租户菜单 */
  TenantMenu?: MenuGroupDto
  /** 用户菜单 */
  UserMenu?: MenuGroupDto
  /** 其他分组名称对应的菜单分组 */
  [key: string]: MenuGroupDto | undefined
}
