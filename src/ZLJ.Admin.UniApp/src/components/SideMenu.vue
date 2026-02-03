<template>
  <view class="side-menu" :class="{ 'side-menu--open': isOpen }">
    <view class="side-menu-mask" @click="handleClose"></view>
    <view class="side-menu-content" :style="{ paddingTop: statusBarHeight + 'px' }">
      <scroll-view class="side-menu-scroll" scroll-y>
        <uni-list :border="false">
          <template v-for="(group, groupKey) in menuGroups" :key="groupKey">
            <template v-for="item in group.items" :key="item.name">
              <uni-list-item
                v-if="!item.items || item.items.length === 0"
                :title="item.displayName"
                :clickable="true"
                @click="handleMenuClick(item)"
              >
                <template v-slot:header>
                  <uni-icons :type="getDefaultIcon(item)" size="18" color="#666" style="margin-right: 3rpx;" />
                </template>
                <template v-slot:footer>
                  <uni-icons type="right" size="16" color="#999" />
                </template>
              </uni-list-item>
              
              <uni-list-item
                v-else
                :title="item.displayName"
                :clickable="true"
                class="menu-sub-title"
                @click="handleSubmenuToggle(item)"
              >
                <template v-slot:header>
                  <uni-icons :type="getDefaultIcon(item)" size="18" color="#666" style="margin-right: 3rpx;" />
                </template>
                <template v-slot:footer>
                  <uni-icons :type="expandedMenus.has(item.name) ? 'down' : 'right'" size="16" color="#999" />
                </template>
              </uni-list-item>
              
              <uni-list-item
                v-if="expandedMenus.has(item.name)"
                v-for="subItem in item.items"
                :key="subItem.name"
                :title="subItem.displayName"
                :clickable="true"
                class="menu-sub-item"
                @click="handleMenuClick(subItem)"
              >
                <template v-slot:header>
                  <uni-icons :type="getDefaultIcon(subItem)" size="18" color="#666" style="margin-right: 3rpx;" />
                </template>
                <template v-slot:footer>
                  <uni-icons type="right" size="16" color="#999" />
                </template>
              </uni-list-item>
            </template>
          </template>
        </uni-list>
      </scroll-view>
    </view>
  </view>
</template>

<script setup lang="ts">
import { computed, ref, onMounted } from 'vue'
import { useAppStore } from '@/stores/app'
import type { MenuItemDto } from '@/types/menu'

interface Props {
  open: boolean
}

const props = defineProps<Props>()

const emit = defineEmits<{
  close: []
}>()

const appStore = useAppStore()
const statusBarHeight = ref(0)
const expandedMenus = ref<Set<string>>(new Set())

onMounted(() => {
  const systemInfo = uni.getSystemInfoSync()
  statusBarHeight.value = systemInfo.statusBarHeight || 0
})

const isOpen = computed(() => props.open)

const menuGroups = computed(() => {
  const menu = appStore.menu
  if (!menu || Object.keys(menu).length === 0) {
    return {}
  }
  return menu
})

