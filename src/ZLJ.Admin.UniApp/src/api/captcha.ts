import { getApiBaseUrl } from '@/utils/http'

export function getCaptchaUrl(yzmKey: string): string {
  const baseUrl = getApiBaseUrl()
  return `${baseUrl}/api/Captcha/Captcha?id=${yzmKey}`
}
