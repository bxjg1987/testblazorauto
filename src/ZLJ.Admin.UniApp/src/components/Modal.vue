<template>
  <uni-popup ref="popup" type="dialog">
    <uni-popup-dialog
      :type="type"
      :title="title"
      :content="content"
      :confirm-text="confirmText"
      :cancel-text="cancelText"
      :show-cancel="showCancel"
      @confirm="handleConfirm"
      @close="handleClose"
    >
      <slot></slot>
    </uni-popup-dialog>
  </uni-popup>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'

interface Props {
  visible: boolean
  title?: string
  confirmText?: string
  cancelText?: string
  showCancel?: boolean
  type?: 'success' | 'warning' | 'info' | 'error'
  content?: string
}

const props = withDefaults(defineProps<Props>(), {
  title: '提示',
  confirmText: '确定',
  cancelText: '取消',
  showCancel: true,
  type: 'info',
})

const emit = defineEmits<{
  (e: 'confirm'): void
  (e: 'cancel'): void
  (e: 'update:visible', value: boolean): void
}>()

const popup = ref()

watch(() => props.visible, (newVal) => {
  if (newVal) {
    popup.value?.open()
  } else {
    popup.value?.close()
  }
})

const handleConfirm = () => {
  emit('confirm')
  emit('update:visible', false)
}

const handleClose = () => {
  emit('cancel')
  emit('update:visible', false)
}
</script>
