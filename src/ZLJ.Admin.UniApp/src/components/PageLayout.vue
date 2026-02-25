<template>
  <view class="page-wrapper">
    <view class="page-header" v-if="showHeader" :style="{ paddingTop: statusBarHeight + 'px' }">
      <view class="header-left" v-if="showMenuButton">
        <view class="menu-btn" @click="handleOpenMenu">
          <uni-icons type="bars" size="24" color="#666" />
        </view>
      </view>
      <view class="header-center" :class="{ 'header-center--full': !showMenuButton }">
        <text class="header-title">{{ title }}</text>
      </view>
      <view class="header-right">
        <slot name="headerRight"></slot>
      </view>
    </view>

    <view
      class="page-content"
      :class="{ 'page-content--no-header': !showHeader, 'page-content--no-bottom': !showBottomMenu }"
      :style="{ paddingTop: showHeader ? (statusBarHeight + 100) + 'px' : '30rpx' }"
    >
      <slot></slot>
    </view>

    <SideMenu :open="showSideMenu" @close="showSideMenu = false" />
    <BottomMenu v-if="showBottomMenu" />
  </view>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import SideMenu from '@/components/SideMenu.vue'
import BottomMenu from '@/components/BottomMenu.vue'

interface Props {
  /** 页面标题 */
  title?: string
  /** 是否显示头部 */
  showHeader?: boolean
  /** 是否显示菜单按钮 */
  showMenuButton?: boolean
  /** 是否显示底部菜单 */
  showBottomMenu?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  title: '',
  showHeader: true,
  showMenuButton: true,
  showBottomMenu: true,
})

const showSideMenu = ref(false)
const statusBarHeight = ref(0)

onMounted(() => {
  const systemInfo = uni.getSystemInfoSync()
  statusBarHeight.value = systemInfo.statusBarHeight || 0
})

const handleOpenMenu = () => {
  showSideMenu.value = true
}

const closeSideMenu = () => {
  showSideMenu.value = false
}

defineExpose({
  showSideMenu,
  closeSideMenu,
})
</script>

<style scoped>
.page-wrapper {
  min-height: 100vh;
  background: #f5f5f5;
}

.page-header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 30rpx;
  background: #ffffff;
  border-bottom: 1rpx solid #f0f0f0;
  z-index: 99;
}

.header-left,
.header-right {
  width: 80rpx;
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

.header-center {
  flex: 1;
  text-align: center;
}

.header-center--full {
  padding-left: 80rpx;
}

.header-title {
  font-size: 36rpx;
  font-weight: 600;
  color: #333;
}

.page-content {
  min-height: 100vh;
  background: #f5f5f5;
  padding: 30rpx;
  padding-bottom: 120rpx;
}

.page-content--no-header {
  padding-top: 30rpx;
}

.page-content--no-bottom {
  padding-bottom: 30rpx;
}
</style>
