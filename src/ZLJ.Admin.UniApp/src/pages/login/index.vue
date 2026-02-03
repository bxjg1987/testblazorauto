<template>
  <view class="container">
    <view class="login-box">
      <text class="title">用户登录</text>
      
      <view class="form">
        <view class="form-item">
          <text class="label">租户</text>
          <input 
            class="input" 
            v-model="form.tenancyName" 
            placeholder="请输入租户"
            type="text"
          />
        </view>
        
        <view class="form-item">
          <text class="label">用户名</text>
          <input 
            class="input" 
            v-model="form.userNameOrEmailAddress" 
            placeholder="请输入用户名"
            type="text"
          />
        </view>
        
        <view class="form-item">
          <text class="label">密码</text>
          <input 
            class="input" 
            v-model="form.password" 
            placeholder="请输入密码"
            type="password"
          />
        </view>
        
        <view class="form-item" v-if="showCaptcha">
          <text class="label">验证码</text>
          <view class="captcha-wrapper">
            <input 
              class="input captcha-input" 
              v-model="form.yzmValue" 
              placeholder="请输入验证码"
              type="text"
            />
            <image 
              class="captcha-image" 
              :src="captchaUrl" 
              @click="refreshCaptcha"
              mode="aspectFit"
            />
          </view>
        </view>
      </view>
      
      <button class="btn-login" @click="handleLogin" :disabled="loading">
        {{ loading ? '登录中...' : '登录' }}
      </button>
      
      <view class="actions">
        <text class="link">忘记密码?</text>
        <text class="link">注册账号</text>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useUserStore } from '@/stores/user'
import { authenticate } from '@/api/auth'
import { appInitializationService } from '@/services/app-initialization'
import { getCaptchaUrl } from '@/api/captcha'

const userStore = useUserStore()
const loading = ref(false)
const showCaptcha = ref(true)

const form = ref({
  tenancyName: 'default',
  userNameOrEmailAddress: '',
  password: '',
  rememberClient: true,
  yzmKey: '',
  yzmValue: '',
})

const captchaUrl = computed(() => {
  if (form.value.yzmKey) {
    return getCaptchaUrl(form.value.yzmKey)
  }
  return ''
})

onMounted(() => {
  refreshCaptcha()
})

const refreshCaptcha = () => {
  form.value.yzmKey = generateGuid()
  form.value.yzmValue = ''
}

const generateGuid = (): string => {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
    const r = Math.random() * 16 | 0
    const v = c === 'x' ? r : (r & 0x3 | 0x8)
    return v.toString(16)
  })
}

const handleLogin = async () => {
  if (!form.value.userNameOrEmailAddress) {
    uni.showToast({
      title: '请输入用户名',
      icon: 'none',
    })
    return
  }
  
  if (!form.value.password) {
    uni.showToast({
      title: '请输入密码',
      icon: 'none',
    })
    return
  }
  
  if (showCaptcha.value && !form.value.yzmValue) {
    uni.showToast({
      title: '请输入验证码',
      icon: 'none',
    })
    return
  }
  
  loading.value = true
  
  try {
    const result = await authenticate(form.value)
    
    userStore.setToken(result.accessToken)
    
    await appInitializationService.initializeAfterLogin()
    
    uni.showToast({
      title: '登录成功',
      icon: 'success',
    })
    
    setTimeout(() => {
      uni.reLaunch({
        url: '/pages/index/index',
      })
    }, 1000)
  } catch (error: any) {
    console.error('登录失败:', error)
    refreshCaptcha()
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.container {
  min-height: 100vh;
  background: #f5f5f5;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 40rpx;
}

.login-box {
  width: 100%;
  max-width: 600rpx;
  background: #ffffff;
  border-radius: 24rpx;
  padding: 60rpx 40rpx;
  box-shadow: 0 4rpx 20rpx rgba(0, 0, 0, 0.08);
}

.title {
  display: block;
  font-size: 48rpx;
  font-weight: bold;
  color: #333;
  text-align: center;
  margin-bottom: 60rpx;
}

.form {
  margin-bottom: 40rpx;
}

.form-item {
  margin-bottom: 30rpx;
}

.form-item .label {
  display: block;
  font-size: 28rpx;
  color: #333;
  margin-bottom: 12rpx;
}

.form-item .input {
  width: 100%;
  height: 88rpx;
  background: #f8f8f8;
  border-radius: 12rpx;
  padding: 0 24rpx;
  font-size: 28rpx;
  box-sizing: border-box;
}

.captcha-wrapper {
  display: flex;
  gap: 20rpx;
}

.captcha-input {
  flex: 1;
}

.captcha-image {
  width: 260rpx;
  height: 100rpx;
  border-radius: 12rpx;
  background: #f8f8f8;
}

.btn-login {
  width: 100%;
  height: 96rpx;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: #ffffff;
  font-size: 32rpx;
  font-weight: 600;
  border-radius: 48rpx;
  border: none;
  margin-bottom: 30rpx;
}

.btn-login:active {
  opacity: 0.9;
}

.btn-login:disabled {
  opacity: 0.6;
}

.actions {
  display: flex;
  justify-content: space-between;
}

.actions .link {
  font-size: 26rpx;
  color: #667eea;
}
</style>
