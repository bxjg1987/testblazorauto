/*
 * @Author: weisheng
 * @Date: 2025-06-23 22:23:05
 * @LastEditTime: 2025-06-27 13:04:54
 * @LastEditors: weisheng
 * @Description:
 * @FilePath: /wot-starter/pages.config.ts
 * 页面路由配置
 */
import { defineUniPages } from '@uni-helper/vite-plugin-uni-pages'

export default defineUniPages({
  pages: [],
  globalStyle: {
    navigationBarBackgroundColor: '@navBgColor',
    navigationBarTextStyle: '@navTxtStyle',
    navigationBarTitleText: 'ZLJ Admin',
    backgroundColor: '@bgColor',
    backgroundTextStyle: '@bgTxtStyle',
    backgroundColorTop: '@bgColorTop',
    backgroundColorBottom: '@bgColorBottom',
    enablePullDownRefresh: false,
    onReachBottomDistance: 50,
    animationType: 'pop-in',
    animationDuration: 300,
  },
  tabBar: {
    custom: true,
    // #ifdef MP-ALIPAY
    customize: true,
    overlay: true,
    // #endif
    height: '0',
    color: '@tabColor',
    selectedColor: '@tabSelectedColor',
    backgroundColor: '@tabBgColor',
    borderStyle: '@tabBorderStyle',
    list: [{
      pagePath: 'pages/index',
    }, {
      pagePath: 'pages/profile',
    }],
  },
})
