<script setup lang="ts">
/** 页面路由配置 */
definePage({
  meta: {
    layout: 'default',
    showNavbar: true,
    showTabbar: true,
    requiresAuth: true,
    title: '管理',
  },
})

/** 后台管理主页 */
</script>

<template>
  <view class="p-4">
    <view class="text-center py-8">
      <text class="text-xl font-bold">后台管理</text>
    </view>
    <wd-cell-group title="管理功能">
      <wd-cell title="租户管理" is-link to="/pages/tenant" />
      <wd-cell title="组织架构" is-link to="/pages/organization-unit" />
      <wd-cell title="岗位管理" is-link to="/pages/post" />
    </wd-cell-group>
  </view>
</template>
