<template>
  <PageLayout title="后台管理首页">
    <view class="content">
      <text class="title">后台管理首页</text>
    </view>
  </PageLayout>
</template>

<script setup lang="ts">
import PageLayout from '@/components/PageLayout.vue'
</script>

<style scoped>
.content {
  background: #ffffff;
  border-radius: 16rpx;
  padding: 40rpx;
}

.title {
  display: block;
  font-size: 32rpx;
  font-weight: 600;
  color: #333;
  text-align: center;
}
</style>
