import { computed, ref } from 'vue'
import { defineStore } from 'pinia'

/** 用户信息接口 */
export interface UserInfo {
  /** 用户ID */
  id: number
  /** 用户名 */
  username: string
  /** 昵称 */
  nickname: string
  /** 头像地址 */
  avatar?: string
  /** 邮箱 */
  email?: string
  /** 手机号 */
  phone?: string
  /** 角色 */
  role?: string
}

/** 用户状态管理 Store */
export const useUserStore = defineStore('user', () => {
  const token = ref<string>('')
  const userInfo = ref<UserInfo | null>(null)

  const isLoggedIn = computed(() => !!token.value)
  const username = computed(() => userInfo.value?.username || '')
  const nickname = computed(() => userInfo.value?.nickname || '')

  /** 设置认证令牌 */
  const setToken = (newToken: string) => {
    token.value = newToken
    uni.setStorageSync('token', newToken)
  }

  /** 设置用户信息 */
  const setUserInfo = (info: UserInfo) => {
    userInfo.value = info
    uni.setStorageSync('userInfo', JSON.stringify(info))
  }

  /** 从本地存储恢复令牌和用户信息 */
  const loadTokenFromStorage = () => {
    const storedToken = uni.getStorageSync('token')
    if (storedToken) {
      token.value = storedToken as string
    }
    const storedUserInfo = uni.getStorageSync('userInfo')
    if (storedUserInfo) {
      try {
        userInfo.value = JSON.parse(storedUserInfo as string) as UserInfo
      } catch {
        userInfo.value = null
      }
    }
  }

  /** 清除用户数据 */
  const clearUserData = () => {
    token.value = ''
    userInfo.value = null
    uni.removeStorageSync('token')
    uni.removeStorageSync('userInfo')
  }

  /** 登出操作 */
  const logout = (returnUrl?: string) => {
    clearUserData()
    const url = returnUrl
      ? `/pages/login?returnUrl=${encodeURIComponent(returnUrl)}`
      : '/pages/login'
    uni.reLaunch({ url })
  }

  return {
    token,
    userInfo,
    isLoggedIn,
    username,
    nickname,
    setToken,
    setUserInfo,
    loadTokenFromStorage,
    clearUserData,
    logout,
  }
})
