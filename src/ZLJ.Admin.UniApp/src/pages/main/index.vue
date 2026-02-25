<template>
  <PageLayout title="后台管理" :show-header="false" ref="pageLayoutRef">
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
        <view class="welcome-card">
          <text class="welcome-title">欢迎回来，{{ nickname || '管理员' }}</text>
          <text class="welcome-desc">您有 {{ pendingCount }} 条待处理事项</text>
        </view>

        <view class="stats-grid" v-if="stats.length > 0">
          <view class="stat-item" v-for="(item, index) in stats" :key="index" @click="handleStatClick(item)">
            <text class="stat-value">{{ item.value }}</text>
            <text class="stat-label">{{ item.label }}</text>
          </view>
        </view>

        <view class="quick-actions">
          <text class="section-title">快捷操作</text>
          <view class="action-grid">
            <view class="action-item" v-for="(item, index) in quickActions" :key="index" @click="handleActionClick(item)">
              <view class="action-icon" :style="{ background: item.color }">
                <text>{{ item.icon }}</text>
              </view>
              <text class="action-label">{{ item.label }}</text>
            </view>
          </view>
        </view>
      </view>
    </view>
  </PageLayout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { onShow } from '@dcloudio/uni-app'
import PageLayout from '@/components/PageLayout.vue'
import { useUserStore } from '@/stores/user'

interface StatItem {
  value: number
  label: string
  path?: string
}

interface ActionItem {
  icon: string
  label: string
  path?: string
  color: string
}

const userStore = useUserStore()
const pageLayoutRef = ref()
const statusBarHeight = ref(0)
const pendingCount = ref(0)

const nickname = computed(() => userStore.nickname)

const stats = ref<StatItem[]>([
  { value: 12, label: '待办工单', path: '/pages/WorkOrderInfo/index' },
  { value: 5, label: '待审租赁', path: '/pages/RentOrderShenqing/index' },
  { value: 8, label: '待入库', path: '/pages/ruku/index' },
  { value: 3, label: '待出库', path: '/pages/chuku/index' },
])

const quickActions = ref<ActionItem[]>([
  { icon: '📋', label: '创建工单', path: '/pages/WorkOrderInfo/index', color: '#1890ff' },
  { icon: '📝', label: '租赁申请', path: '/pages/RentOrderShenqing/index', color: '#52c41a' },
  { icon: '📦', label: '入库', path: '/pages/ruku/index', color: '#faad14' },
  { icon: '🚚', label: '出库', path: '/pages/chuku/index', color: '#f5222d' },
  { icon: '🔄', label: '调拨', path: '/pages/diaobo/index', color: '#722ed1' },
  { icon: '📊', label: '库存', path: '/pages/inventory/index', color: '#13c2c2' },
])

onMounted(() => {
  const systemInfo = uni.getSystemInfoSync()
  statusBarHeight.value = systemInfo.statusBarHeight || 0
})

const handleOpenMenu = () => {
  if (pageLayoutRef.value) {
    pageLayoutRef.value.showSideMenu = true
  }
}

const handleStatClick = (item: StatItem) => {
  if (item.path) {
    uni.navigateTo({ url: item.path })
  }
}

const handleActionClick = (item: ActionItem) => {
  if (item.path) {
    uni.navigateTo({ url: item.path })
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

.welcome-card {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 24rpx;
  padding: 40rpx;
  margin-bottom: 30rpx;
}

.welcome-title {
  display: block;
  font-size: 36rpx;
  font-weight: 600;
  color: #ffffff;
  margin-bottom: 12rpx;
}

.welcome-desc {
  display: block;
  font-size: 26rpx;
  color: rgba(255, 255, 255, 0.8);
}

.stats-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 20rpx;
  margin-bottom: 40rpx;
}

.stat-item {
  flex: 1;
  min-width: 45%;
  background: #ffffff;
  border-radius: 16rpx;
  padding: 30rpx;
  text-align: center;
}

.stat-value {
  display: block;
  font-size: 48rpx;
  font-weight: bold;
  color: #1890ff;
  margin-bottom: 8rpx;
}

.stat-label {
  display: block;
  font-size: 24rpx;
  color: #666;
}

.quick-actions {
  background: #ffffff;
  border-radius: 16rpx;
  padding: 30rpx;
}

.section-title {
  display: block;
  font-size: 32rpx;
  font-weight: 600;
  color: #333;
  margin-bottom: 24rpx;
}

.action-grid {
  display: flex;
  flex-wrap: wrap;
  gap: 20rpx;
}

.action-item {
  width: 25%;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.action-icon {
  width: 80rpx;
  height: 80rpx;
  border-radius: 20rpx;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 12rpx;
}

.action-icon text {
  font-size: 36rpx;
}

.action-label {
  font-size: 24rpx;
  color: #333;
}
</style>
