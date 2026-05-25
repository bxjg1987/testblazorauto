import { getApiBaseUrl } from '@/utils/http'

/** 获取验证码图片地址 */
export function getCaptchaUrl(yzmKey: string): string {
  const baseUrl = getApiBaseUrl()
  return `${baseUrl}/api/Captcha/Captcha?id=${yzmKey}`
}
