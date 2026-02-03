<template>
  <view class="bottom-menu">
    <view 
      class="menu-item" 
      :class="{ active: isActive(item) }"
      v-for="(item, index) in menuItems" 
      :key="index"
      @click="handleMenuClick(item)"
    >
      <uni-icons 
        :type="item.customData?.mobileIcon || 'home'" 
        :size="24" 
        :color="isActive(item) ? '#007AFF' : '#666'"
        class="menu-icon"
      />
      <view class="menu-text">{{ item.displayName }}</view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { useAppStore } from '@/stores/app'
import { filterMainMenus } from '@/utils/menu'
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

const isActive = (item: MenuItemDto) => {
  if (!item.url) return false
  let url = item.url
  
  if (!url.startsWith('/pages/')) {
    if (url.startsWith('/')) {
      url = '/pages' + url
    } else {
      url = '/pages/' + url
    }
  }
  
  if (!url.endsWith('/index')) {
    url = url + '/index'
  }
  
  return currentPath.value === url
}

const menuItems = computed(() => {
  const menu = appStore.menu
  const dynamicMenus = menu && Object.keys(menu).length > 0 ? filterMainMenus(menu) : []
  
  const fixedMenus: MenuItemDto[] = [
    {
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
  console.log('[BottomMenu] 点击菜单项:', item)
  
  if (item.url) {
    let url = item.url
    
    console.log('[BottomMenu] 原始 URL:', url)
    
    if (!url.startsWith('/pages/')) {
      if (url.startsWith('/')) {
        url = '/pages' + url
      } else {
        url = '/pages/' + url
      }
    }
    
    if (!url.endsWith('/index')) {
      url = url + '/index'
    }
    
    console.log('[BottomMenu] 转换后 URL:', url)
    
    uni.redirectTo({
      url: url,
      animationType: 'fade',
      animationDuration: 150,
      success: () => {
        console.log('[BottomMenu] 跳转成功')
      },
      fail: (err) => {
        console.error('[BottomMenu] 跳转失败:', err)
        uni.showToast({
          title: '页面不存在',
          icon: 'none'
        })
      }
    })
  } else {
    console.log('[BottomMenu] 菜单项没有 URL')
  }
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
  color: #007AFF;
  font-weight: 600;
}
</style>
