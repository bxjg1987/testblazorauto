import { getUserConfiguration } from '@/api/config'
import { getCurrentLoginInformations } from '@/api/session'
import type { MenuDto } from '@/types/menu'
import { filterMainMenus } from '@/utils/menu'
import { useUserStore } from '@/stores/user'
import { useAppStore } from '@/stores/app'
import { setApiBaseUrl } from '@/utils/http'

export class AppInitializationService {
  private static instance: AppInitializationService
  
  private constructor() {}  
  static getInstance(): AppInitializationService {
    if (!AppInitializationService.instance) {
      AppInitializationService.instance = new AppInitializationService()
    }
    return AppInitializationService.instance
  }
  
  async initializeAfterLogin(): Promise<void> {
    const userStore = useUserStore()
    const appStore = useAppStore()
    
    try {
      console.log('[AppInit] 开始初始化应用配置（登录后）')
      
      const config = await getUserConfiguration()
      console.log('[AppInit] ABP 配置获取成功:', config)
      appStore.setAbpConfig(config)
      
      const menu = config.nav?.menus as MenuDto
      console.log('[AppInit] 菜单数据:', JSON.stringify(menu, null, 2))
      if (menu) {
        appStore.setMenu(menu)
        console.log('[AppInit] 菜单已存储到 Store')
      } else {
        console.log('[AppInit] 菜单数据为空')
      }
      
      const currentUser = await getCurrentLoginInformations()
      console.log('[AppInit] 当前用户信息:', currentUser)
      appStore.setCurrentUser(currentUser)
      
      if (currentUser.user) {
        userStore.setUserInfo({
          id: currentUser.user.id,
          username: currentUser.user.userName || '',
          nickname: currentUser.user.name || currentUser.user.userName || '',
          email: currentUser.user.emailAddress,
          phone: currentUser.user.phoneNumber,
        })
        console.log('[AppInit] 用户信息已存储到 Store')
      }
      
      console.log('[AppInit] 应用配置初始化完成')
    } catch (error) {
      console.error('[AppInit] 初始化应用配置失败:', error)
      throw error
    }
  }
  
  async initializeOnLaunch(): Promise<void> {
    const userStore = useUserStore()
    const appStore = useAppStore()
    
    try {
      console.log('[AppInit] 开始初始化应用配置（启动时）')
      
      userStore.loadTokenFromStorage()
      console.log('[AppInit] Token 状态:', userStore.isLoggedIn ? '已登录' : '未登录')
      
      const config = await getUserConfiguration()
      console.log('[AppInit] ABP 配置获取成功:', config)
      appStore.setAbpConfig(config)
      
      const menu = config.nav?.menus as MenuDto
      console.log('[AppInit] 菜单数据:', JSON.stringify(menu, null, 2))
      if (menu) {
        appStore.setMenu(menu)
        console.log('[AppInit] 菜单已存储到 Store')
      } else {
        console.log('[AppInit] 菜单数据为空')
      }
      
      if (userStore.isLoggedIn) {
        const currentUser = await getCurrentLoginInformations()
        console.log('[AppInit] 当前用户信息:', currentUser)
        appStore.setCurrentUser(currentUser)
        
        if (currentUser.user) {
          userStore.setUserInfo({
            id: currentUser.user.id,
            username: currentUser.user.userName || '',
            nickname: currentUser.user.name || currentUser.user.userName || '',
            email: currentUser.user.emailAddress,
            phone: currentUser.user.phoneNumber,
          })
          console.log('[AppInit] 用户信息已存储到 Store')
        }
      } else {
        console.log('[AppInit] 未登录，跳过用户信息获取')
      }
      
      console.log('[AppInit] 应用配置初始化完成')
    } catch (error) {
      console.error('[AppInit] 初始化应用配置失败:', error)
      throw error
    }
  }
  
  async reinitialize(apiUrl: string): Promise<void> {
    try {
      console.log('[AppInit] 开始重新初始化应用配置')
      setApiBaseUrl(apiUrl)
      
      await this.initializeOnLaunch()
      
      uni.showToast({
        title: '连接成功',
        icon: 'success',
      })
      
      setTimeout(() => {
        uni.reLaunch({
          url: '/pages/index/index',
        })
      }, 1000)
    } catch (error) {
      console.error('[AppInit] 重新初始化失败:', error)
      uni.showToast({
        title: '连接失败',
        icon: 'none',
      })
      throw error
    }
  }
}

export const appInitializationService = AppInitializationService.getInstance()
