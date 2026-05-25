<script setup lang="ts">
import type { MenuItemDto, MenuGroupDto } from '@/types/menu'
import { useAppStore } from '@/store/app'

/** 侧边菜单组件 */

/** 组件属性 */
interface Props {
  /** 是否打开 */
  open: boolean
}

const props = defineProps<Props>()

const emit = defineEmits<{
  /** 关闭事件 */
  close: []
}>()

const appStore = useAppStore()

/** 状态栏高度 */
const statusBarHeight = ref(0)

/** 展开的子菜单 */
const expandedMenus = ref<Set<string>>(new Set())

/** 按分组整理的侧边菜单（仅包含有侧边菜单项的分组） */
const groupedSideMenus = computed(() => {
  const groups: Array<{ name: string; displayName: string; items: MenuItemDto[] }> = []
  const menu = appStore.menu
  for (const key of Object.keys(menu)) {
    const group = menu[key]
    if (!group?.items?.length) continue
    const sideItems = filterSideItemsFromGroup(group)
    if (sideItems.length > 0) {
      groups.push({
        name: key,
        displayName: group.displayName || key,
        items: sideItems,
      })
    }
  }
  return groups
})

/** 从单个分组中筛选侧边菜单项 */
function filterSideItemsFromGroup(group: MenuGroupDto): MenuItemDto[] {
  if (!group.items) return []
  const allItems = collectLeafItems(group.items)
  return allItems.filter(
    (item) =>
      item.customData?.mobileShowModel === 4 ||
      item.customData?.mobileShowModel === undefined,
  )
}

/** 递归收集所有叶子菜单项 */
function collectLeafItems(items: MenuItemDto[]): MenuItemDto[] {
  const result: MenuItemDto[] = []
  for (const item of items) {
    if (item.items && item.items.length > 0) {
      result.push(...collectLeafItems(item.items))
    } else {
      result.push(item)
    }
  }
  return result
}

/** 切换子菜单展开 */
const toggleSubmenu = (itemName: string) => {
  const newSet = new Set(expandedMenus.value)
  if (newSet.has(itemName)) {
    newSet.delete(itemName)
  } else {
    newSet.add(itemName)
  }
  expandedMenus.value = newSet
}

/** 处理菜单项点击 */
const handleMenuClick = (item: MenuItemDto) => {
  if (item.items && item.items.length > 0) {
    toggleSubmenu(item.name)
  } else {
    emit('close')
    navigateToMenuItem(item)
  }
}

/** 导航到菜单项 */
function navigateToMenuItem(item: MenuItemDto) {
  const url = item.url
  if (!url) return
  let path = url
  if (!path.startsWith('/pages/')) {
    path = path.startsWith('/') ? `/pages${path}` : `/pages/${path}`
  }
  router
    .push(path)
    .catch((err: unknown) => {
      const errMsg = err instanceof Error ? err.message : String(err)
      if (errMsg.includes('not found') || errMsg.includes('不存在')) {
        uni.showToast({ title: '页面不存在', icon: 'none' })
      }
    })
}

/** 处理遮罩点击 */
const handleMaskClick = () => {
  emit('close')
}

onMounted(() => {
  const systemInfo = uni.getSystemInfoSync()
  statusBarHeight.value = systemInfo.statusBarHeight || 0
})
</script>

<template>
  <wd-popup :model-value="props.open" position="left" :custom-style="{ width: '70vw', height: '100vh' }" @close="handleMaskClick">
    <view class="side-menu-content" :style="{ paddingTop: statusBarHeight + 'px' }">
      <scroll-view scroll-y class="side-menu-scroll">
        <wd-cell-group v-for="group in groupedSideMenus" :key="group.name" :title="group.displayName">
          <template v-for="item in group.items" :key="item.name">
            <!-- 叶子菜单项 -->
            <wd-cell v-if="!item.items || item.items.length === 0" :title="item.displayName" is-link @click="handleMenuClick(item)" />
            <!-- 带子菜单的父菜单项 -->
            <template v-else>
              <wd-cell :title="item.displayName" is-link @click="toggleSubmenu(item.name)" />
              <template v-if="expandedMenus.has(item.name)">
                <wd-cell v-for="subItem in item.items" :key="subItem.name" :title="subItem.displayName" is-link @click="handleMenuClick(subItem)" />
              </template>
            </template>
          </template>
        </wd-cell-group>
      </scroll-view>
    </view>
  </wd-popup>
</template>

<style scoped>
.side-menu-content {
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.side-menu-scroll {
  flex: 1;
  overflow: hidden;
}
</style>