const getDefaultIcon = (item: MenuItemDto): string => {
  const name = item.name?.toLowerCase() || ''
  const url = item.url?.toLowerCase() || ''
  
  let icon = 'file'
  
  if (item.customData && item.customData.mobileIcon) {
    console.log('[SideMenu] 使用 mobileIcon:', item.customData.mobileIcon, '菜单:', item.displayName)
    return item.customData.mobileIcon
  }
  
  console.log('[SideMenu] mobileIcon 不存在或为空，使用默认图标，菜单:', item.displayName, 'name:', name, 'url:', url)
  
  if (name.includes('setting') || url.includes('setting')) icon = 'settings'
  else if (name.includes('staff') || url.includes('staff') || name.includes('employee')) icon = 'staff'
  else if (name.includes('warehouse') || url.includes('warehouse')) icon = 'shop'
  else if (name.includes('equipment') || url.includes('equipment') || name.includes('shebei')) icon = 'gear'
  else if (name.includes('workorder') || url.includes('workorder') || name.includes('工单')) icon = 'notification'
  else if (name.includes('rent') || url.includes('rent') || name.includes('租赁')) icon = 'calendar'
  else if (name.includes('meter') || url.includes('meter') || name.includes('抄表')) icon = 'info'
  else if (name.includes('administrative') || url.includes('administrative') || name.includes('行政')) icon = 'location'
  else if (name.includes('tenant') || url.includes('tenant') || name.includes('租户')) icon = 'vip'
  else if (name.includes('company') || url.includes('company') || name.includes('单位')) icon = 'contact'
  else if (name.includes('hangfire') || url.includes('hangfire') || name.includes('作业')) icon = 'refresh'
  else if (name.includes('ycsdk') || url.includes('ycsdk')) icon = 'cloud-upload'
  else if (name.includes('product') || url.includes('product') || name.includes('产品') || name.includes('category')) icon = 'list'
  else if (name.includes('inventory') || url.includes('inventory') || name.includes('库存')) icon = 'wallet'
  else if (name.includes('inbound') || url.includes('inbound') || name.includes('入库')) icon = 'download'
  else if (name.includes('outbound') || url.includes('outbound') || name.includes('出库')) icon = 'upload'
  else if (name.includes('transfer') || url.includes('transfer') || name.includes('调拨')) icon = 'loop'
  else if (name.includes('apply') || url.includes('apply') || name.includes('申请')) icon = 'compose'
  else if (name.includes('change') || url.includes('change') || name.includes('变更')) icon = 'redo'
  else if (name.includes('dictionary') || url.includes('dictionary') || name.includes('字典')) icon = 'help'
  else if (name.includes('organization') || url.includes('organization') || name.includes('组织')) icon = 'bars'
  else if (name.includes('post') || url.includes('post') || name.includes('岗位')) icon = 'person'
  else if (name.includes('profile') || url.includes('profile') || name.includes('个人')) icon = 'person-filled'
  else if (name.includes('main') || url.includes('main') || name.includes('首页')) icon = 'home'
  else if (name.includes('login') || url.includes('login') || name.includes('登录')) icon = 'auth'
  else if (name.includes('init') || url.includes('init') || name.includes('初始化')) icon = 'gear-filled'
  else if (name.includes('statistics') || url.includes('statistics') || name.includes('统计')) icon = 'info-filled'
  else if (name.includes('client') || url.includes('client') || name.includes('客户端')) icon = 'phone'
  else if (name.includes('server') || url.includes('server') || name.includes('fwq') || name.includes('服务器')) icon = 'cloud'
  else if (name.includes('host') || url.includes('host') || name.includes('主机')) icon = 'videocam'
  else if (name.includes('tunnel') || url.includes('tunnel') || name.includes('隧道')) icon = 'link'
  else if (name.includes('baseinfo') || url.includes('baseinfo') || name.includes('基础')) icon = 'folder'
  else if (name.includes('contract') || url.includes('contract') || name.includes('合同')) icon = 'paperplane'
  
  console.log('[SideMenu] 使用默认图标:', icon, '菜单:', item.displayName)
  return icon
}

const handleClose = () => {
  emit('close')
}

const handleSubmenuToggle = (item: MenuItemDto) => {
  if (expandedMenus.value.has(item.name)) {
    expandedMenus.value.delete(item.name)
  } else {
    expandedMenus.value.add(item.name)
  }
  expandedMenus.value = new Set(expandedMenus.value)
}

const handleMenuClick = (item: MenuItemDto) => {
  console.log('[SideMenu] 点击菜单项:', item)
  
  if (item.url) {
    let url = item.url
    
    console.log('[SideMenu] 原始 URL:', url)
    
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
    
    console.log('[SideMenu] 转换后 URL:', url)
    
    handleClose()
    
    uni.redirectTo({
      url: url,
      success: () => {
        console.log('[SideMenu] 跳转成功')
      },
      fail: (err) => {
        console.error('[SideMenu] 跳转失败:', err)
        uni.showToast({
          title: '页面不存在',
          icon: 'none'
        })
      }
    })
  } else {
    console.log('[SideMenu] 菜单项没有 URL')
  }
}
</script>

<style scoped>
.side-menu {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 1000;
  pointer-events: none;
}

.side-menu--open {
  pointer-events: auto;
}

.side-menu-mask {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  opacity: 0;
  transition: opacity 0.3s;
}

.side-menu--open .side-menu-mask {
  opacity: 1;
}

.side-menu-content {
  position: absolute;
  top: 0;
  left: 0;
  bottom: 0;
  width: 600rpx;
  background: #ffffff;
  transform: translateX(-100%);
  transition: transform 0.3s;
  display: flex;
  flex-direction: column;
}

.side-menu--open .side-menu-content {
  transform: translateX(0);
}

.side-menu-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 30rpx;
  border-bottom: 1rpx solid #f0f0f0;
}

.side-menu-title {
  font-size: 36rpx;
  font-weight: 600;
  color: #333;
}

.side-menu-close {
  width: 60rpx;
  height: 60rpx;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  background: #f5f5f5;
}

.side-menu-scroll {
  flex: 1;
  overflow-y: auto;
}

.menu-group-title {
  background: #f8f8f8;
}

.menu-group-title :deep(.uni-list-item__content-title) {
  font-size: 30rpx;
  font-weight: 600;
  color: #333;
}

.menu-sub-title {
  background: #fafafa;
}

.menu-sub-title :deep(.uni-list-item__content-title) {
  font-size: 28rpx;
  font-weight: 500;
  color: #666;
}

.menu-sub-item :deep(.uni-list-item__container) {
  padding-left: 80rpx;
}

.sub-item-icon {
  width: 36rpx;
  height: 36rpx;
  border-radius: 4rpx;
  background: #e0e0e0;
}

:deep(.uni-list-item__container) {
  padding-left: 16rpx;
}

:deep(.uni-list-item__header) {
  margin-right: 3rpx;
}
</style>
