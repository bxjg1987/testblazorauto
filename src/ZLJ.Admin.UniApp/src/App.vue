<template>
  <view></view>
</template>

<script setup lang="ts">
import { onLaunch, onShow, onHide } from '@dcloudio/uni-app'
import { getApiBaseUrl } from '@/utils/http'
import { appInitializationService } from '@/services/app-initialization'

onLaunch(async () => {
  const apiBaseUrl = getApiBaseUrl()

  if (!apiBaseUrl || apiBaseUrl === '/api') {
    uni.redirectTo({
      url: '/pages/init/index',
    })
    return
  }

  try {
    await appInitializationService.initializeOnLaunch()
  } catch (error: any) {
    if (error.name === 'UnauthorizedException' || error.statusCode === 401) {
      uni.redirectTo({
        url: '/pages/login/index',
      })
    } else {
      uni.redirectTo({
        url: '/pages/init/index',
      })
    }
  }
})

onShow(() => {})

onHide(() => {})
</script>

<style>
@import './styles/index.css';
</style>
