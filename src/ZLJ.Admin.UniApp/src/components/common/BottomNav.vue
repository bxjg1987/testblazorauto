<script setup lang="ts">
import type { MenuItemDto } from '@/types/menu'
import { normalizeMenuUrl } from '@/utils/menu'

/** 底部导航组件 */

const { bottomMenus, navigateTo } = useMenu()

/** 当前选中标签索引 */
const activeTab = ref(0)

/** 标签项接口 */
interface TabItem {
  /** 标签名称 */
  name: string
  /** 标签图标 */
  icon: string
  /** 标签路径 */
  path: string
  /** 关联菜单项 */
  menuItem: MenuItemDto | null
}

/** 固定标签项 */
const fixedTabs: TabItem[] = [
  { name: '首页', icon: 'home', path: '/pages/index', menuItem: null },
  { name: '我的', icon: 'user', path: '/pages/profile', menuItem: null },
]

/** 所有标签项（固定 + 动态菜单） */
const allTabs = computed(() => {
  const dynamicTabs = bottomMenus.value
    .filter(item => item.url)
    .map((item): TabItem => ({
      name: item.customData?.mobileText || item.displayName,
      icon: item.customData?.mobileIcon || 'list',
      path: item.url ? normalizeMenuUrl(item.url) : '',
      menuItem: item,
    }))
  return [...fixedTabs, ...dynamicTabs]
})

/** 切换标签 */
const handleTabChange = (index: number) => {
  activeTab.value = index
  const tab = allTabs.value[index]
  if (tab?.menuItem) {
    navigateTo(tab.menuItem)
  } else if (tab?.path) {
    uni.switchTab({ url: tab.path })
  }
}

/** 同步当前页面到激活标签 */
const syncActiveTab = () => {
  const pages = getCurrentPages()
  const currentPage = pages[pages.length - 1]
  if (!currentPage) return
  const currentPath = '/' + currentPage.route
  const index = allTabs.value.findIndex(tab => tab.path === currentPath)
  if (index >= 0) {
    activeTab.value = index
  }
}

onMounted(() => {
  syncActiveTab()
})
</script>

<template>
  <wd-tabbar v-model="activeTab" @change="handleTabChange" bordered fixed placeholder>
    <wd-tabbar-item v-for="(tab, index) in allTabs" :key="index" :title="tab.name" :icon="tab.icon" />
  </wd-tabbar>
</template>
