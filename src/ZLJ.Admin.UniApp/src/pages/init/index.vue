<template>
  <view class="init-container">
    <view class="init-content">
      <text class="init-title">系统初始化</text>
      <text class="init-desc">请配置后端 API 地址</text>

      <view class="env-buttons">
        <button class="uni-button uni-button--default" size="mini" @click="setDevEnv">开发环境</button>
        <button class="uni-button uni-button--primary" size="mini" @click="setProdEnv">生产环境</button>
      </view>

      <view class="init-form">
        <uni-forms>
          <uni-forms-item label="API 地址" required>
            <uni-easyinput
              v-model="apiUrl"
              placeholder="请输入后端 API 地址"
            />
          </uni-forms-item>
        </uni-forms>

        <view class="button-group">
          <button class="uni-button uni-button--primary" @click="handleConnect" :disabled="connecting">
            {{ connecting ? '连接中...' : '连接' }}
          </button>
        </view>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { appInitializationService } from '@/services/app-initialization'

const apiUrl = ref('')
const connecting = ref(false)

const setDevEnv = () => {
  apiUrl.value = 'http://192.168.2.5:21021'
}

const setProdEnv = () => {
  apiUrl.value = 'https://fyjapi.dyfy365.com:20001'
}

const handleConnect = async () => {
  if (!apiUrl.value.trim()) {
    uni.showToast({
      title: '请输入 API 地址',
      icon: 'none',
    })
    return
  }

  connecting.value = true

  await appInitializationService.reinitialize(apiUrl.value.trim())

  connecting.value = false
}
</script>

<style scoped>
.init-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #1890ff 0%, #096dd9 100%);
}

.init-content {
  width: 600rpx;
  max-width: 90%;
  background: #ffffff;
  border-radius: 24rpx;
  padding: 60rpx 40rpx;
}

.init-title {
  display: block;
  font-size: 40rpx;
  font-weight: 600;
  color: #333;
  text-align: center;
  margin-bottom: 16rpx;
}

.init-desc {
  display: block;
  font-size: 28rpx;
  color: #666;
  text-align: center;
  margin-bottom: 40rpx;
}

.env-buttons {
  display: flex;
  gap: 20rpx;
  justify-content: center;
  margin-bottom: 40rpx;
}

.init-form {
  margin-top: 40rpx;
}

.button-group {
  margin-top: 40rpx;
}

.uni-button {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  height: 88rpx;
  font-size: 32rpx;
  border-radius: 8rpx;
  border: none;
  padding: 0 32rpx;
  box-sizing: border-box;
  cursor: pointer;
  transition: all 0.3s;
}

.uni-button:active {
  opacity: 0.8;
}

.uni-button--default {
  background-color: #ffffff;
  color: #333;
  border: 1rpx solid #dcdfe6;
}

.uni-button--primary {
  background-color: #1890ff;
  color: #ffffff;
}

.uni-button[size="mini"] {
  height: 60rpx;
  font-size: 24rpx;
  padding: 0 24rpx;
  width: auto;
}
</style>
