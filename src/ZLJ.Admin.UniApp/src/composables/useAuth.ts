import { authenticate } from '@/api/auth'
import { useUserStore } from '@/store/user'
import { useAppStore } from '@/store/app'
import { appInitializationService } from '@/services/app-initialization'

/** 认证相关组合式函数 */
export function useAuth() {
  const userStore = useUserStore()
  const appStore = useAppStore()

  /** 用户登录 */
  const login = async (userNameOrEmailAddress: string, password: string, options?: {
    tenancyName?: string
    yzmKey?: string
    yzmValue?: string
  }) => {
    const result = await authenticate({
      userNameOrEmailAddress,
      password,
      tenancyName: options?.tenancyName,
      yzmKey: options?.yzmKey,
      yzmValue: options?.yzmValue,
    })

    userStore.setToken(result.accessToken)

    await appInitializationService.initializeAfterLogin()

    return result
  }

  /** 用户登出 */
  const logout = (returnUrl?: string) => {
    appStore.clearAppData()
    userStore.logout(returnUrl)
  }

  /** 处理登录后跳转 */
  const handleLoginRedirect = () => {
    const pages = getCurrentPages()
    const currentPage = pages[pages.length - 1]
    const url = currentPage?.$page?.options?.returnUrl
    if (url) {
      uni.reLaunch({ url: decodeURIComponent(url) })
    } else {
      uni.reLaunch({ url: '/pages/index' })
    }
  }

  return { login, logout, handleLoginRedirect }
}
