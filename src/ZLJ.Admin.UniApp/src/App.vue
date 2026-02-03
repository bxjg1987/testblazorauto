<template>
  <view></view>
</template>

<script setup lang="ts">
import { onLaunch, onShow, onHide } from '@dcloudio/uni-app'
import { getApiBaseUrl } from '@/utils/http'
import { appInitializationService } from '@/services/app-initialization'

onLaunch(async () => {
  console.log('App Launch')
  
  const apiBaseUrl = getApiBaseUrl()
  console.log('Current API base URL:', apiBaseUrl)
  
  if (!apiBaseUrl || apiBaseUrl === '/api') {
    console.log('API not configured, redirecting to init page')
    uni.redirectTo({
      url: '/pages/init/index',
    })
    return
  }
  
  try {
    await appInitializationService.initializeOnLaunch()
    console.log('App initialization completed successfully')
  } catch (error: any) {
    console.error('App initialization failed:', error)
    
    if (error.name === 'UnauthorizedException' || error.statusCode === 401) {
      console.log('未授权，跳转到登录页')
      uni.redirectTo({
        url: '/pages/login/index',
      })
    } else {
      console.log('初始化失败，跳转到配置页')
      uni.redirectTo({
        url: '/pages/init/index',
      })
    }
  }
})

onShow(() => {
  console.log('App Show')
})

onHide(() => {
  console.log('App Hide')
})
</script>

<style>
@import './styles/index.css';
</style>
