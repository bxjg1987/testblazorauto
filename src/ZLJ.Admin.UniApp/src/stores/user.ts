import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export interface UserInfo {
  id: number
  username: string
  nickname: string
  avatar?: string
  email?: string
  phone?: string
  role?: string
}

export const useUserStore = defineStore('user', () => {
  const token = ref<string>('')
  const userInfo = ref<UserInfo | null>(null)
  
  const isLoggedIn = computed(() => !!token.value)
  const username = computed(() => userInfo.value?.username || '')
  const nickname = computed(() => userInfo.value?.nickname || '')
  
  const setToken = (newToken: string) => {
    token.value = newToken
    uni.setStorageSync('token', newToken)
  }
  
  const setUserInfo = (info: UserInfo) => {
    userInfo.value = info
    uni.setStorageSync('userInfo', JSON.stringify(info))
  }
  
  const loadTokenFromStorage = () => {
    const storedToken = uni.getStorageSync('token')
    if (storedToken) {
      token.value = storedToken
    }
    
    const storedUserInfo = uni.getStorageSync('userInfo')
    if (storedUserInfo) {
      try {
        userInfo.value = JSON.parse(storedUserInfo)
      } catch (e) {
        console.error('解析用户信息失败', e)
      }
    }
  }
  
  const clearUserData = () => {
    token.value = ''
    userInfo.value = null
    uni.removeStorageSync('token')
    uni.removeStorageSync('userInfo')
  }
  
  const logout = () => {
    clearUserData()
    uni.reLaunch({
      url: '/pages/login/index',
    })
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
