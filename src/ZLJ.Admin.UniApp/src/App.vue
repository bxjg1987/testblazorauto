<script setup lang="ts">
import { appInitializationService } from '@/services/app-initialization'
import { getApiBaseUrl } from '@/utils/http'

/** 应用启动时初始化 */
onLaunch(async () => {
  const apiBaseUrl = getApiBaseUrl()

  if (!apiBaseUrl || apiBaseUrl === '/api') {
    uni.reLaunch({ url: '/pages/init' })
    return
  }

  try {
    await appInitializationService.initializeOnLaunch()
  } catch (error) {
    console.error('[App] 初始化失败:', error)
  }
})
</script>

<style lang="scss">
@use '@wot-ui/ui/styles/theme/index.scss' as *;
.page-wraper {
  min-height: calc(100vh - var(--window-top));
  box-sizing: border-box;
  background: var(--wot-filled-content);
}
</style>
