<script setup lang="ts">
/** 页面路由配置 */
definePage({
  meta: {
    layout: 'default',
    showNavbar: true,
    showTabbar: true,
    requiresAuth: true,
    title: '首页',
  },
})

/** 首页 */
</script>

<template>
  <view class="p-4">
    <view class="text-center py-8">
      <text class="text-xl font-bold">欢迎使用 ZLJ Admin</text>
    </view>
    <wd-grid :column="3" clickable>
      <wd-grid-item icon="home" text="首页" />
      <wd-grid-item icon="user" text="个人中心" to="/pages/profile" />
      <wd-grid-item icon="setting" text="设置" to="/pages/init" />
    </wd-grid>
  </view>
</template>
