<template>
  <view class="bottom-menu">
    <view
      class="menu-item"
      :class="{ active: isMenuItemActive(item, currentPath) }"
      v-for="(item, index) in menuItems"
      :key="index"
      @click="handleMenuClick(item)"
    >
      <uni-icons
        :type="item.customData?.mobileIcon || 'home'"
        :size="24"
        :color="isMenuItemActive(item, currentPath) ? '#1890ff' : '#666'"
        class="menu-icon"
      />
      <view class="menu-text">{{ item.displayName }}</view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { useAppStore } from '@/stores/app'
import { filterBottomMenus, normalizeMenuUrl, isMenuItemActive as checkMenuItemActive } from '@/utils/menu'
import type { MenuItemDto } from '@/types/menu'

const appStore = useAppStore()
const currentPath = ref('')

onMounted(() => {
  const pages = getCurrentPages()
  if (pages.length > 0) {
    const currentPage = pages[pages.length - 1]
    currentPath.value = '/' + currentPage.route
  }
})

const isMenuItemActive = (item: MenuItemDto, path: string): boolean => {
  return checkMenuItemActive(item, path)
}

const menuItems = computed(() => {
  const menu = appStore.menu

  let homeMenu: MenuItemDto | null = null
  if (menu && menu.MainMenu?.items) {
    for (const item of menu.MainMenu.items) {
      if (item.name === 'adminBlazor_home' || item.url === '/main') {
        homeMenu = {
          ...item,
          displayName: '首页',
          customData: { ...item.customData, mobileIcon: 'home' }
        }
        break
      }
    }
  }

  const dynamicMenus = menu && Object.keys(menu).length > 0 ? filterBottomMenus(menu) : []

  const fixedMenus: MenuItemDto[] = [
    homeMenu || {
      displayName: '首页',
      url: '/pages/index/index',
      customData: { mobileIcon: 'home' }
    },
    ...dynamicMenus,
    {
      displayName: '我的',
      url: '/pages/profile/index',
      customData: { mobileIcon: 'person' }
    }
  ]

  return fixedMenus
})

const handleMenuClick = (item: MenuItemDto) => {
  if (!item.url) {
    return
  }

  const url = normalizeMenuUrl(item.url)

  uni.redirectTo({
    url,
    animationType: 'fade',
    animationDuration: 150,
    fail: () => {
      uni.showToast({
        title: '页面不存在',
        icon: 'none'
      })
    }
  })
}
</script>

<style scoped>
.bottom-menu {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  background: #ffffff;
  border-top: 1rpx solid #f0f0f0;
  display: flex;
  padding-bottom: env(safe-area-inset-bottom);
  z-index: 999;
  will-change: transform;
  transform: translateZ(0);
  -webkit-transform: translateZ(0);
  backface-visibility: hidden;
  -webkit-backface-visibility: hidden;
}

.menu-item {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 20rpx 0;
  position: relative;
  transition: background-color 0.15s ease;
}

.menu-item:active {
  background: #f8f8f8;
}

.menu-icon {
  font-size: 48rpx;
  margin-bottom: 8rpx;
  transition: color 0.15s ease;
}

.menu-text {
  font-size: 24rpx;
  color: #666;
  transition: color 0.15s ease;
}

.menu-item.active .menu-text {
  color: #1890ff;
  font-weight: 600;
}
</style>
