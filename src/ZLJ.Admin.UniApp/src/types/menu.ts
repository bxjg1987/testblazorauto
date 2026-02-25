/**
 * 菜单项数据传输对象
 */
export interface MenuItemDto {
  /** 菜单项名称（唯一标识） */
  name: string
  /** 菜单项图标 */
  icon: string
  /** 菜单项显示名称 */
  displayName: string
  /** 菜单项排序 */
  order: number
  /** 菜单项跳转URL */
  url: string | null
  /** 自定义数据 */
  customData: MenuItemCustomData | null
  /** 子菜单项列表 */
  items: MenuItemDto[] | null
}

/**
 * 菜单项自定义数据
 * mobileShowModel: 1-不显示，2-主菜单(底部导航)，4-普通(侧边菜单)
 */
export interface MenuItemCustomData {
  /** 移动端显示模式：1-不显示，2-主菜单(底部导航)，4-普通(侧边菜单) */
  mobileShowModel?: number
  /** 移动端显示文本 */
  mobileText?: string
  /** 移动端图标名称 */
  mobileIcon?: string
  /** 其他自定义属性 */
  [key: string]: any
}

/**
 * 菜单分组数据传输对象
 */
export interface MenuGroupDto {
  /** 分组名称 */
  name: string
  /** 分组显示名称 */
  displayName: string
  /** 自定义数据 */
  customData: Record<string, any> | null
  /** 分组下的菜单项列表 */
  items: MenuItemDto[] | null
}

/**
 * 已知菜单分组名称
 */
export type KnownMenuGroupName =
  | 'MainMenu'
  | 'AdminMenu'
  | 'TenantMenu'
  | 'UserMenu'

/**
 * 菜单数据传输对象
 */
export interface MenuDto {
  /** 主菜单 */
  MainMenu?: MenuGroupDto
  /** 管理菜单 */
  AdminMenu?: MenuGroupDto
  /** 租户菜单 */
  TenantMenu?: MenuGroupDto
  /** 用户菜单 */
  UserMenu?: MenuGroupDto
  /** 其他菜单分组 */
  [key: string]: MenuGroupDto | undefined
}

/**
 * 菜单显示模式枚举 (对应后端 MenuShowModel)
 */
export enum MenuShowModel {
  /** 不显示 */
  None = 1,
  /** 主菜单（底部导航） */
  Main = 2,
  /** 普通（侧边菜单） */
  Normal = 4,
}
