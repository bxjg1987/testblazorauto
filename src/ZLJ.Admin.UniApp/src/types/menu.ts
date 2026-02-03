export interface MenuItemDto {
  name: string
  icon: string
  displayName: string
  order: number
  url: string | null
  customData: Record<string, any> | null
  items: MenuItemDto[] | null
}

export interface MenuGroupDto {
  name: string
  displayName: string
  customData: Record<string, any> | null
  items: MenuItemDto[] | null
}

export interface MenuDto {
  [key: string]: MenuGroupDto
}
