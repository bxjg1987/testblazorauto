<script setup lang="ts">
/** 页面路由配置 */
definePage({
  meta: {
    layout: 'default',
    showNavbar: false,
    showTabbar: false,
    requiresAuth: false,
    title: '初始化配置',
  },
})

import { appInitializationService } from '@/services/app-initialization'
import { getApiBaseUrl } from '@/utils/http'

/** API 地址配置页面 */

/** 默认后端 API 地址 */
const DEFAULT_API_URL = 'http://localhost:21021'

/** API 地址 */
const apiUrl = ref(getApiBaseUrl() === '/api' ? DEFAULT_API_URL : getApiBaseUrl())

/** 是否正在连接 */
const connecting = ref(false)

/** 测试连接并初始化 */
const handleConnect = async () => {
  if (!apiUrl.value) {
    uni.showToast({ title: '请输入API地址', icon: 'none' })
    return
  }

  connecting.value = true
  try {
    await appInitializationService.reinitialize(apiUrl.value)
  } catch {
    uni.showToast({ title: '连接失败，请检查地址', icon: 'none' })
  } finally {
    connecting.value = false
  }
}
</script>

<template>
  <view class="p-6">
    <view class="text-center py-12">
      <text class="text-2xl font-bold">ZLJ Admin</text>
      <view class="mt-2">
        <text class="text-gray-500">请配置后端API地址</text>
      </view>
    </view>

    <view class="mt-8">
      <wd-input v-model="apiUrl" label="API地址" placeholder="例如: http://192.168.1.100:44315" clearable />
      <view class="mt-8">
        <wd-button type="primary" block :loading="connecting" @click="handleConnect">连接</wd-button>
      </view>
    </view>
  </view>
</template>
