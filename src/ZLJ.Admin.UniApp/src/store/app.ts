import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { AbpUserConfigurationDto } from '@/api/config'
import type { GetCurrentLoginInformationsOutput } from '@/api/session'
import type { MenuDto } from '@/types/menu'

/** 应用状态管理 Store */
export const useAppStore = defineStore('app', () => {
  const abpConfig = ref<AbpUserConfigurationDto | null>(null)
  const currentUser = ref<GetCurrentLoginInformationsOutput | null>(null)
  const menu = ref<MenuDto>({})
  const globalLoading = ref(false)
  const globalLoadingText = ref('加载中...')

  /** 设置 ABP 配置 */
  const setAbpConfig = (config: AbpUserConfigurationDto) => {
    abpConfig.value = config
  }

  /** 设置当前用户信息 */
  const setCurrentUser = (user: GetCurrentLoginInformationsOutput) => {
    currentUser.value = user
  }

  /** 设置菜单数据 */
  const setMenu = (menuData: MenuDto) => {
    menu.value = menuData
  }

  /** 显示全局加载状态 */
  const showGlobalLoading = (text: string = '加载中...') => {
    globalLoadingText.value = text
    globalLoading.value = true
  }

  /** 隐藏全局加载状态 */
  const hideGlobalLoading = () => {
    globalLoading.value = false
  }

  /** 清除应用数据 */
  const clearAppData = () => {
    abpConfig.value = null
    currentUser.value = null
    menu.value = {}
    globalLoading.value = false
    globalLoadingText.value = '加载中...'
  }

  return {
    abpConfig,
    currentUser,
    menu,
    globalLoading,
    globalLoadingText,
    setAbpConfig,
    setCurrentUser,
    setMenu,
    showGlobalLoading,
    hideGlobalLoading,
    clearAppData,
  }
})
