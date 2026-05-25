import { useAppStore } from '@/store/app'
import { filterBottomMenus, filterSideMenus, navigateToMenuItem, isMenuItemActive } from '@/utils/menu'
import type { MenuItemDto, MenuDto } from '@/types/menu'

/** 菜单相关组合式函数 */
export function useMenu() {
  const appStore = useAppStore()

  /** 菜单数据（响应式） */
  const menu = computed(() => appStore.menu)

  /** 底部导航菜单项 */
  const bottomMenus = computed(() => filterBottomMenus(appStore.menu))

  /** 侧边菜单项 */
  const sideMenus = computed(() => filterSideMenus(appStore.menu))

  /** 导航到菜单项 */
  const navigateTo = (item: MenuItemDto, onSuccess?: () => void, onFail?: (err: unknown) => void) => {
    navigateToMenuItem(item, onSuccess, onFail)
  }

  /** 判断菜单项是否激活 */
  const isActive = (item: MenuItemDto, currentPath: string) => {
    return isMenuItemActive(item, currentPath)
  }

  return { menu, bottomMenus, sideMenus, navigateTo, isActive }
}
