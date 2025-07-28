# zlj_miniprogram_libs

微信小程序HTTP请求库，基于`we-req`封装，提供了统一的请求拦截和响应处理。

## 安装

```bash
npm install zlj_miniprogram_libs
```

## 使用

### 基本使用

```javascript
import httphelper from 'zlj_miniprogram_libs';

// 发起GET请求
httphelper.get('/api/users').then(res => {
  console.log(res);
});

// 发起POST请求
httphelper.post('/api/users', { name: 'John' }).then(res => {
  console.log(res);
});
```

### 高级配置

```javascript
import { createInstance } from 'zlj_miniprogram_libs';

// 创建自定义实例
const customHttp = createInstance({
  baseURL: 'https://your-api-domain.com',
  timeout: 10000,
  // 获取访问令牌的函数
  getAccessToken: () => {
    return wx.getStorageSync('accessToken');
  },
  // 处理未授权状态的函数
  handleUnauthorized: (res) => {
    wx.showToast({
      title: '请重新登录',
      icon: 'none'
    });
    // 跳转到登录页
    wx.redirectTo({
      url: '/pages/login/index'
    });
  }
});

// 使用自定义实例
customHttp.get('/api/users').then(res => {
  console.log(res);
});
```

## 功能特性

- 统一的请求拦截和响应处理
- 自动处理ABP框架响应格式
- 支持批量操作结果处理
- 可自定义配置
- 支持处理401未授权状态

## 依赖

- [we-req](https://www.npmjs.com/package/we-req)
- [dayjs](https://www.npmjs.com/package/dayjs)
- [pubsub-js](https://www.npmjs.com/package/pubsub-js)