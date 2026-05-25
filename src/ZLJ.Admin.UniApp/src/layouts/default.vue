<script lang="ts">
export default {
  options: {
    addGlobalClass: true,
    virtualHost: true,
    styleIsolation: 'shared',
  },
}
</script>

<script setup lang="ts">
/** 侧边菜单开关状态 */
const sideMenuOpen = ref(false)

/** 当前路由信息 */
const route = useRoute()

/** 是否显示导航栏 */
const showNavbar = computed(() => {
  return route.meta?.showNavbar !== false
})

/** 是否显示底部TabBar */
const showTabbar = computed(() => {
  return route.meta?.showTabbar !== false
})

/** 导航栏标题 */
const navbarTitle = computed(() => {
  return (route.meta?.title as string) || ''
})

/** 切换侧边菜单 */
const toggleSideMenu = () => {
  sideMenuOpen.value = !sideMenuOpen.value
}
</script>

<template>
  <view class="default-layout">
    <!-- 顶部导航栏 -->
    <wd-navbar
      v-if="showNavbar"
      :title="navbarTitle"
      :fixed="true"
      :placeholder="true"
      :safeAreaInsetTop="true"
      custom-style="background-color: #fff;"
    >
      <template #left>
        <view @click="toggleSideMenu">
          <wd-icon name="menu" size="22px" />
        </view>
      </template>
    </wd-navbar>

    <!-- 页面内容 -->
    <view class="layout-content">
      <slot />
    </view>

    <!-- 底部导航 -->
    <BottomNav v-if="showTabbar" />

    <!-- 侧边菜单 -->
    <SideMenu :open="sideMenuOpen" @close="sideMenuOpen = false" />
  </view>
</template>

<style scoped>
.default-layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background-color: #f5f5f5;
}

.layout-content {
  flex: 1;
  overflow-y: auto;
}
</style>
