import { getUserConfiguration } from '@/api/config'
import { getCurrentLoginInformations } from '@/api/session'
import { useUserStore } from '@/store/user'
import { useAppStore } from '@/store/app'
import { setApiBaseUrl } from '@/utils/http'

/** 应用初始化服务 */
export class AppInitializationService {
  private static instance: AppInitializationService

  private constructor() {}

  static getInstance(): AppInitializationService {
    if (!AppInitializationService.instance) {
      AppInitializationService.instance = new AppInitializationService()
    }
    return AppInitializationService.instance
  }

  /** 应用启动时初始化 */
  async initializeOnLaunch(): Promise<void> {
    const userStore = useUserStore()
    const appStore = useAppStore()

    userStore.loadTokenFromStorage()

    const config = await getUserConfiguration()
    appStore.setAbpConfig(config)
    appStore.setMenu(config.nav.menus)

    if (userStore.isLoggedIn) {
      const currentUser = await getCurrentLoginInformations()
      appStore.setCurrentUser(currentUser)
      userStore.setUserInfo({
        id: currentUser.user.id,
        username: currentUser.user.userName || '',
        nickname: currentUser.user.name || currentUser.user.userName || '',
        email: currentUser.user.emailAddress,
        phone: currentUser.user.phoneNumber,
      })
    }
  }

  /** 登录后初始化 */
  async initializeAfterLogin(): Promise<void> {
    const userStore = useUserStore()
    const appStore = useAppStore()

    const config = await getUserConfiguration()
    appStore.setAbpConfig(config)
    appStore.setMenu(config.nav.menus)

    const currentUser = await getCurrentLoginInformations()
    appStore.setCurrentUser(currentUser)
    userStore.setUserInfo({
      id: currentUser.user.id,
      username: currentUser.user.userName || '',
      nickname: currentUser.user.name || currentUser.user.userName || '',
      email: currentUser.user.emailAddress,
      phone: currentUser.user.phoneNumber,
    })
  }

  /** 重新初始化（切换API地址时使用） */
  async reinitialize(apiUrl: string): Promise<void> {
    setApiBaseUrl(apiUrl)
    await this.initializeOnLaunch()
    uni.showToast({ title: '连接成功', icon: 'success' })
    setTimeout(() => {
      uni.reLaunch({ url: '/pages/index' })
    }, 1000)
  }
}

/** 应用初始化服务单例 */
export const appInitializationService = AppInitializationService.getInstance()
