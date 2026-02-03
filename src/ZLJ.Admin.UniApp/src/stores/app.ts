import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { MenuDto } from '@/types/menu'

export const useAppStore = defineStore('app', () => {
  const abpConfig = ref<any>(null)
  const currentUser = ref<any>(null)
  const menu = ref<MenuDto>({})
  
  const setAbpConfig = (config: any) => {
    abpConfig.value = config
  }
  
  const setCurrentUser = (user: any) => {
    currentUser.value = user
  }
  
  const setMenu = (menuData: MenuDto) => {
    menu.value = menuData
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
    setAbpConfig,
    setCurrentUser,
    setMenu,
    clearAppData,
  }
})
