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
import { navigateToMenuItem } from '@/utils/menu'
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
  if (item.customData && item.customData.mobileIcon) {
    return item.customData.mobileIcon
  }

  const name = item.name?.toLowerCase() || ''
  const url = item.url?.toLowerCase() || ''

  if (name.includes('setting') || url.includes('setting')) return 'settings'
  if (name.includes('staff') || url.includes('staff') || name.includes('employee')) return 'staff'
  if (name.includes('warehouse') || url.includes('warehouse')) return 'shop'
  if (name.includes('equipment') || url.includes('equipment') || name.includes('shebei')) return 'gear'
  if (name.includes('workorder') || url.includes('workorder') || name.includes('工单')) return 'notification'
  if (name.includes('rent') || url.includes('rent') || name.includes('租赁')) return 'calendar'
  if (name.includes('meter') || url.includes('meter') || name.includes('抄表')) return 'info'
  if (name.includes('administrative') || url.includes('administrative') || name.includes('行政')) return 'location'
  if (name.includes('tenant') || url.includes('tenant') || name.includes('租户')) return 'vip'
  if (name.includes('company') || url.includes('company') || name.includes('单位')) return 'contact'
  if (name.includes('hangfire') || url.includes('hangfire') || name.includes('作业')) return 'refresh'
  if (name.includes('ycsdk') || url.includes('ycsdk')) return 'cloud-upload'
  if (name.includes('product') || url.includes('product') || name.includes('产品') || name.includes('category')) return 'list'
  if (name.includes('inventory') || url.includes('inventory') || name.includes('库存')) return 'wallet'
  if (name.includes('inbound') || url.includes('inbound') || name.includes('入库')) return 'download'
  if (name.includes('outbound') || url.includes('outbound') || name.includes('出库')) return 'upload'
  if (name.includes('transfer') || url.includes('transfer') || name.includes('调拨')) return 'loop'
  if (name.includes('apply') || url.includes('apply') || name.includes('申请')) return 'compose'
  if (name.includes('change') || url.includes('change') || name.includes('变更')) return 'redo'
  if (name.includes('dictionary') || url.includes('dictionary') || name.includes('字典')) return 'help'
  if (name.includes('organization') || url.includes('organization') || name.includes('组织')) return 'bars'
  if (name.includes('post') || url.includes('post') || name.includes('岗位')) return 'person'
  if (name.includes('profile') || url.includes('profile') || name.includes('个人')) return 'person-filled'
  if (name.includes('main') || url.includes('main') || name.includes('首页')) return 'home'
  if (name.includes('login') || url.includes('login') || name.includes('登录')) return 'auth'
  if (name.includes('init') || url.includes('init') || name.includes('初始化')) return 'gear-filled'
  if (name.includes('statistics') || url.includes('statistics') || name.includes('统计')) return 'info-filled'
  if (name.includes('client') || url.includes('client') || name.includes('客户端')) return 'phone'
  if (name.includes('server') || url.includes('server') || name.includes('fwq') || name.includes('服务器')) return 'cloud'
  if (name.includes('host') || url.includes('host') || name.includes('主机')) return 'videocam'
  if (name.includes('tunnel') || url.includes('tunnel') || name.includes('隧道')) return 'link'
  if (name.includes('baseinfo') || url.includes('baseinfo') || name.includes('基础')) return 'folder'
  if (name.includes('contract') || url.includes('contract') || name.includes('合同')) return 'paperplane'

  return 'file'
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
  handleClose()
  navigateToMenuItem(item)
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
