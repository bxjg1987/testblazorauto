<template>
  <PageLayout title="我的" :show-header="false" :show-menu-button="false">
    <view class="container">
      <view class="header">
        <view class="avatar-box">
          <image class="avatar" src="/static/default-avatar.png" mode="aspectFill" />
        </view>
        <text class="username">{{ nickname || '未登录' }}</text>
        <text class="role">{{ roleText }}</text>
      </view>
      
      <view class="menu-list">
        <view class="menu-item" v-for="(item, index) in menuList" :key="index" @click="handleMenuClick(item)">
          <view class="menu-left">
            <text class="menu-icon">{{ item.icon }}</text>
            <text class="menu-text">{{ item.name }}</text>
          </view>
          <text class="menu-arrow">›</text>
        </view>
      </view>
      
      <button class="btn-logout" @click="handleLogout">退出登录</button>
      
      <Modal 
        v-model:visible="showApiConfigModal" 
        title="API 配置"
        @confirm="handleApiConfigConfirm"
        @cancel="handleApiConfigCancel"
      >
        <ApiConfig @save="handleApiConfigSave" />
      </Modal>
    </view>
  </PageLayout>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { onShow } from '@dcloudio/uni-app'
import Modal from '@/components/Modal.vue'
import ApiConfig from '@/components/ApiConfig.vue'
import PageLayout from '@/components/PageLayout.vue'
import { useUserStore } from '@/stores/user'

interface MenuItem {
  name: string
  icon: string
  path?: string
  action?: string
}

const userStore = useUserStore()
const showApiConfigModal = ref(false)

const menuList = ref<MenuItem[]>([
  { name: '个人资料', icon: '👤', path: '/pages/profile/edit' },
  { name: '修改密码', icon: '🔒', path: '/pages/profile/password' },
  { name: 'API 配置', icon: '🔗', action: 'apiConfig' },
  { name: '系统设置', icon: '⚙️', path: '/pages/settings/index' },
  { name: '关于我们', icon: 'ℹ️', path: '/pages/about/index' },
])

const nickname = computed(() => userStore.nickname)
const roleText = computed(() => userStore.isLoggedIn ? '超级管理员' : '未登录')

onShow(() => {
  if (!userStore.isLoggedIn) {
    uni.reLaunch({
      url: '/pages/login/index',
    })
  }
})

const handleMenuClick = (item: MenuItem) => {
  if (item.action === 'apiConfig') {
    showApiConfigModal.value = true
    return
  }
  
  if (item.path) {
    uni.navigateTo({
      url: item.path,
    })
  }
}

const handleApiConfigConfirm = () => {
  showApiConfigModal.value = false
}

const handleApiConfigCancel = () => {
  showApiConfigModal.value = false
}

const handleApiConfigSave = (url: string) => {
  setTimeout(() => {
    showApiConfigModal.value = false
  }, 500)
}

const handleLogout = () => {
  uni.showModal({
    title: '提示',
    content: '确定要退出登录吗？',
    success: (res) => {
      if (res.confirm) {
        userStore.logout()
        uni.showToast({
          title: '已退出登录',
          icon: 'success',
        })
      }
    },
  })
}
</script>

<style scoped>
.container {
  min-height: 100vh;
  background: #f5f5f5;
  padding-bottom: 120rpx;
}

.header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 80rpx 40rpx;
  text-align: center;
}

.avatar-box {
  width: 160rpx;
  height: 160rpx;
  margin: 0 auto 30rpx;
  border-radius: 50%;
  overflow: hidden;
  border: 6rpx solid rgba(255, 255, 255, 0.3);
}

.avatar {
  width: 100%;
  height: 100%;
}

.username {
  display: block;
  font-size: 40rpx;
  font-weight: 600;
  color: #ffffff;
  margin-bottom: 12rpx;
}

.role {
  display: block;
  font-size: 26rpx;
  color: rgba(255, 255, 255, 0.8);
}

.menu-list {
  background: #ffffff;
  margin: 30rpx;
  border-radius: 16rpx;
  overflow: hidden;
}

.menu-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 30rpx;
  border-bottom: 1rpx solid #f0f0f0;
}

.menu-item:last-child {
  border-bottom: none;
}

.menu-item:active {
  background: #f8f8f8;
}

.menu-left {
  display: flex;
  align-items: center;
}

.menu-icon {
  font-size: 36rpx;
  margin-right: 20rpx;
}

.menu-text {
  font-size: 30rpx;
  color: #333;
  flex: 1;
}

.menu-arrow {
  font-size: 36rpx;
  color: #999;
}

.btn-logout {
  margin: 60rpx 30rpx;
  height: 96rpx;
  line-height: 96rpx;
  background: #ffffff;
  color: #ff4d4f;
  font-size: 32rpx;
  font-weight: 600;
  border-radius: 48rpx;
  border: none;
}

.btn-logout:active {
  background: #f8f8f8;
}
</style>
