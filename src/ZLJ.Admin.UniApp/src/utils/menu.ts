import type { MenuDto, MenuItemDto } from '@/types/menu'
import { MenuShowModel } from '@/types/menu'
import router from '@/router'

/** 将ABP后端菜单URL转换为扁平页面路由路径 */
export function normalizeMenuUrl(url: string): string {
  if (!url) return ''
  if (url.startsWith('/pages/')) return url
  if (url.startsWith('/')) return `/pages${url}`
  return `/pages/${url}`
}

/** 递归收集所有菜单项 */
function collectMenuItems(items: MenuItemDto[]): MenuItemDto[] {
  const result: MenuItemDto[] = []
  for (const item of items) {
    if (item.items && item.items.length > 0) {
      result.push(...collectMenuItems(item.items))
    } else {
      result.push(item)
    }
  }
  return result
}

/** 筛选底部导航菜单项（mobileShowModel === 2） */
export function filterBottomMenus(menu: MenuDto): MenuItemDto[] {
  const allItems: MenuItemDto[] = []
  for (const key of Object.keys(menu)) {
    const group = menu[key]
    if (group && group.items) {
      allItems.push(...collectMenuItems(group.items))
    }
  }
  return allItems
    .filter((item) => item.customData?.mobileShowModel === MenuShowModel.Main)
    .sort((a, b) => a.order - b.order)
}

/** 筛选侧边菜单项（mobileShowModel === 4 或未设置） */
export function filterSideMenus(menu: MenuDto): MenuItemDto[] {
  const allItems: MenuItemDto[] = []
  for (const key of Object.keys(menu)) {
    const group = menu[key]
    if (group && group.items) {
      allItems.push(...collectMenuItems(group.items))
    }
  }
  return allItems
    .filter(
      (item) =>
        item.customData?.mobileShowModel === MenuShowModel.Normal ||
        item.customData?.mobileShowModel === undefined,
    )
    .sort((a, b) => a.order - b.order)
}

/** 导航到菜单项对应的页面 */
export function navigateToMenuItem(
  item: MenuItemDto,
  onSuccess?: () => void,
  onFail?: (err: unknown) => void,
): void {
  const path = normalizeMenuUrl(item.url)
  router
    .push(path)
    .then(() => {
      onSuccess?.()
    })
    .catch((err: unknown) => {
      const errMsg = err instanceof Error ? err.message : String(err)
      if (errMsg.includes('not found') || errMsg.includes('不存在')) {
        uni.showToast({ title: '页面不存在', icon: 'none' })
      }
      onFail?.(err)
    })
}

/** 判断当前页面是否与菜单项匹配 */
export function isMenuItemActive(item: MenuItemDto, currentPath: string): boolean {
  const targetPath = normalizeMenuUrl(item.url)
  return currentPath === targetPath
}
