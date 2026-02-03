import type { MenuDto, MenuItemDto } from '@/types/menu'

export function filterMainMenus(menu: MenuDto): MenuItemDto[] {
  const result: MenuItemDto[] = []
  
  function findMainMenus(items: MenuItemDto[]) {
    for (const item of items) {
      if (item.customData && item.customData.isMain === true) {
        result.push(item)
      }
      if (item.items && item.items.length > 0) {
        findMainMenus(item.items)
      }
    }
  }
  
  if (menu) {
    for (const key in menu) {
      const group = menu[key]
      if (group && group.items) {
        findMainMenus(group.items)
      }
    }
  }
  
  return result.sort((a, b) => a.order - b.order)
}
