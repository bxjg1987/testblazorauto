<script setup lang="ts">
/** 页面路由配置 */
definePage({
  meta: {
    layout: 'default',
    showNavbar: true,
    showTabbar: true,
    requiresAuth: true,
    title: '个人中心',
  },
})

/** 个人中心页面 */

const userStore = useUserStore()

/** 处理登出 */
const handleLogout = () => {
  uni.showModal({
    title: '提示',
    content: '确定要退出登录吗？',
    success: (res) => {
      if (res.confirm) {
        const { logout } = useAuth()
        logout()
      }
    },
  })
}
</script>

<template>
  <view class="p-4">
    <view class="text-center py-8">
      <wd-icon name="user" size="64px" />
      <view class="mt-4">
        <text class="text-lg font-bold">{{ userStore.nickname || userStore.username || '未登录' }}</text>
      </view>
    </view>

    <wd-cell-group title="用户信息">
      <wd-cell title="用户名" :value="userStore.username" />
      <wd-cell title="昵称" :value="userStore.nickname" />
      <wd-cell title="邮箱" :value="userStore.userInfo?.email || '-'" />
      <wd-cell title="手机" :value="userStore.userInfo?.phone || '-'" />
    </wd-cell-group>

    <view class="mt-8">
      <wd-button type="error" block @click="handleLogout">退出登录</wd-button>
    </view>
  </view>
</template>
