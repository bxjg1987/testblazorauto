// 微信小程序HTTP请求库
// 基于we-req封装，提供了统一的请求拦截和响应处理

import Request from 'we-req'

// 创建默认实例
const createInstance = (options = {}) => {
  return Request.init({
    ...options,
    interceptors: {
      // 请求成功拦截器
      requestSuccessFn: (config) => {
        // 在请求发送前可以对 config 进行修改 设置token之类的
        config.header = config.header || {}
        config.header['Accept-Language'] = 'zh-CN,zh;q=0.9';
        
        // 如果提供了获取token的方法，则设置Authorization头
        if (options.getAccessToken) {
          const key = options.getAccessToken()
          config.header.Authorization = `Bearer ${key}`
        }
        
        return config;
      },
      //请求失败拦截器
      requestFailFn: (err) => {
        setTimeout(() => {
          wx.showToast({
            title: '请求失败',
            icon: 'error'
          })
        }, 0);
      },
      //响应失败拦截器
      async responseFailFn(err) {
        setTimeout(() => {
          wx.showToast({
            title: '响应失败',
            icon: 'error'
          })
        }, 0);
      },
      //响应成功拦截器
      async responseSuccessFn(res) {
        if (!res || !res.data)
          return;

        // 处理401未授权状态
        if (res.statusCode == 401) {
          // 如果提供了处理未授权的方法，则调用它
          if (options.handleUnauthorized) {
            options.handleUnauthorized(res);
            return;
          }
          
          // 默认处理：显示提示
          setTimeout(() => {
            wx.showToast({
              title: '请登录',
              icon: 'none'
            })
          }, 200);
          return;
        }

        // 处理ABP框架响应
        if (res.data.__abp) {
          if (res.data.success) {
            // 处理批量操作结果
            if (res.data.result && 'errorMessage' in res.data.result && 'ids' in res.data.result) {
              var msg = '';
              res.data.result.errorMessage.forEach((value, index, arr) => {
                msg += `编号${value.id} ${value.message}\r\n`;
              });
              
              if (msg) {
                wx.showModal({
                  title: res.data.result.ids && res.data.result.ids.length > 0 ? '部分操作失败！' : '操作失败',
                  content: msg,
                  showCancel: false
                })
              }

              if (res.data.result.ids.length > 0)
                return res.data.result.ids;
              return false;
            }
            
            // 返回成功结果
            return res.data.result || true;
          } else {
            // 处理ABP错误响应
            setTimeout(() => {
              wx.showModal({
                title: res.data.error.message,
                content: res.data.error.details || '',
                showCancel: false
              })
            }, 0);
            return false;
          }
        }
      }
    },
    complete() {
      // 响应完成处理
    }
  });
};

// 创建默认实例
const defaultInstance = createInstance({
  baseURL: 'https://pc.sfxc.co:22000/',
  timeout: 20000,
  loading: true
});

// 导出默认实例和创建实例的方法
export default defaultInstance;
export { createInstance };
  //baseURL: 'http://192.168.2.5:21021/',
  baseURL: 'https://pc.sfxc.co:22000/',
  //baseURL:'http://localhost:21021/',
  timeout: 20000,
  loading: true, //开启全局加载弹窗
  interceptors: { // 响应成功拦截器
    // 请求成功拦截器
    requestSuccessFn: (config) => {
      // 在请求发送前可以对 config 进行修改 设置token之类的
      //console.log('请求前，请求头设置');
      config.header = {}
      config.header['Accept-Language'] = 'zh-CN,zh;q=0.9';
      const key = wx.getStorageSync('accessToken')
      config.header.Authorization = `Bearer ${key}`
      //console.log(config.headers);
      return config;
    },
    //请求失败拦截器 // 可选：处理请求失败的情况，如记录日志或提示用户
    requestFailFn: (err) => {
      //console.error('请求失败，requestFailFn');
      setTimeout(() => {
        wx.showToast({
          title: '请求失败',
          icon: 'error'
        })
      }, 0);
    },
    //响应失败拦截器
    async responseFailFn(err) {
      //console.log('响应失败 responseFailFn');
      setTimeout(() => {
        wx.showToast({
          title: '响应失败',
          icon: 'error'
        })
      }, 0);
    },
    async responseSuccessFn(res) {

      //这个库貌似有bug，这里加个状态，若拦截器已执行，则跳过
      //辣鸡，无论成功还是失败，这里都会执行 且res为空，同时响应失败的拦截器也会执行
      if (!res || !res.data)
        return;
      //res.__ljqyzx = true;

      //console.log('响应成功responseSuccessFn，数据如下');
      //console.log(res);
      const app = getApp();
      if (res.statusCode == 401) { //若是没登陆，直接跳转到登录页去 
        // 若并发请求时 会多次跳转到登录页，这里加个判断，另外登录页中要处理此状态
        if (!app.globalData.tzdly) {
          app.globalData.tzdly = true;
          setTimeout(() => {
            wx.showToast({
              title: '请登录',
              icon: 'none'
            })

            wx.redirectTo({
              url: '/pages/login/index'
            });
          }, 200);

        
        }
        // wx.redirectTo({
        //   url: '/pages/login/index'
        // });
        return;
      }

      //abp错误会响应500
      // if (res.statusCode != 200) {
      //   setTimeout(() => {
      //     wx.showToast({
      //       title: '响应码' + res.statusCode,
      //       icon: 'error'
      //     })
      //   }, 0);
      // }
      // 在收到响应后可以对 res 进行处理或转换
      if (res.data.__abp) {

        if (res.data.success) {
          //console.log('abp响应成功');
          //console.log(res.data.result);
          //res.data.result.__ljqyzx = true;
          //console.log('dfsdf333');

          //是批量操作
          if ( res.data.result&& 'errorMessage' in res.data.result && 'ids' in res.data.result) {
            //console.log('批量操作');
            //console.log(res.data.result.errorMessage);
            //console.log('xxxx');
            //var msgAry = res.data.result.errorMessage.map(x=>`编号${ x.id } ${x.message}`);
            var msg = '';
            res.data.result.errorMessage.forEach((value, index, arr) => {
              msg += `编号${value.id} ${value.message}\r\n`;
            });
            //console.log(msg);
            //console.log(res.data.result.errorMessage);
            if (msg) {
              wx.showModal({
                title: res.data.result.ids && res.data.result.ids.length > 0 ? '部分操作失败！' : '操作失败',
                content: msg,
                showCancel: false,
                // complete: (res) => {
                //   if (res.cancel) {
                //   }
                //   if (res.confirm) {
                //   }
                // }
              })
            }

            if (res.data.result.ids.length > 0)
              return res.data.result.ids;
            return false;
          }
          //console.log('非批量操作返回成功');
          return res.data.result || true;
        } else {
          
          setTimeout(() => {
            // wx.showToast({
            //   title: '解包失败' + res.data.error.message,
            //   icon: 'error'
            // })
            //console.log('响应失败1：')
            //console.log(res)
            wx.showModal({
              title: res.data.error.message,
              content: res.data.error.details || '',
              showCancel: false
            })
          }, 0);
          return false;
        }
      }
      //否则不反悔，则调用方判断结果也是失败
    }
  },
  complete() {
    //失败时要显示失败提示，这里别关
    //console.log('响应结束');
    //   wx.hideToast();
  }
});