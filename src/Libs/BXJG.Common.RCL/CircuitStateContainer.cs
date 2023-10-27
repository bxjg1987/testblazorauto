using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.RCL
{
    /*
     * blazor server 线路
     * blazor server 通过signalR建立长连接，形成一个线路，叫：Circuit
     * 
     * 我们的界面组件之间的交互可以通过事件总线（可以也可以不使用我们的Zhongjie），这种情况页面可以在根组件通过级联参数共享同一个Zhongjie实例，让组件之间彼此通信。
     * 
     * 还有一种场景是业务逻辑层希望触发一些事件，导致界面更新，
     * 这又分为两种情况
     * 1、业务逻辑层发送通知，通过自定义实时通知显示业务通知
     * 2、业务逻辑层产生事件，不发送通知，但希望界面理解更新。
     * 
     * 这些情况，我们可以让线路关联一个Zhongjie实例，来实现上面的功能。虽然可以使用全局的Zhongjie，不过那样全局Zhongjie中会存在太多数据，担心查询变慢。
     * 所以每个线路一个Zhongjie实例更好。
     * 
     * 如果用户登陆了，线路应该关联到一个用户id，若未登录则是匿名用户，无需关联。
     * 这样可以反过来，通过用户id去拿到所有线路，进而拿线路关联的zhongjie实例,上面通知到用户就是通过此方式实现的。
     * 
     * 总的来说，界面之间组件的交互可以通过根组件使用级联参数方式交互，而业务逻辑层与界面交互需要通过中间对象（用户id和zhongjie实例）
     * 
     * 我们定义一个线路状态容器，它已线路对象为key，dictionary<string,object>为值，之所以使用字典作为value是更通用，因为这是定义在公共库的。
     * 一些常用的字段可以定义为强类型
     * 那么value用dictionary<string,object>会不会有线程安全问题？
     * 一般来说线路状态仅仅用来存储一些开始初始化好的数据，大部分情况仅仅是读取，用非线程安全的dictionary<string,object>效率应该更高
     * 
     * 参考资源：https://learn.microsoft.com/zh-cn/aspnet/core/blazor/fundamentals/signalr?view=aspnetcore-7.0#server-side-circuit-handler
     */

    //此对象还可以进一步抽象，且分为两种：
    //一种是不自动注销的，
    //另一种是类似缓存，需要滑动过期的或绝对过期的，因为这种内部一定会开启线程，有点耗费资源，所以分为两种

    /// <summary>
    /// blazor server 线路状态容器，它存储一些与线路及其关联的状态
    /// 此对象应该单例注册到ioc容器
    /// key线路实例
    /// value线路状态字典，非线程安全的
    /// 由于是ui方面的东东，它不是线程安全的，所以类似遍历它有几率报错
    /// </summary>
    public class CircuitStateContainer : Dictionary<Circuit, Dictionary<string, object>>//,IDisposable
    {
        public const string userId = "userId";
        public const string zhongjie = "zhongjie";
        /// <summary>
        /// 获取与线路关联的用户id
        /// </summary>
        public object GetUserId(Circuit circuit)
        {
            if (this.TryGetValue(circuit, out var cir))
            {
                if (cir.TryGetValue(userId, out var uid))
                {
                    return uid;
                }
            }
            return default;
        }
        /// <summary>
        /// 设置与线路关联的用户id
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="value"></param>
        public void SetUserId(Circuit circuit, object value)
        {
            this[circuit][userId] = value;
        }
        /// <summary>
        /// 获取与线路关联的中介（事件总线）实例
        /// </summary>
        public Zhongjie GetZhongjie(Circuit circuit)
        {
            if (this.TryGetValue(circuit, out var cir))
            {
                if (cir.TryGetValue(zhongjie, out var obj))
                {
                    return obj as Zhongjie;
                }
            }
            return default;
        }
        /// <summary>
        /// 设置与线路关联的中介（事件总线）实例
        /// </summary>
        /// <param name="circuit"></param>
        /// <param name="value"></param>
        public void SetZhongjie(Circuit circuit, object value)
        {
            this[circuit][zhongjie] = value;
        }
        /// <summary>
        /// 根据用户id获取关联的线路及其关联信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<Circuit, Dictionary<string, object>>> GetByUserId(object obj)
        {
            return GetByValue(userId, obj);
        }
        /// <summary>
        /// 根据中介(事件总线)实例获取关联的线路及其关联信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<Circuit, Dictionary<string, object>>> GetByZhongjie(object obj)
        {
            return GetByValue(zhongjie, obj);
        }
        /// <summary>
        /// 根据值获取线路及其关联的信息
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<Circuit, Dictionary<string, object>>> GetByValue(string propertyName, object value)
        {
            return this.Where(c => value != default && c.Value.ContainsKey(propertyName) && c.Value[propertyName] != default && c.Value[propertyName].Equals(value));
        }
    }
}