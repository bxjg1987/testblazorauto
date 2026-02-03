<template>
  <view>
    <uni-forms>
      <uni-forms-item label="API 地址" required>
        <uni-easyinput 
          v-model="apiUrl" 
          placeholder="请输入后端 API 地址"
        />
      </uni-forms-item>
    </uni-forms>
    
    <uni-card title="当前配置" :is-shadow="false">
      <text>{{ currentUrl || '未配置' }}</text>
    </uni-card>
    
    <view style="margin-top: 20px;">
      <view class="uni-row" style="display: flex; gap: 10px;">
        <view class="uni-col" style="flex: 1;">
          <button class="uni-button uni-button--default" @click="handleTest" :disabled="testing">
            {{ testing ? '测试中...' : '测试连接' }}
          </button>
        </view>
        <view class="uni-col" style="flex: 1;">
          <button class="uni-button uni-button--primary" @click="handleSave" :disabled="saving">
            {{ saving ? '保存中...' : '保存配置' }}
          </button>
        </view>
      </view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { getUserConfiguration } from '@/api/config'
import { setApiBaseUrl } from '@/utils/http'

const emit = defineEmits<{
  (e: 'save', url: string): void
  (e: 'cancel'): void
}>()

const apiUrl = ref('')
const currentUrl = ref('')
const testing = ref(false)
const saving = ref(false)

onMounted(() => {
  loadConfig()
})

const loadConfig = () => {
  const savedUrl = uni.getStorageSync('apiBaseUrl')
  if (savedUrl) {
    currentUrl.value = savedUrl
    apiUrl.value = savedUrl
  } else {
    const defaultUrl = import.meta.env.VITE_API_BASE_URL || '/api'
    currentUrl.value = defaultUrl
    apiUrl.value = defaultUrl
  }
}

const handleTest = async () => {
  if (!apiUrl.value.trim()) {
    uni.showToast({
      title: '请输入 API 地址',
      icon: 'none',
    })
    return
  }
  
  testing.value = true
  
  try {
    const tempUrl = apiUrl.value.trim()
    setApiBaseUrl(tempUrl)
    
    await getUserConfiguration()
    
    uni.showToast({
      title: '连接成功',
      icon: 'success',
    })
  } catch (error: any) {
    console.error('连接测试失败:', error)
    uni.showToast({
      title: error.message || '连接失败',
      icon: 'none',
    })
  } finally {
    testing.value = false
  }
}

const handleSave = async () => {
  if (!apiUrl.value.trim()) {
    uni.showToast({
      title: '请输入 API 地址',
      icon: 'none',
    })
    return
  }
  
  saving.value = true
  
  try {
    const tempUrl = apiUrl.value.trim()
    setApiBaseUrl(tempUrl)
    
    await getUserConfiguration()
    
    uni.setStorageSync('apiBaseUrl', tempUrl)
    currentUrl.value = tempUrl
    
    uni.showToast({
      title: '保存成功',
      icon: 'success',
    })
    
    setTimeout(() => {
      uni.reLaunch({
        url: '/pages/index/index',
      })
    }, 1000)
    
    emit('save', tempUrl)
  } catch (error: any) {
    console.error('保存配置失败:', error)
    uni.showToast({
      title: error.message || '连接失败',
      icon: 'none',
    })
  } finally {
    saving.value = false
  }
}
</script>

<style scoped>
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
  background-color: #2979ff;
  color: #ffffff;
}

.uni-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
