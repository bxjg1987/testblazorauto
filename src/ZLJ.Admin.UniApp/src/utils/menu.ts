import type { MenuDto, MenuItemDto, MenuItemCustomData } from '@/types/menu'

/**
 * 将菜单 URL 转换为页面路径
 * @param url 原始 URL
 * @returns 标准化的页面路径
 */
export const normalizeMenuUrl = (url: string): string => {
  if (!url) return ''

  let normalizedUrl = url

  if (!normalizedUrl.startsWith('/pages/')) {
    if (normalizedUrl.startsWith('/')) {
      normalizedUrl = '/pages' + normalizedUrl
    } else {
      normalizedUrl = '/pages/' + normalizedUrl
    }
  }

  if (!normalizedUrl.endsWith('/index')) {
    normalizedUrl = normalizedUrl + '/index'
  }

  return normalizedUrl
}

/**
 * 过滤出底部导航菜单项 (mobileShowModel === 2 Main 主菜单)
 * @param menu 菜单数据
 * @returns 底部菜单项列表
 */
export const filterBottomMenus = (menu: MenuDto): MenuItemDto[] => {
  const result: MenuItemDto[] = []

  function findBottomMenus(items: MenuItemDto[]) {
    for (const item of items) {
      const customData = item.customData as MenuItemCustomData | null

      if (customData && customData.mobileShowModel === 2) {
        result.push(item)
      }

      if (item.items && item.items.length > 0) {
        findBottomMenus(item.items)
      }
    }
  }

  if (menu) {
    for (const key in menu) {
      const group = menu[key]
      if (group && group.items) {
        findBottomMenus(group.items)
      }
    }
  }

  return result.sort((a, b) => a.order - b.order)
}

/**
 * 过滤出侧边菜单项 (mobileShowModel === 4 Normal 或未设置)
 * @param menu 菜单数据
 * @returns 侧边菜单项列表
 */
export const filterSideMenus = (menu: MenuDto): MenuItemDto[] => {
  const result: MenuItemDto[] = []

  function findSideMenus(items: MenuItemDto[]) {
    for (const item of items) {
      const customData = item.customData as MenuItemCustomData | null

      const showModel = customData?.mobileShowModel
      if (!showModel || showModel === 4) {
        result.push(item)
      }

      if (item.items && item.items.length > 0) {
        findSideMenus(item.items)
      }
    }
  }

  if (menu) {
    for (const key in menu) {
      const group = menu[key]
      if (group && group.items) {
        findSideMenus(group.items)
      }
    }
  }

  return result.sort((a, b) => a.order - b.order)
}

/**
 * 兼容旧方法：过滤出主菜单项
 * @deprecated 使用 filterBottomMenus 代替
 */
export const filterMainMenus = filterBottomMenus

/**
 * 导航到菜单项对应的页面
 * @param item 菜单项
 * @param onSuccess 成功回调
 * @param onFail 失败回调
 */
export const navigateToMenuItem = (
  item: MenuItemDto,
  onSuccess?: () => void,
  onFail?: (err: any) => void
): void => {
  if (!item.url) {
    return
  }

  const url = normalizeMenuUrl(item.url)

  uni.redirectTo({
    url,
    success: () => {
      onSuccess?.()
    },
    fail: (err) => {
      uni.showToast({
        title: '页面不存在',
        icon: 'none',
      })
      onFail?.(err)
    },
  })
}

/**
 * 检查当前页面是否为指定菜单项
 * @param item 菜单项
 * @param currentPath 当前页面路径
 * @returns 是否匹配
 */
export const isMenuItemActive = (item: MenuItemDto, currentPath: string): boolean => {
  if (!item.url) return false
  const normalizedUrl = normalizeMenuUrl(item.url)
  return currentPath === normalizedUrl
}
