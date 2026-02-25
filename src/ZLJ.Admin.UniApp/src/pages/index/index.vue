<template>
  <PageLayout title="首页" :show-header="false" ref="pageLayoutRef">
    <view class="container">
      <view class="menu-btn-wrapper" :style="{ top: statusBarHeight + 'px' }">
        <view class="menu-btn" @click="handleOpenMenu">
          <uni-icons type="bars" size="24" color="#666" />
        </view>
      </view>

      <view class="header">
        <view class="header-center">
          <text class="title">小管印</text>
          <text class="subtitle">管理系统</text>
        </view>
      </view>

      <view class="content">
        <view class="card">
          <text class="card-title">欢迎使用</text>
          <text class="card-desc">这是一个基于 UniApp + Vue3 + TypeScript 开发的多端应用</text>
        </view>

        <view class="feature-list">
          <view class="feature-item" v-for="(item, index) in features" :key="index">
            <text class="feature-icon">{{ item.icon }}</text>
            <text class="feature-text">{{ item.name }}</text>
          </view>
        </view>
      </view>
    </view>
  </PageLayout>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { onShow } from '@dcloudio/uni-app'
import PageLayout from '@/components/PageLayout.vue'
import { useUserStore } from '@/stores/user'

const userStore = useUserStore()
const pageLayoutRef = ref()
const statusBarHeight = ref(0)

onMounted(() => {
  const systemInfo = uni.getSystemInfoSync()
  statusBarHeight.value = systemInfo.statusBarHeight || 0
})

const features = ref([
  { icon: '📱', name: 'Android App' },
  { icon: '🌐', name: 'H5 网页' },
  { icon: '🔧', name: 'iOS App' },
  { icon: '💬', name: '微信小程序' },
])

const handleOpenMenu = () => {
  if (pageLayoutRef.value) {
    pageLayoutRef.value.showSideMenu = true
  }
}

onShow(() => {
  if (!userStore.isLoggedIn) {
    uni.reLaunch({
      url: '/pages/login/index',
    })
  }
})
</script>

<style scoped>
.container {
  min-height: 100vh;
  padding: 40rpx;
  padding-bottom: 120rpx;
  position: relative;
}

.menu-btn-wrapper {
  position: fixed;
  left: 20rpx;
  padding-top: 0;
  z-index: 100;
}

.menu-btn {
  width: 60rpx;
  height: 60rpx;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  background: #f5f5f5;
}

.header {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 60rpx 0;
}

.header-center {
  flex: 1;
  text-align: center;
}

.header .title {
  display: block;
  font-size: 56rpx;
  font-weight: bold;
  color: #1890ff;
  margin-bottom: 16rpx;
}

.header .subtitle {
  display: block;
  font-size: 28rpx;
  color: #666;
}

.content {
  padding: 40rpx 0;
}

.card {
  background: rgba(255, 255, 255, 0.95);
  border-radius: 24rpx;
  padding: 60rpx 40rpx;
  margin-bottom: 40rpx;
  box-shadow: 0 8rpx 32rpx rgba(0, 0, 0, 0.1);
}

.card-title {
  display: block;
  font-size: 40rpx;
  font-weight: 600;
  color: #333;
  margin-bottom: 16rpx;
}

.card-desc {
  display: block;
  font-size: 28rpx;
  color: #666;
  line-height: 1.6;
}

.feature-list {
  display: flex;
  flex-wrap: wrap;
  gap: 20rpx;
}

.feature-item {
  flex: 1;
  min-width: 140rpx;
  background: rgba(255, 255, 255, 0.9);
  border-radius: 16rpx;
  padding: 30rpx 20rpx;
  text-align: center;
}

.feature-icon {
  display: block;
  font-size: 48rpx;
  margin-bottom: 12rpx;
}

.feature-text {
  display: block;
  font-size: 24rpx;
  color: #333;
}
</style>
