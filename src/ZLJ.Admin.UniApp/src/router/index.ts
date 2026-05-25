/// <reference types="@uni-helper/vite-plugin-uni-pages/client" />
import { pages, subPackages } from 'virtual:uni-pages'
import { createRouter, useRoute } from '@wot-ui/router'
import { useUserStore } from '@/store/user'

/** 子包页面配置 */
interface SubPackagePage {
  path: string
  [key: string]: unknown
}

/** 子包配置 */
interface SubPackageConfig {
  root: string
  pages: SubPackagePage[]
  [key: string]: unknown
}

/** 生成路由配置 */
function generateRoutes() {
  const routes = pages.map((page) => {
    const newPath = `/${page.path}`
    return { ...page, path: newPath }
  })
  if (subPackages && subPackages.length > 0) {
    (subPackages as SubPackageConfig[]).forEach((subPackage) => {
      const subRoutes = subPackage.pages.map((page) => {
        const newPath = `/${subPackage.root}/${page.path}`
        return { ...page, path: newPath }
      })
      routes.push(...subRoutes)
    })
  }
  return routes
}

/** 路由实例 */
const router = createRouter({
  routes: generateRoutes(),
})

/** 无需登录即可访问的页面路径 */
const whiteList = ['/pages/login', '/pages/init', '/pages/index']

router.beforeEach((to, from, next) => {
  const userStore = useUserStore()
  if (whiteList.includes(to.path)) {
    next()
    return
  }
  if (to.meta?.requiresAuth && !userStore.isLoggedIn) {
    next({
      path: '/pages/login',
      query: { returnUrl: to.fullPath },
    })
    return
  }
  next()
})

router.afterEach((to) => {
  console.log(`页面切换: ${to.path}`)
})

export default router
