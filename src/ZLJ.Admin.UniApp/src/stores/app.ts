import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { MenuDto } from '@/types/menu'

/**
 * 应用状态管理 Store
 */
export const useAppStore = defineStore('app', () => {
  const abpConfig = ref<any>(null)
  const currentUser = ref<any>(null)
  const menu = ref<MenuDto>({})
  const globalLoading = ref(false)
  const globalLoadingText = ref('加载中...')

  const setAbpConfig = (config: any) => {
    abpConfig.value = config
  }

  const setCurrentUser = (user: any) => {
    currentUser.value = user
  }

  const setMenu = (menuData: MenuDto) => {
    menu.value = menuData
  }

  const showGlobalLoading = (text: string = '加载中...') => {
    globalLoading.value = true
    globalLoadingText.value = text
  }

  const hideGlobalLoading = () => {
    globalLoading.value = false
  }

  const clearAppData = () => {
    abpConfig.value = null
    currentUser.value = null
    menu.value = {}
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
