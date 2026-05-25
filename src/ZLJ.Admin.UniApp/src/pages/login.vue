<script setup lang="ts">
/** 页面路由配置 */
definePage({
  meta: {
    layout: 'default',
    showNavbar: false,
    showTabbar: false,
    requiresAuth: false,
    title: '登录',
  },
})

import { getCaptchaUrl } from '@/api/captcha'

/** 登录页面 */

const { login, handleLoginRedirect } = useAuth()

/** 表单数据 */
const formData = reactive({
  tenancyName: '',
  userNameOrEmailAddress: '',
  password: '',
  yzmKey: '',
  yzmValue: '',
})

/** 是否正在登录 */
const loading = ref(false)

/** 验证码图片地址 */
const captchaUrl = ref('')

/** 刷新验证码 */
const refreshCaptcha = () => {
  formData.yzmKey = `captcha_${Date.now()}`
  captchaUrl.value = getCaptchaUrl(formData.yzmKey)
}

/** 处理登录 */
const handleLogin = async () => {
  if (!formData.userNameOrEmailAddress) {
    uni.showToast({ title: '请输入用户名', icon: 'none' })
    return
  }
  if (!formData.password) {
    uni.showToast({ title: '请输入密码', icon: 'none' })
    return
  }

  loading.value = true
  try {
    await login(formData.userNameOrEmailAddress, formData.password, {
      tenancyName: formData.tenancyName || undefined,
      yzmKey: formData.yzmKey || undefined,
      yzmValue: formData.yzmValue || undefined,
    })
    handleLoginRedirect()
  } catch {
    refreshCaptcha()
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  refreshCaptcha()
})
</script>

<template>
  <view class="login-page p-6">
    <view class="text-center mt-20 mb-10">
      <text class="text-2xl font-bold">ZLJ Admin</text>
    </view>

    <view class="login-form">
      <wd-input v-model="formData.tenancyName" label="租户" placeholder="可选" clearable />
      <wd-input v-model="formData.userNameOrEmailAddress" label="用户名" placeholder="请输入用户名" clearable required />
      <wd-input v-model="formData.password" label="密码" placeholder="请输入密码" type="password" show-password clearable required />

      <view class="flex items-center mt-4" v-if="captchaUrl">
        <wd-input v-model="formData.yzmValue" label="验证码" placeholder="请输入验证码" clearable class="flex-1" />
        <image :src="captchaUrl" class="w-24 h-10 ml-2" @click="refreshCaptcha" mode="aspectFit" />
      </view>

      <view class="mt-8">
        <wd-button type="primary" block :loading="loading" @click="handleLogin">登录</wd-button>
      </view>
    </view>
  </view>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  background: var(--wot-filled-content);
}
</style>
